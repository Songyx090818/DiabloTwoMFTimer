using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using DTwoMFTimerHelper.Utils;
using DTwoMFTimerHelper.Services;
using DTwoMFTimerHelper.Models;
using DTwoMFTimerHelper.UI.Profiles;
using Timer = System.Windows.Forms.Timer;

namespace DTwoMFTimerHelper.UI.Timer
{
    public partial class TimerControl : UserControl
    {
        // 计时器相关字段
        private bool isTimerRunning = false;
        private DateTime startTime = DateTime.MinValue;
        private System.Windows.Forms.Timer? timer;
        private Models.CharacterProfile? currentProfile = null;
        private readonly Models.MFRecord? inProgressRecord = null;
        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer
            {
                Interval = 100 // 100毫秒
            };
            timer.Tick += Timer_Tick;
        }

        private string currentCharacter = "";
        private string currentScene = "";
        
        // 组件引用
        private StatisticsControl? statisticsControl;
        private HistoryControl? historyControl;
        private CharacterSceneControl? characterSceneControl;

        // 公共属性
        public bool IsTimerRunning => isTimerRunning;
        public Models.CharacterProfile? CurrentProfile => currentProfile;
        public Models.MFRecord? InProgressRecord => inProgressRecord;
        
        // 控件字段定义
        private Button? btnStatusIndicator;
        private Label? lblTimeDisplay;
        
        // 计时器状态字段
        private bool isPaused = false;
        private TimeSpan pausedDuration = TimeSpan.Zero;
        private DateTime pauseStartTime = DateTime.MinValue;
        
        // 事件
        public event EventHandler? TimerStateChanged;


        public TimerControl()
        {
            
            InitializeComponent();
            // 注册语言变更事件
            LanguageManager.OnLanguageChanged += LanguageManager_OnLanguageChanged;
            
            InitializeTimer();
            UpdateUI();
        }
        
        private void LanguageManager_OnLanguageChanged(object? sender, EventArgs e)
        {
            UpdateUI();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 取消注册语言变更事件
                LanguageManager.OnLanguageChanged -= LanguageManager_OnLanguageChanged;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // 状态指示按钮
            btnStatusIndicator = new Button
            {
                Enabled = false,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(16, 16),
                Location = new Point(15, 45),
                Name = "btnStatusIndicator",
                TabIndex = 0,
                TabStop = false,
                BackColor = Color.Red,
                FlatAppearance = { BorderSize = 0 }
            };

            // 主要计时显示标签
            lblTimeDisplay = new Label
            {
                AutoSize = false,
                Font = new Font("Microsoft YaHei UI", 30F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                Location = new Point(15, 30),
                Name = "lblTimeDisplay",
                Size = new Size(290, 64),
                TextAlign = ContentAlignment.MiddleCenter,
                TabIndex = 1,
                Text = "00:00:00:0"
            };
            
            // 初始化统计信息控件
            statisticsControl = new StatisticsControl
            {
                Location = new Point(5, 100),
                Name = "statisticsControl",
                Size = new Size(290, 75),
                TabIndex = 5,
                Parent = this
            };

            // 初始化历史记录控件
            historyControl = new HistoryControl
            {
                Location = new Point(15, 170),
                Name = "historyControl",
                Size = new Size(290, 90),
                TabIndex = 3,
                Parent = this
            };
            
            // 初始化角色场景信息组件
            characterSceneControl = new CharacterSceneControl
            {
                Location = new Point(15, 270),
                Name = "characterSceneControl",
                Size = new Size(290, 40),
                TabIndex = 4
            };
            
            SuspendLayout();
            
            // 
            // TimerControl - 主控件设置
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnStatusIndicator);
            Controls.Add(lblTimeDisplay);
            Controls.Add(statisticsControl);
            Controls.Add(historyControl);
            Controls.Add(characterSceneControl);
            Name = "TimerControl";
            Size = new Size(320, 320);
            ResumeLayout(false);
            PerformLayout();
        }
        
        // 使用LogManager进行日志记录
        
        /// <summary>
        /// 设置角色和场景信息
        /// </summary>
        /// <param name="character">角色名称</param>
        /// <param name="scene">场景名称</param>
        public void SetCharacterAndScene(string character, string scene)
        {
            // 将调用委托给CharacterSceneControl，由它负责更新组件和保存设置
            characterSceneControl?.SetCharacterAndScene(character, scene);
            
            UpdateUI();
        }
        
