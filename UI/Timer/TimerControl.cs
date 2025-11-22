using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using DTwoMFTimerHelper.Services;
using DTwoMFTimerHelper.Utils;

namespace DTwoMFTimerHelper.UI.Timer {
    public partial class TimerControl : UserControl {
        // 服务层引用
        private readonly ITimerService? _timerService;
        private readonly IProfileService? _profileService;
        private AntdUI.LabelTime labelTime1;
        private readonly ITimerHistoryService? _historyService;
        public TimerControl() {
            InitializeComponent();
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            // 向路径中添加一个椭圆 (因为长宽相等，所以是正圆)
            path.AddEllipse(0, 0, btnStatusIndicator.Width, btnStatusIndicator.Height);
            // 设置控件的 Region 属性，这样只有圆内的部分可见
            btnStatusIndicator.Region = new System.Drawing.Region(path);
        }

        public TimerControl(IProfileService profileService, ITimerService timerService, ITimerHistoryService historyService) : this() {
            _timerService = timerService;
            _profileService = profileService;
            _historyService = historyService;

            characterSceneControl?.Initialize(_profileService);
            historyControl?.Initialize(_historyService);

            // 订阅服务事件
            SubscribeToServiceEvents();

            // 注册语言变更事件
            LanguageManager.OnLanguageChanged += LanguageManager_OnLanguageChanged;

            UpdateUI();
        }

        // 组件引用
        private StatisticsControl statisticsControl;
        private HistoryControl historyControl;
        private CharacterSceneControl characterSceneControl;

        // 控件字段定义
        private Label btnStatusIndicator;
        private Label lblTimeDisplay;

        // 事件
        public event EventHandler? TimerStateChanged;

        #region Properties
        public bool IsTimerRunning => _timerService.IsRunning;
        // public Models.CharacterProfile? CurrentProfile
        // {
        //     get => _profileService.CurrentProfile;
        //     set => _profileService.CurrentProfile = value; // 直接设置ProfileService中的属性
        // }
        #endregion

        #region Service Event Handlers
        private void SubscribeToServiceEvents() {
            if (_timerService == null || _profileService == null) return; // ✅ 新增检查
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
            if (_timerService == null || _profileService == null) return; // ✅ 新增检查
            // 取消订阅TimerService事件
            _timerService.TimeUpdatedEvent -= OnTimeUpdated;
            _timerService.TimerRunningStateChangedEvent -= OnTimerRunningStateChanged;
            _timerService.TimerPauseStateChangedEvent -= OnTimerPauseStateChanged;
            _timerService.TimerResetEvent -= OnTimerReset;
            _timerService.RunCompletedEvent -= OnRunCompleted;

            // 取消订阅ProfileService事件
            _profileService.CurrentProfileChangedEvent -= OnProfileChanged;
            _profileService.CurrentSceneChangedEvent -= OnSceneChanged;
            _profileService.CurrentDifficultyChangedEvent -= OnDifficultyChanged;
        }

