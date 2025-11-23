using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using DTwoMFTimerHelper.Services;
using DTwoMFTimerHelper.UI;
using DTwoMFTimerHelper.Utils;

namespace DTwoMFTimerHelper.UI.Timer {
    public partial class TimerControl : UserControl {
        // 服务层引用
        private readonly ITimerService? _timerService;
        private readonly IProfileService? _profileService;
        private readonly ITimerHistoryService? _historyService;

        // 组件引用 (这里去掉初始化，统一在 InitializeComponent 中处理)
        private StatisticsControl statisticsControl;
        private HistoryControl historyControl;
        private CharacterSceneControl characterSceneControl;
        private LootRecordsControl lootRecordsControl;
        private AntdUI.LabelTime labelTime1; // 如果这是第三方控件，请确保引用正确

        // 控件字段定义
        private Label btnStatusIndicator;
        private Button toggleLootButton;
        private Label lblTimeDisplay;

        public TimerControl() {
            InitializeComponent();

            // 设置圆形指示器
            // 建议放在构造函数末尾，确保控件尺寸已初始化
            if (btnStatusIndicator != null) {
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath()) {
                    path.AddEllipse(0, 0, btnStatusIndicator.Width, btnStatusIndicator.Height);
                    btnStatusIndicator.Region = new System.Drawing.Region(path);
                }
            }

            // 初始化时隐藏掉落记录控件
            if (lootRecordsControl != null) {
                lootRecordsControl.Visible = false;
                // 设置初始高度为不包含掉落记录的高度
                this.Size = new Size(this.Width, UISizeConstants.TimerControlHeightWithoutLoot);
            }
        }

        public TimerControl(IProfileService profileService, ITimerService timerService, ITimerHistoryService historyService) : this() {
            _timerService = timerService;
            _profileService = profileService;
            _historyService = historyService;

            // 初始化子控件的服务引用
            characterSceneControl?.Initialize(_profileService);
            historyControl?.Initialize(_historyService);

            // 订阅服务事件
            SubscribeToServiceEvents();

            // 注册语言变更事件
            LanguageManager.OnLanguageChanged += LanguageManager_OnLanguageChanged;
        }