        public void UpdateUI()
        {
            // 更新状态指示按钮颜色
            if (btnStatusIndicator != null)
            {
                btnStatusIndicator.BackColor = isTimerRunning ? Color.Green : Color.Red;
            }
            
            // 更新时间显示
            if (isTimerRunning && startTime != DateTime.MinValue)
            {
                TimeSpan elapsed;

                // 优先使用inProgressRecord中的elapsedTime值（来自yaml文件）
                if (inProgressRecord != null && inProgressRecord.ElapsedTime.HasValue && inProgressRecord.ElapsedTime.Value > 0)
                {
                    LogManager.WriteDebugLog("TimerControl", $"使用inProgressRecord中的elapsedTime值: {inProgressRecord.ElapsedTime}秒");
                    elapsed = TimeSpan.FromSeconds(inProgressRecord.ElapsedTime.Value);
                    
                    // 如果不是暂停状态，需要加上从LatestTime到现在的时间
                    if (!isPaused && inProgressRecord.LatestTime.HasValue)
                    {
                        double timeSinceLatest = (DateTime.Now - inProgressRecord.LatestTime.Value).TotalSeconds;
                        elapsed = elapsed.Add(TimeSpan.FromSeconds(timeSinceLatest));
                    }
                }
                else if (isPaused && pauseStartTime != DateTime.MinValue)
                {
                    // 暂停状态，计算到暂停开始时的时间
                    elapsed = pauseStartTime - startTime - pausedDuration;
                }
                else
                {
                    // 运行状态，计算实际经过时间（扣除暂停时间）
                    elapsed = DateTime.Now - startTime - pausedDuration;
                }
                
                // 显示100毫秒格式
                string formattedTime = string.Format("{0:00}:{1:00}:{2:00}:{3}", 
                    elapsed.Hours, elapsed.Minutes, elapsed.Seconds, 
                    (int)(elapsed.Milliseconds / 100));
                    
                if (lblTimeDisplay != null) 
                {
                    // 根据时间长度调整字体大小确保显示完整
                    if (elapsed.Hours > 9)
                    {
                        // 小时数超过9时使用更小的字体
                        lblTimeDisplay.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    }
                    else if (elapsed.Hours > 0)
                    {
                        // 有小时数但不超过9时使用中等字体
                        lblTimeDisplay.Font = new Font("Microsoft YaHei UI", 28F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    }
                    else
                    {
                        // 没有小时数时使用合适的字体大小
                        lblTimeDisplay.Font = new Font("Microsoft YaHei UI", 30F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    }
                    
                    // 暂停时显示不同的样式
                    if (isPaused)
                    {
                        lblTimeDisplay.Text = formattedTime;
                    }
                    else
                    {
                        lblTimeDisplay.Text = formattedTime;
                    }
                }
            }
            else
            {
                if (lblTimeDisplay != null) 
                {
                    lblTimeDisplay.Font = new Font("Microsoft YaHei UI", 30F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblTimeDisplay.Text = "00:00:00:0";
                }
            }
            
            // 更新统计信息组件
            if (statisticsControl != null && historyControl != null)
            {
                // 从HistoryControl获取统计数据
                int runCount = historyControl.RunCount;
                TimeSpan fastestTime = historyControl.FastestTime;
                List<TimeSpan> runHistory = historyControl.RunHistory;
                
                statisticsControl.UpdateStatistics(runCount, fastestTime, runHistory);
            }
            
            // 更新历史记录组件
            historyControl?.UpdateHistory(historyControl.RunHistory);
            
            // 更新角色场景信息组件
            if (characterSceneControl != null)
            {
                if (currentProfile != null)
                {
                    // 获取角色职业信息
                    // 使用统一方法获取本地化职业名称
                    _ = Utils.LanguageManager.GetLocalizedClassName(currentProfile.Class);
                }
                
                // 获取游戏难度
                string difficultyText = GetCurrentDifficultyText();
                
                // 获取本地化的场景名称
                string localizedSceneName = Utils.LanguageManager.GetString(currentScene);
                
                characterSceneControl.UpdateCharacterSceneInfo(currentCharacter, currentProfile, localizedSceneName, difficultyText);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// 通过快捷键触发开始/停止计时
        /// 当停止时：保存记录到角色档案，并立即开始下一场计时
        /// </summary>
        public void ToggleTimer()
        {   
            LogManager.WriteDebugLog("TimerControl", $"ToggleTimer 调用（快捷键触发），当前状态: isTimerRunning={isTimerRunning}");
            if (!isTimerRunning)
            {   
                LogManager.WriteDebugLog("TimerControl", $"通过快捷键开始计时，当前角色档案: {(currentProfile != null ? currentProfile.Name : "null")}");
                StartTimer();
            }
            else
            {   
                // 停止当前计时并保存记录
                LogManager.WriteDebugLog("TimerControl", $"通过快捷键停止计时");
                StopTimer(true); // 传入true表示通过快捷键触发，需要保存并自动开始下一场
            }
        }
        
        // 提供给外部调用的暂停方法，用于快捷键触发
        public void TogglePause()
        {
            if (isTimerRunning)
            {
                if (isPaused)
                {
                    ResumeTimer();
                }
                else
                {
                    PauseTimer();
                }
            }
        }
        
        private void PauseTimer()
        {
            if (isTimerRunning && !isPaused)
            {
                isPaused = true;
                pauseStartTime = DateTime.Now;
                UpdateUI();
                
                // 更新未完成记录的LatestTime和ElapsedTime
                UpdateIncompleteRecord();
                
                // 保存暂停状态到设置
                SaveTimerState();
            }
        }
        
        private void ResumeTimer()
        {            
            if (isTimerRunning && isPaused && pauseStartTime != DateTime.MinValue)
            {
                // 在计算暂停时间之前，先更新记录，使用pauseStartTime作为更新时间点
                UpdateIncompleteRecord(pauseStartTime);
                
                pausedDuration += DateTime.Now - pauseStartTime;
                isPaused = false;
                pauseStartTime = DateTime.MinValue;
                UpdateUI();
                
                // 只更新latestTime，不影响elapsedTime计算
                if (currentProfile != null)
                {
                    var record = FindIncompleteRecordForCurrentScene();
                    if (record != null)
                    {
                        // 只更新LatestTime，保持ElapsedTime不变
                        record.LatestTime = DateTime.Now;
                        Services.DataManager.UpdateMFRecord(currentProfile, record);
                        LogManager.WriteDebugLog("TimerControl", $"已更新未完成记录的LatestTime: 场景={currentScene}, 更新时间点={DateTime.Now}");
                    }
                }
                
                // 保存恢复状态到设置
                SaveTimerState();
            }
        }
        
        // 提供给外部调用的重置方法
        public void ResetTimerExternally()
        {
            ResetTimer();
        }

        /// <summary>
        /// 开始计时器
        /// </summary>
        public void StartTimer()
        {
            if (isTimerRunning)
                return;
            
            // 如果没有设置角色和场景，尝试从当前打开的ProfileManager获取
            if (string.IsNullOrEmpty(currentCharacter) || string.IsNullOrEmpty(currentScene))
            {
                LogManager.WriteDebugLog("TimerControl", "角色或场景为空，尝试从主窗口获取档案信息");
                TryGetProfileInfoFromMainForm();
            }
            
            LogManager.WriteDebugLog("TimerControl", $"[快捷键触发] 开始计时前的角色档案信息: currentCharacter={currentCharacter}, currentProfile={(currentProfile != null ? currentProfile.Name : "null")}");
            LogManager.WriteDebugLog("TimerControl", $"[快捷键触发] 当前场景: {currentScene}, 当前时间: {DateTime.Now}");
            
            isTimerRunning = true;
            isPaused = false;
            startTime = DateTime.Now;
            pausedDuration = TimeSpan.Zero;
            pauseStartTime = DateTime.MinValue;
            timer?.Start();
            
            // 在开始计时时创建一条记录
            CreateStartRecord();
            
            // 保存当前角色、场景和难度到设置
            if (!string.IsNullOrEmpty(currentCharacter) && !string.IsNullOrEmpty(currentScene))
            {
                try
                {
                    var settings = Services.SettingsManager.LoadSettings();
                    // 提取纯角色名称，去除可能包含的职业信息 (如 "AAA (刺客)" -> "AAA")
                    string pureCharacterName = currentCharacter;
                    if (currentCharacter.Contains(" ("))
                    {
                        int index = currentCharacter.IndexOf(" (");
                        pureCharacterName = currentCharacter.Substring(0, index);
                        LogManager.WriteDebugLog("TimerControl", $"已从角色名称中提取纯名称: 原名称='{currentCharacter}', 提取后='{pureCharacterName}'");
                    }
                    settings.LastUsedProfile = pureCharacterName;
                    settings.LastUsedScene = currentScene;
                    settings.LastUsedDifficulty = GetCurrentDifficulty().ToString();
                    
                    // 保存计时状态
                    SaveTimerStateToSettings(settings);
                    
                    Services.SettingsManager.SaveSettings(settings);
                    Console.WriteLine($"已保存设置: 角色={pureCharacterName}, 场景={currentScene}, 难度={settings.LastUsedDifficulty}");
                    LogManager.WriteDebugLog("TimerControl", $"已保存设置到配置文件: LastUsedProfile={pureCharacterName}, LastUsedScene={currentScene}, LastUsedDifficulty={settings.LastUsedDifficulty}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"保存设置失败: {ex.Message}");
                }
            }
            
            UpdateUI();
            TimerStateChanged?.Invoke(this, EventArgs.Empty);
        }
        
        /// <summary>
        /// 尝试从主窗口获取角色和场景信息
        /// </summary>
        private void TryGetProfileInfoFromMainForm()
        {   
                // 获取主窗口
                var mainForm = this.FindForm() as MainForm;
                if (mainForm != null && mainForm.ProfileManager != null)
                {   
                    var profileManager = mainForm.ProfileManager;
                    if (profileManager.CurrentProfile != null)
                    {   
                        currentProfile = profileManager.CurrentProfile;
                        currentCharacter = profileManager.CurrentProfile.Name;
                        currentScene = profileManager.CurrentScene;
                    }
                    else
                    {
                        LogManager.WriteDebugLog("TimerControl", "ProfileManager.CurrentProfile 为 null，无法获取角色信息");
                    }
                }
                else
                {
                    LogManager.WriteDebugLog("TimerControl", $"mainForm 或 ProfileManager 为 null: mainForm={(mainForm != null ? "非null" : "null")}, ProfileManager={(mainForm != null && mainForm.ProfileManager != null ? "非null" : "null")}");
                }
        }
        
        /// <summary>
        /// 同步更新角色和场景信息
        /// 当ProfileManager中的角色或场景改变时调用此方法
        /// </summary>
        public void SyncWithProfileManager()
        {   
            TryGetProfileInfoFromMainForm();
            // 调用HistoryControl中的方法加载历史数据
            historyControl?.LoadProfileHistoryData(currentProfile, currentScene, currentCharacter, GetCurrentDifficulty());
            UpdateUI();
            LogManager.WriteDebugLog("TimerControl", $"已同步角色和场景信息: {currentCharacter} - {currentScene}");
        }
        
        /// <summary>
        /// 当切换到计时器Tab时调用此方法
        /// 自动加载角色档案中对应的场景数据并显示
        /// </summary>
        public void OnTabSelected()
        {   
            SyncWithProfileManager();
            LogManager.WriteDebugLog("TimerControl", "计时器Tab被选中，已自动加载角色档案数据");
        }
        
        /// <summary>
        /// 将当前计时记录保存到角色档案
        /// </summary>
        private void SaveToProfile()
        {   
            // 如果没有设置当前角色档案，尝试从主窗口获取
            if (currentProfile == null)
            {
                TryGetProfileInfoFromMainForm();
                // 如果仍然获取不到，记录日志并返回
                if (currentProfile == null)
                {
                    LogManager.WriteDebugLog("TimerControl", "无法保存记录：未找到当前角色档案");
                    return;
                }
            }
            
            if (string.IsNullOrEmpty(currentCharacter) || string.IsNullOrEmpty(currentScene) || startTime == DateTime.MinValue)
                return;

            try
            {
                // 从场景名称中提取ACT值
                int actValue = DTwoMFTimerHelper.Services.DataManager.GetSceneActValue(currentScene);
                
                // 获取难度信息
                var difficulty = GetCurrentDifficulty();
                
                // 根据当前语言设置正确的场景中英文名称
                string sceneEnName = currentScene; // 默认值
                string sceneZhName = currentScene; // 默认值
                
                // 从设置中获取当前语言
                bool isChineseScene = Services.SettingsManager.StringToLanguage(Services.SettingsManager.LoadSettings().Language) == DTwoMFTimerHelper.UI.Settings.SettingsControl.LanguageOption.Chinese;
                
                // 如果是中文场景，需要区分中英文
                if (isChineseScene || currentScene.StartsWith("ACT ") || currentScene.StartsWith("Act ") || currentScene.StartsWith("act "))
                {
                    // 当前显示的是中文，所以SceneZhName就是currentScene
                    sceneZhName = currentScene;
                    
                    // 尝试获取对应的英文名称
                    sceneEnName = DTwoMFTimerHelper.Services.DataManager.GetEnglishSceneName(currentScene);
                    LogManager.WriteDebugLog("TimerControl", $"场景中英文映射: 中文='{sceneZhName}', 英文='{sceneEnName}'");
                }
                else
                {
                    // 当前显示的是英文，所以SceneEnName就是currentScene
                    sceneEnName = currentScene;
                    sceneZhName = currentScene; // 如果无法获取中文，保持一致
                }
                
                // 计算实际持续时间
                TimeSpan actualDuration = DateTime.Now - startTime - pausedDuration;
                double durationSeconds = actualDuration.TotalSeconds;
                
                // 确保场景名称不为空
                if (string.IsNullOrEmpty(sceneEnName))
                {
                    sceneEnName = "UnknownScene"; // 设置默认值
                    LogManager.WriteDebugLog("TimerControl", $"警告: SaveToProfile中sceneEnName为空，使用默认值 '{sceneEnName}'");
                }
                
                // 创建新的MF记录，确保设置正确的LatestTime和ElapsedTime
                var newRecord = new Models.MFRecord
                {
                    SceneName = sceneEnName, // 使用英文名称作为SceneName
                    ACT = actValue,
                    Difficulty = difficulty,
                    StartTime = startTime,
                    EndTime = DateTime.Now,
                    LatestTime = DateTime.Now, // 设置LatestTime为结束时间
                    ElapsedTime = durationSeconds // 设置ElapsedTime为实际计算的持续时间
                    // IsCompleted是只读属性，通过设置EndTime来自动计算
                };

                // 获取英文场景名称并移除ACT前缀，与CreateStartRecord方法保持一致的格式
                string sceneEnNameForSearch = DTwoMFTimerHelper.Services.DataManager.GetEnglishSceneName(currentScene);
                string pureEnglishSceneNameForSearch = DTwoMFTimerHelper.Utils.LanguageManager.GetPureEnglishSceneName(currentScene);
                
                LogManager.WriteDebugLog("TimerControl", $"查找未完成记录: 原始场景名='{currentScene}', 纯英文场景名='{pureEnglishSceneNameForSearch}'");
                
                // 查找同场景同难度的未完成记录（使用与CreateStartRecord一致的纯英文场景名称格式）
                var existingRecord = currentProfile.Records.FirstOrDefault(r => 
                    r.SceneName == pureEnglishSceneNameForSearch && 
                    r.Difficulty == difficulty && 
                    !r.IsCompleted);
                
                if (existingRecord != null)
                {
                    // 计算实际持续时间（使用不同的变量名避免作用域冲突）
                    TimeSpan existingRecordDuration = DateTime.Now - existingRecord.StartTime - pausedDuration;
                    double existingRecordSeconds = existingRecordDuration.TotalSeconds;
                    
                    // 更新现有记录，确保设置正确的值
                    existingRecord.EndTime = DateTime.Now;
                    existingRecord.LatestTime = DateTime.Now; // 设置LatestTime为结束时间
                    existingRecord.ElapsedTime = existingRecordSeconds; // 设置ElapsedTime为实际计算的持续时间
                    // IsCompleted是只读属性，通过设置EndTime来自动计算
                    existingRecord.ACT = actValue;
                    existingRecord.Difficulty = difficulty;
                    
                    // 更新现有记录
                    DTwoMFTimerHelper.Services.DataManager.UpdateMFRecord(currentProfile, existingRecord);
                    LogManager.WriteDebugLog("TimerControl", $"[更新现有记录] {currentCharacter} - {currentScene}, ACT: {actValue}, 难度: {difficulty}, 开始时间: {existingRecord.StartTime}, 结束时间: {DateTime.Now}, ElapsedTime: {existingRecord.ElapsedTime}, 计算时间: {existingRecord.DurationSeconds}秒");
                }
                else
                {
                    // 添加新记录
                    Services.DataManager.AddMFRecord(currentProfile, newRecord);
                    LogManager.WriteDebugLog("TimerControl", $"[添加新记录] {currentCharacter} - {currentScene}, ACT: {actValue}, 难度: {difficulty}, 开始时间: {startTime}, 结束时间: {DateTime.Now}, ElapsedTime: {newRecord.ElapsedTime}, 计算时间: {newRecord.DurationSeconds}秒");
                }

                // 记录日志
                Console.WriteLine($"已保存计时记录到角色档案: {currentCharacter} - {currentScene}, ACT: {actValue}, ElapsedTime: {newRecord.ElapsedTime}, 计算时间: {newRecord.DurationSeconds}秒");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存计时记录失败: {ex.Message}");
                Console.WriteLine($"异常堆栈: {ex.StackTrace}");
            }
        }
        
        // 从场景名称中提取ACT值的功能已移至DataManager.GetSceneActValue方法
        // 请使用DTwoMFTimerHelper.Services.DataManager.GetSceneActValue()替代
        
        /// <summary>
        /// 获取当前游戏难度
        /// </summary>
        private Models.GameDifficulty GetCurrentDifficulty()
        {
            try
            {
                // 尝试从主窗口的ProfileManager获取难度信息
                var mainForm = this.FindForm() as MainForm;
                if (mainForm != null && mainForm.ProfileManager != null)
                {
                    // 这里简化处理，实际应该调用ProfileManager的相关属性
                    // 暂时默认返回地狱难度
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取难度信息失败: {ex.Message}");
            }
            return Models.GameDifficulty.Hell; // 默认地狱难度
        }
        
        // GetSceneEnglishName方法已被移除，现在使用DTwoMFTimerHelper.Data.DataManager.GetEnglishSceneName和DTwoMFTimerHelper.Resources.LanguageManager.GetPureEnglishSceneName
        
        /// <summary>
        /// 获取当前游戏难度的中文显示文本
        /// </summary>
        /// <returns>难度的中文文本</returns>
        private string GetCurrentDifficultyText()
        {
            Models.GameDifficulty difficulty = GetCurrentDifficulty();
            
            switch (difficulty)
            {
                case Models.GameDifficulty.Normal:
                    return DTwoMFTimerHelper.Utils.LanguageManager.GetString("DifficultyNormal");
                case Models.GameDifficulty.Nightmare:
                    return DTwoMFTimerHelper.Utils.LanguageManager.GetString("DifficultyNightmare");
                case Models.GameDifficulty.Hell:
                    return DTwoMFTimerHelper.Utils.LanguageManager.GetString("DifficultyHell");
                default:
                    return DTwoMFTimerHelper.Utils.LanguageManager.GetString("DifficultyUnknown");
            }
        }

        private void StopTimer(bool autoStartNext = false)
        {            
            isTimerRunning = false;
            isPaused = false;
            timer?.Stop();

            // 清除计时状态设置
            ClearTimerState();
            
            // 记录本次运行时间
            if (startTime != DateTime.MinValue)
            {
                TimeSpan runTime = DateTime.Now - startTime - pausedDuration;
                
                // 使用HistoryControl来记录新的运行时间
                if (historyControl != null)
                {
                    historyControl.AddRunRecord(runTime);
                }
                
                // 保存记录到角色档案
                // 首先尝试获取当前角色档案（如果为null）
                if (currentProfile == null)
                {
                    LogManager.WriteDebugLog("TimerControl", "StopTimer: currentProfile为null，尝试获取角色档案");
                    TryGetProfileInfoFromMainForm();
                }
                
                // 调用SaveToProfile方法保存记录（SaveToProfile内部也会检查并尝试获取角色档案）
                SaveToProfile();
            }
            
            UpdateUI();
            TimerStateChanged?.Invoke(this, EventArgs.Empty);

            // 如果是通过快捷键触发，自动开始下一场计时
            if (autoStartNext)
            {
                // 短暂延迟后自动开始下一场
                System.Threading.Tasks.Task.Delay(100).ContinueWith(_ =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((Action)StartTimer);
                    }
                    else
                    {
                        StartTimer();
                    }
                });
            }
        }

        private void ResetTimer()
        {
            StopTimer();
            ResetTimerDisplay();
        }

        private void ResetTimerDisplay()
        {            
            startTime = DateTime.MinValue;
            pausedDuration = TimeSpan.Zero;
            pauseStartTime = DateTime.MinValue;

            // 清除计时状态设置
            ClearTimerState();
            
            UpdateUI();
        }

        public void SetCurrentProfile(Models.CharacterProfile? profile)
        {
            currentProfile = profile;
            UpdateUI();
        }

        /// <summary>
        /// 在开始计时时创建一条记录
        /// 即使没有完整的角色和场景信息，也会创建一条基本记录
        /// </summary>
        private void CreateStartRecord()
        {
            // 如果没有角色档案，尝试从主窗口获取
            if (currentProfile == null)
            {
                LogManager.WriteDebugLog("TimerControl", "CreateStartRecord: currentProfile为null，尝试获取角色档案");
                TryGetProfileInfoFromMainForm();
                
                // 如果仍然没有档案，创建一个临时的基本记录
                if (currentProfile == null)
                {
                    LogManager.WriteDebugLog("TimerControl", "仍然没有角色档案，创建临时记录");
                }
            }

            // 确保currentCharacter和currentScene有默认值
            if (string.IsNullOrEmpty(currentCharacter))
                currentCharacter = "Unknown Character";
            if (string.IsNullOrEmpty(currentScene))
                currentScene = "Unknown Scene";

            try
            {
                // 从场景名称中提取ACT值
                int actValue = DTwoMFTimerHelper.Services.DataManager.GetSceneActValue(currentScene);
                
                // 获取难度信息
                var difficulty = GetCurrentDifficulty();
                
                // 获取英文场景名称和纯英文场景名称（移除ACT前缀）
                string sceneEnName = Services.DataManager.GetEnglishSceneName(currentScene);
                string pureEnglishSceneName = DTwoMFTimerHelper.Utils.LanguageManager.GetPureEnglishSceneName(currentScene);
                
                LogManager.WriteDebugLog("TimerControl", $"保存场景名称: 原始='{currentScene}', 英文='{sceneEnName}', 纯英文='{pureEnglishSceneName}'");
                
                // 确保场景名称不为空
                if (string.IsNullOrEmpty(pureEnglishSceneName))
                {
                    pureEnglishSceneName = "UnknownScene"; // 设置默认值
                    LogManager.WriteDebugLog("TimerControl", $"警告: CreateStartRecord中pureEnglishSceneName为空，使用默认值 '{pureEnglishSceneName}'");
                }
                
                // 创建新的MF记录（未完成）
                var newRecord = new DTwoMFTimerHelper.Models.MFRecord
                {
                    SceneName = pureEnglishSceneName, // 使用不带ACT前缀的英文名称作为SceneName
                    ACT = actValue,
                    Difficulty = difficulty,
                    StartTime = startTime,
                    EndTime = null, // 未完成，结束时间设为null
                    LatestTime = startTime, // 初始化LatestTime为开始时间，而不是null
                    ElapsedTime = 0.0 // 开始时已用时间为0
                };

                // 只有当有角色档案时才添加记录
                if (currentProfile != null)
                {
                    DTwoMFTimerHelper.Services.DataManager.AddMFRecord(currentProfile, newRecord);
                    
                    // 记录日志
                    Console.WriteLine($"已创建开始记录到角色档案: {currentCharacter} - {currentScene}, ACT: {actValue}");
                    LogManager.WriteDebugLog("TimerControl", $"已创建开始记录到角色档案: {currentCharacter} - {currentScene}, ACT: {actValue}, 开始时间: {startTime}");
                }
                else
                {
                    // 记录日志但不添加到档案
                    LogManager.WriteDebugLog("TimerControl", $"已创建临时记录但未保存到档案: {currentCharacter} - {currentScene}, ACT: {actValue}, 开始时间: {startTime}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建开始记录失败: {ex.Message}");
                LogManager.WriteDebugLog("TimerControl", $"创建开始记录失败: {ex.Message}, 堆栈: {ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// 查找用户同场景同难度的最近一条未完成记录
        /// </summary>
        private Models.MFRecord? FindIncompleteRecordForCurrentScene()
        {
            if (currentProfile == null || string.IsNullOrEmpty(currentScene))
                return null;
                
            var difficulty = GetCurrentDifficulty();
            
            // 使用与创建记录时相同的场景名称处理逻辑
            string pureEnglishSceneName = DTwoMFTimerHelper.Utils.LanguageManager.GetPureEnglishSceneName(currentScene);
            
            // 查找同场景、同难度、未完成的最近一条记录
            return currentProfile.Records
                .Where(r => r.SceneName == pureEnglishSceneName && r.Difficulty == difficulty && !r.IsCompleted)
                .OrderByDescending(r => r.StartTime)
                .FirstOrDefault();
        }
        
        /// <summary>
        /// 更新未完成记录的LatestTime和ElapsedTime
        /// </summary>
        /// <param name="updateTime">用于更新的时间点，默认为当前时间</param>
        private void UpdateIncompleteRecord(DateTime? updateTime = null)
        {
            if (!isTimerRunning || currentProfile == null)
                return;
                
            var record = FindIncompleteRecordForCurrentScene();
            if (record == null)
                return;
                
            try
            {
                // 保存更新前的累计时间和上次更新时间
                double previousElapsedTime = record.ElapsedTime ?? 0;
                DateTime? previousLatestTime = record.LatestTime;
                
                // 使用提供的时间点或当前时间
                DateTime now = updateTime ?? DateTime.Now;
                
                // 计算新的ElapsedTime，确保只增不减
                double newElapsedTime;
                
                // 确保LatestTime始终有值
                if (!record.LatestTime.HasValue)
                {
                    // 如果LatestTime为空，初始化它为StartTime
                    record.LatestTime = record.StartTime;
                    previousLatestTime = record.StartTime;
                    LogManager.WriteDebugLog("TimerControl", $"[更新记录] 初始化LatestTime为StartTime: {record.StartTime}");
                }
                
                // 如果之前有LatestTime，计算新的ElapsedTime
                if (previousLatestTime.HasValue)
                {
                    // 确保updateTime不早于previousLatestTime
                    DateTime effectiveUpdateTime = now > previousLatestTime.Value ? now : previousLatestTime.Value;
                    // ElapsedTime = (更新时间 - 上次LatestTime) + 已有的ElapsedTime
                    double additionalSeconds = (effectiveUpdateTime - previousLatestTime.Value).TotalSeconds;
                    newElapsedTime = previousElapsedTime + additionalSeconds;
                    LogManager.WriteDebugLog("TimerControl", $"[更新记录] 基于LatestTime计算: 上次时间={previousLatestTime.Value}, 当前时间={effectiveUpdateTime}, 增加时间={additionalSeconds}, 总计={newElapsedTime}");
                }
                else
                {
                    // 第一次更新，从StartTime开始计算
                    newElapsedTime = (now - record.StartTime).TotalSeconds;
                    LogManager.WriteDebugLog("TimerControl", $"[更新记录] 基于StartTime计算: 开始时间={record.StartTime}, 当前时间={now}, 总计={newElapsedTime}");
                }
                
                // 确保累计时间不会减少并且始终大于0
                if (newElapsedTime > previousElapsedTime || previousElapsedTime == 0)
                {
                    record.ElapsedTime = newElapsedTime;
                    LogManager.WriteDebugLog("TimerControl", $"[更新记录] ElapsedTime已更新: {previousElapsedTime} -> {newElapsedTime}");
                }
                else
                {
                    // 如果计算出的时间小于等于之前的时间，保持原值
                    newElapsedTime = previousElapsedTime;
                    LogManager.WriteDebugLog("TimerControl", $"[更新记录] 保持原有ElapsedTime: {previousElapsedTime}");
                }
                
                // 更新LatestTime
                record.LatestTime = now;
                LogManager.WriteDebugLog("TimerControl", $"[更新记录] LatestTime已更新: {(previousLatestTime.HasValue ? previousLatestTime.Value.ToString() : "null")} -> {now}");
                
                // 更新记录
                DTwoMFTimerHelper.Services.DataManager.UpdateMFRecord(currentProfile, record);
                
                // 记录更详细的日志信息，包含上次累计时间和上次更新时间
                LogManager.WriteDebugLog("TimerControl", $"已更新未完成记录: 场景={currentScene}, 上次累计时间={previousElapsedTime}秒, 当前累计时间={newElapsedTime}秒, 上次更新时间={previousLatestTime}, 更新时间点={now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新未完成记录失败: {ex.Message}");
                LogManager.WriteDebugLog("TimerControl", $"更新未完成记录失败: {ex.Message}, 堆栈: {ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// 保存计时状态到设置
        /// </summary>
        private void SaveTimerState()
        {            
            try
            {
                var settings = Services.SettingsManager.LoadSettings();
                SaveTimerStateToSettings(settings);
                Services.SettingsManager.SaveSettings(settings);
                LogManager.WriteDebugLog("TimerControl", $"已保存计时状态: isTimerInProgress={isTimerRunning}, isPaused={isPaused}, character={currentCharacter}, scene={currentScene}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存计时状态失败: {ex.Message}");
                LogManager.WriteDebugLog("TimerControl", $"保存计时状态失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 将计时状态保存到设置对象
        /// </summary>
        private void SaveTimerStateToSettings(AppSettings settings)
        {            
            settings.IsTimerInProgress = isTimerRunning;
            settings.TimerStartTime = startTime;
            settings.TimerPausedDuration = pausedDuration.TotalMilliseconds;
            settings.IsTimerPaused = isPaused;
            settings.TimerPauseStartTime = pauseStartTime;
            settings.InProgressCharacter = currentCharacter;
            settings.InProgressScene = currentScene;
        }
        
        /// <summary>
        /// 清除计时状态设置
        /// </summary>
        private static void ClearTimerState()
        {            
            try
            {
                var settings = Services.SettingsManager.LoadSettings();
                settings.IsTimerInProgress = false;
                settings.TimerStartTime = DateTime.MinValue;
                settings.TimerPausedDuration = 0;
                settings.IsTimerPaused = false;
                settings.TimerPauseStartTime = DateTime.MinValue;
                settings.InProgressCharacter = "";
                settings.InProgressScene = "";
                Services.SettingsManager.SaveSettings(settings);
                LogManager.WriteDebugLog("TimerControl", "已清除计时状态");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清除计时状态失败: {ex.Message}");
                LogManager.WriteDebugLog("TimerControl", $"清除计时状态失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 在应用程序关闭时调用，保存计时状态
        /// </summary>
        public void OnApplicationClosing()
        {            
            if (isTimerRunning)
            {
                // 如果计时器正在运行，更新未完成记录
                if (!isPaused)
                {
                    UpdateIncompleteRecord();
                }
                
                SaveTimerState();
                LogManager.WriteDebugLog("TimerControl", "应用程序关闭时保存了计时状态和未完成记录");
            }
        }
    }
}