        private void OnTimeUpdated(string timeString) {
            if (_timerService == null) return;

            // 线程安全检查
            if (lblTimeDisplay != null && lblTimeDisplay.InvokeRequired) {
                lblTimeDisplay.Invoke(new Action<string>(OnTimeUpdated), timeString);
            }
            else if (lblTimeDisplay != null) {
                // 仅仅更新文本，不再创建新字体对象，性能极佳
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
            // 可以在这里处理暂停状态的特殊UI显示
            TimerStateChanged?.Invoke(this, EventArgs.Empty);
        }

        // ProfileService事件处理程序
        private void OnProfileChanged(Models.CharacterProfile? profile) {
            LoadProfileHistoryData();
            UpdateCharacterSceneInfo();
        }

        private void OnSceneChanged(string scene) {
            LoadProfileHistoryData();
            UpdateCharacterSceneInfo();
        }

        private void OnDifficultyChanged(Models.GameDifficulty difficulty) {
            LoadProfileHistoryData();
            UpdateCharacterSceneInfo();
        }

        private void LoadProfileHistoryData() {
            if (historyControl != null) {
                var profile = _profileService.CurrentProfile;
                var scene = _profileService.CurrentScene;
                var characterName = profile?.Name ?? "";
                var difficulty = _profileService.CurrentDifficulty;

                historyControl.LoadProfileHistoryData(profile, scene, characterName, difficulty);
            }
        }
        private void OnTimerReset() {
            if (lblTimeDisplay != null && lblTimeDisplay.InvokeRequired) {
                lblTimeDisplay.Invoke(new Action(OnTimerReset));
            }
            else if (lblTimeDisplay != null) {
                // 只要重置文字即可，不用管字体了
                lblTimeDisplay.Text = "00:00:00.0";
            }

            UpdateStatistics();
        }

        private void OnRunCompleted(TimeSpan runTime) {
            // 使用HistoryControl来记录新的运行时间
            historyControl?.AddRunRecord(runTime);
            UpdateStatistics();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 通过快捷键触发开始/重新开始计时
        /// </summary>
        public void ToggleTimer() {
            LogManager.WriteDebugLog("TimerControl", $"ToggleTimer 调用（快捷键触发），当前状态: isTimerRunning={_timerService.IsRunning}");
            _timerService.StartOrNextRun();
        }

        /// <summary>
        /// 暂停/恢复计时
        /// </summary>
        public void TogglePause() {
            _timerService.TogglePause();
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        public void HandleExternalReset() {
            _timerService.Reset();
        }

        /// <summary>
        /// 当切换到计时器Tab时调用
        /// </summary>
        public void HandleTabSelected() {
            LoadProfileHistoryData();
            UpdateUI();
        }

        /// <summary>
        /// 在应用程序关闭时调用
        /// </summary>
        public void HandleApplicationClosing() {
            _timerService.HandleApplicationClosing();
        }

        /// <summary>
        /// 删除选中的历史记录
        /// </summary>
        /// <returns>是否删除成功</returns>
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
            // 更新状态指示按钮颜色
            if (btnStatusIndicator != null) {
                btnStatusIndicator.BackColor = _timerService.IsRunning ? Color.Green : Color.Red;
            }

            // 更新时间显示
            if (!_timerService.IsRunning && !_timerService.IsPaused) {
                // 只有在计时器完全停止（不是暂停）时才重置显示
                if (lblTimeDisplay != null) {
                    lblTimeDisplay.Font = new Font("Microsoft YaHei UI", 30F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblTimeDisplay.Text = "00:00:00:0";
                }
            }

            UpdateStatistics();
            UpdateCharacterSceneInfo();
        }

        /// <summary>
        /// 公共方法，供外部调用刷新UI
        /// </summary>
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
        #endregion

        #region UI Initialization
        private void InitializeComponent() {
            btnStatusIndicator = new Label();
            lblTimeDisplay = new Label();
            statisticsControl = new StatisticsControl();
            historyControl = new HistoryControl();
            characterSceneControl = new CharacterSceneControl();
            labelTime1 = new AntdUI.LabelTime();
            SuspendLayout();
            // 
            // btnStatusIndicator
            // 
            btnStatusIndicator.Location = new Point(20, 12);
            btnStatusIndicator.Margin = new Padding(6);
            btnStatusIndicator.Name = "btnStatusIndicator";
            btnStatusIndicator.Size = new Size(24, 24);
            btnStatusIndicator.TabIndex = 0;
            // 
            // lblTimeDisplay
            // 
            lblTimeDisplay.AutoSize = true;
            lblTimeDisplay.Font = new Font("Microsoft YaHei UI", 28F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblTimeDisplay.Location = new Point(30, 46);
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
            statisticsControl.Location = new Point(9, 140);
            statisticsControl.Margin = new Padding(9, 8, 9, 8);
            statisticsControl.Name = "statisticsControl";
            statisticsControl.RunCount = 0;
            statisticsControl.Size = new Size(605, 116);
            statisticsControl.TabIndex = 2;
            // 
            // historyControl
            // 
            historyControl.Location = new Point(9, 272);
            historyControl.Margin = new Padding(9, 8, 9, 8);
            historyControl.Name = "historyControl";
            historyControl.Size = new Size(421, 117);
            historyControl.TabIndex = 3;
            // 
            // characterSceneControl
            // 
            characterSceneControl.Location = new Point(9, 392);
            characterSceneControl.Margin = new Padding(6);
            characterSceneControl.Name = "characterSceneControl";
            characterSceneControl.Size = new Size(605, 80);
            characterSceneControl.TabIndex = 4;
            // 
            // labelTime1
            // 
            labelTime1.Location = new Point(53, 3);
            labelTime1.Name = "labelTime1";
            labelTime1.ShowTime = false;
            labelTime1.Size = new Size(135, 40);
            labelTime1.TabIndex = 5;
            labelTime1.Text = "labelTime1";
            // 
            // TimerControl
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(labelTime1);
            Controls.Add(characterSceneControl);
            Controls.Add(historyControl);
            Controls.Add(statisticsControl);
            Controls.Add(lblTimeDisplay);
            Controls.Add(btnStatusIndicator);
            Margin = new Padding(6);
            Name = "TimerControl";
            Size = new Size(667, 627);
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
    }
}