        // 2. 【关键修复】重写 OnLoad 方法
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            // 在这里加载数据，此时控件已经创建完成
            if (!DesignMode && _profileService != null) {
                // 加载历史数据
                LoadProfileHistoryData();
                // 更新界面状态
                UpdateUI();
            }
        }

        // 事件
        public event EventHandler? TimerStateChanged;

        public bool IsTimerRunning => _timerService?.IsRunning ?? false;

        #region Service Event Handlers
        private void SubscribeToServiceEvents() {
            if (_timerService == null || _profileService == null) return;
            // 订阅TimerService事件
            _timerService.TimeUpdatedEvent += OnTimeUpdated;
            _timerService.TimerRunningStateChangedEvent += OnTimerRunningStateChanged;
            _timerService.TimerPauseStateChangedEvent += OnTimerPauseStateChanged;
            _timerService.TimerResetEvent += OnTimerReset;
            _timerService.RunCompletedEvent += OnRunCompleted;

            // 订阅ProfileService事件
            _profileService.CurrentProfileChangedEvent += OnProfileChanged;
            _profileService.CurrentSceneChangedEvent += OnSceneChanged;
            _profileService.CurrentDifficultyChangedEvent += OnDifficultyChanged;
        }

        private void UnsubscribeFromServiceEvents() {
            if (_timerService == null || _profileService == null) return;

            _timerService.TimeUpdatedEvent -= OnTimeUpdated;
            _timerService.TimerRunningStateChangedEvent -= OnTimerRunningStateChanged;
            _timerService.TimerPauseStateChangedEvent -= OnTimerPauseStateChanged;
            _timerService.TimerResetEvent -= OnTimerReset;
            _timerService.RunCompletedEvent -= OnRunCompleted;

            _profileService.CurrentProfileChangedEvent -= OnProfileChanged;
            _profileService.CurrentSceneChangedEvent -= OnSceneChanged;
            _profileService.CurrentDifficultyChangedEvent -= OnDifficultyChanged;
        }

        private void OnTimeUpdated(string timeString) {
            if (_timerService == null) return;

            if (lblTimeDisplay != null && lblTimeDisplay.InvokeRequired) {
                lblTimeDisplay.Invoke(new Action<string>(OnTimeUpdated), timeString);
            }
            else if (lblTimeDisplay != null) {
                lblTimeDisplay.Text = timeString;
            }
        }

        private void OnTimerRunningStateChanged(bool isRunning) {
            if (btnStatusIndicator != null && btnStatusIndicator.InvokeRequired) {
                btnStatusIndicator.Invoke(new Action<bool>(OnTimerRunningStateChanged), isRunning);
            }
            else if (btnStatusIndicator != null) {
                btnStatusIndicator.BackColor = isRunning ? Color.Green : Color.Red;
            }

            TimerStateChanged?.Invoke(this, EventArgs.Empty);
            UpdateStatistics();
        }

        private void OnTimerPauseStateChanged(bool isPaused) {
            TimerStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnProfileChanged(Models.CharacterProfile? profile) {
            LoadProfileHistoryData();
            UpdateCharacterSceneInfo();

            // 更新掉落记录
            if (lootRecordsControl != null && profile != null) {
                lootRecordsControl.UpdateLootRecords(profile.LootRecords);
            }
        }

        private void OnSceneChanged(string scene) {
            LoadProfileHistoryData();
            UpdateCharacterSceneInfo();
        }

        private void OnDifficultyChanged(Models.GameDifficulty difficulty) {
            LoadProfileHistoryData();
            UpdateCharacterSceneInfo();
        }

        // 【关键修复】确保加载后刷新列表
        private void LoadProfileHistoryData() {
            LogManager.WriteDebugLog("TimerControl", "LoadProfileHistoryData");
            if (historyControl != null && _profileService != null) {
                var profile = _profileService.CurrentProfile;
                var scene = _profileService.CurrentScene;
                var characterName = profile?.Name ?? "";
                var difficulty = _profileService.CurrentDifficulty;
                LogManager.WriteDebugLog("TimerControl", $"LoadProfileHistoryData: profile={profile?.Name}, scene={scene}, characterName={characterName}, difficulty={difficulty}");

                // 1. 尝试加载数据
                historyControl.LoadProfileHistoryData(profile, scene, characterName, difficulty);

                // 2. 更新掉落记录
                if (lootRecordsControl != null && profile != null) {
                    lootRecordsControl.UpdateLootRecords(profile.LootRecords);
                }
            }
        }

        private void OnTimerReset() {
            if (lblTimeDisplay != null && lblTimeDisplay.InvokeRequired) {
                lblTimeDisplay.Invoke(new Action(OnTimerReset));
            }
            else if (lblTimeDisplay != null) {
                lblTimeDisplay.Text = "00:00:00.0";
            }
            UpdateStatistics();
        }

        private void OnRunCompleted(TimeSpan runTime) {
            historyControl?.AddRunRecord(runTime);
            UpdateStatistics();
        }
        #endregion

        #region Public Methods
        public void ToggleTimer() {
            _timerService?.StartOrNextRun();
        }

        public void TogglePause() {
            _timerService?.TogglePause();
        }

        public void HandleExternalReset() {
            _timerService?.Reset();
        }

        public void HandleTabSelected() {
            LoadProfileHistoryData();
            UpdateUI();
        }

        public void HandleApplicationClosing() {
            _timerService?.HandleApplicationClosing();
        }

        public async Task<bool> DeleteSelectedRecordAsync() {
            if (historyControl != null) {
                return await historyControl.DeleteSelectedRecordAsync();
            }
            return false;
        }
        #endregion

        #region Private Methods
        private void LanguageManager_OnLanguageChanged(object? sender, EventArgs e) {
            UpdateUI();
        }

        private void UpdateUI() {
            if (btnStatusIndicator != null && _timerService != null) {
                btnStatusIndicator.BackColor = _timerService.IsRunning ? Color.Green : Color.Red;
            }

            if (_timerService != null && !_timerService.IsRunning && !_timerService.IsPaused) {
                if (lblTimeDisplay != null) {
                    lblTimeDisplay.Text = "00:00:00.0";
                }
            }

            UpdateStatistics();
            UpdateCharacterSceneInfo();
            UpdateLootRecords();
        }

        public void RefreshUI() {
            if (this.InvokeRequired) {
                this.Invoke(new Action(UpdateUI));
            }
            else {
                UpdateUI();
            }
        }

        private void UpdateStatistics() {
            if (statisticsControl != null && historyControl != null) {
                int runCount = historyControl.RunCount;
                TimeSpan fastestTime = historyControl.FastestTime;
                var runHistory = historyControl.RunHistory;
                statisticsControl.UpdateStatistics(runCount, fastestTime, runHistory);
            }
            historyControl?.UpdateHistory(historyControl.RunHistory);
        }

        private void UpdateCharacterSceneInfo() {
            characterSceneControl?.UpdateCharacterSceneInfo();
        }

        private void UpdateLootRecords() {
            if (lootRecordsControl != null && _profileService != null && _profileService.CurrentProfile != null) {
                lootRecordsControl.UpdateLootRecords(_profileService.CurrentProfile.LootRecords);
            }
        }
        #endregion

        #region UI Initialization
        private void InitializeComponent() {
            // 设置当前控件的宽度
            this.Width = UISizeConstants.TimerControlWidth;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimerControl));
            btnStatusIndicator = new Label();
            lblTimeDisplay = new Label();
            statisticsControl = new StatisticsControl();
            historyControl = new HistoryControl();
            characterSceneControl = new CharacterSceneControl();
            lootRecordsControl = new LootRecordsControl();
            labelTime1 = new AntdUI.LabelTime();
            toggleLootButton = new Button();
            SuspendLayout();
            // 
            // btnStatusIndicator
            // 
            btnStatusIndicator.Location = new Point(20, 19);
            btnStatusIndicator.Margin = new Padding(6);
            btnStatusIndicator.Name = "btnStatusIndicator";
            btnStatusIndicator.Size = new Size(24, 24);
            btnStatusIndicator.TabIndex = 0;
            btnStatusIndicator.Click += btnStatusIndicator_Click;
            // 
            // lblTimeDisplay
            // 
            lblTimeDisplay.AutoSize = true;
            lblTimeDisplay.Font = new Font("Microsoft YaHei UI", 28F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblTimeDisplay.Location = new Point(20, 61);
            lblTimeDisplay.Margin = new Padding(6, 0, 6, 0);
            lblTimeDisplay.Name = "lblTimeDisplay";
            lblTimeDisplay.Size = new Size(303, 86);
            lblTimeDisplay.TabIndex = 1;
            lblTimeDisplay.Text = "00:00:00";
            lblTimeDisplay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // statisticsControl
            // 
            statisticsControl.AverageTime = TimeSpan.Parse("00:00:00");
            statisticsControl.FastestTime = TimeSpan.Parse("00:00:00");
            statisticsControl.Location = new Point(9, 157);
            statisticsControl.Margin = new Padding(9, 8, 9, 8);
            statisticsControl.Name = "statisticsControl";
            statisticsControl.RunCount = 0;
            statisticsControl.Size = new Size(605, 116);
            statisticsControl.TabIndex = 2;
            // 
            // historyControl
            // 
            historyControl.Location = new Point(9, 289);
            historyControl.Margin = new Padding(9, 8, 9, 8);
            historyControl.Name = "historyControl";
            historyControl.Size = new Size(421, 117);
            historyControl.TabIndex = 3;
            // 
            // characterSceneControl
            // 
            characterSceneControl.Location = new Point(9, 409);
            characterSceneControl.Margin = new Padding(6);
            characterSceneControl.Name = "characterSceneControl";
            characterSceneControl.Size = new Size(213, 80);
            characterSceneControl.TabIndex = 4;
            // 
            // lootRecordsControl
            // 
            lootRecordsControl.Location = new Point(9, 495);
            lootRecordsControl.Margin = new Padding(9, 8, 9, 8);
            lootRecordsControl.Name = "lootRecordsControl";
            lootRecordsControl.Size = new Size(421, 219);
            lootRecordsControl.TabIndex = 6;
            // 
            // labelTime1
            // 
            labelTime1.Location = new Point(65, 9);
            labelTime1.Name = "labelTime1";
            labelTime1.ShowTime = false;
            labelTime1.Size = new Size(135, 40);
            labelTime1.TabIndex = 5;
            labelTime1.Text = "labelTime1";
            // 
            // toggleLootButton
            // 
            toggleLootButton.Location = new Point(299, 434);
            toggleLootButton.Name = "toggleLootButton";
            toggleLootButton.Size = new Size(131, 40);
            toggleLootButton.TabIndex = 7;
            toggleLootButton.Text = Utils.LanguageManager.GetString("ShowLoot", "显示掉落");
            toggleLootButton.UseVisualStyleBackColor = true;
            toggleLootButton.Click += toggleLootButton_Click;
            // 
            // TimerControl
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(toggleLootButton);
            Controls.Add(lootRecordsControl);
            Controls.Add(labelTime1);
            Controls.Add(characterSceneControl);
            Controls.Add(historyControl);
            Controls.Add(statisticsControl);
            Controls.Add(lblTimeDisplay);
            Controls.Add(btnStatusIndicator);
            Margin = new Padding(6);
            Name = "TimerControl";
            Size = new Size(667, 850);
            ResumeLayout(false);
            PerformLayout();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                UnsubscribeFromServiceEvents();
                LanguageManager.OnLanguageChanged -= LanguageManager_OnLanguageChanged;
            }
            base.Dispose(disposing);
        }
        #endregion

        private void btnStatusIndicator_Click(object sender, EventArgs e) {

        }

        private void toggleLootButton_Click(object sender, EventArgs e) {
            if (lootRecordsControl != null) {
                // 确保状态正确切换
                lootRecordsControl.Visible = !lootRecordsControl.Visible;
                bool isVisible = lootRecordsControl.Visible;

                // 更新按钮文本
                toggleLootButton.Text = isVisible ? Utils.LanguageManager.GetString("HideLoot", "隐藏掉落") : Utils.LanguageManager.GetString("ShowLoot", "显示掉落");

                // 计算高度差值
                int heightDifference = UISizeConstants.TimerControlHeightWithLoot - UISizeConstants.TimerControlHeightWithoutLoot;
                
                // 调整TimerControl的高度
                int newHeight = isVisible ? UISizeConstants.TimerControlHeightWithLoot : UISizeConstants.TimerControlHeightWithoutLoot;
                int heightChange = newHeight - this.Height;
                this.Size = new Size(this.Width, newHeight);

                // 调整父窗体（MainForm）的高度
                if (this.ParentForm != null) {
                    int newFormHeight = this.ParentForm.ClientSize.Height + heightChange;
                    this.ParentForm.ClientSize = new Size(this.ParentForm.ClientSize.Width, newFormHeight);
                }
            }
        }
    }
}