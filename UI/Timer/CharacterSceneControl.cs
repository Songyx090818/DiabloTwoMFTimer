using System.Drawing;
using System;
using System.Windows.Forms;
using DTwoMFTimerHelper.Utils;
using DTwoMFTimerHelper.Models;

namespace DTwoMFTimerHelper.UI.Timer
{
    public partial class CharacterSceneControl : UserControl
    {
        private Label? lblCharacterDisplay;
        private Label? lblSceneDisplay;

        // 角色和场景数据
        public string CurrentCharacter { get; set; } = "";
        public string CurrentScene { get; set; } = "";
        public CharacterProfile? CurrentProfile { get; set; } = null;
        public string DifficultyText { get; set; } = "";

        public CharacterSceneControl()
        {
            InitializeComponent();
            // 注册语言变更事件
            Utils.LanguageManager.OnLanguageChanged += LanguageManager_OnLanguageChanged;
        }
        
        protected override void Dispose(bool disposing)
        {            if (disposing)
            {
                // 取消注册语言变更事件
                Utils.LanguageManager.OnLanguageChanged -= LanguageManager_OnLanguageChanged;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // 角色显示
            lblCharacterDisplay = new Label
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                Location = new Point(0, 0),
                Name = "lblCharacterDisplay",
                Size = new Size(290, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                TabIndex = 0,
                Parent = this
            };

            // 场景显示
            lblSceneDisplay = new Label
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                Location = new Point(0, 25),
                Name = "lblSceneDisplay",
                Size = new Size(290, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                TabIndex = 1,
                Parent = this
            };

            // 控件设置
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Size = new Size(290, 50);
            Name = "CharacterSceneControl";
        }

        private void LanguageManager_OnLanguageChanged(object? sender, EventArgs e)
        {
            UpdateUI();
        }

        public void UpdateCharacterSceneInfo(string characterName, CharacterProfile? profile, string sceneName, string difficultyText)
        {
            // 更新属性
            this.CurrentCharacter = characterName;
            this.CurrentProfile = profile;
            this.CurrentScene = sceneName;
            this.DifficultyText = difficultyText;
            
            // 更新UI
            UpdateUI();
        }
        
        /// <summary>
        /// 设置角色和场景信息
        /// </summary>
        /// <param name="character">角色名称</param>
        /// <param name="scene">场景名称</param>
        public void SetCharacterAndScene(string character, string scene)
        {
            CurrentCharacter = character;
            
            // 使用DataManager获取对应的英文场景名称
            string englishSceneName = DTwoMFTimerHelper.Services.DataManager.GetEnglishSceneName(scene);
            
            CurrentScene = englishSceneName;
            
            Utils.LogManager.WriteDebugLog("CharacterSceneControl", $"SetCharacterAndScene 调用: 角色={character}, 原始场景={scene}, 英文场景={englishSceneName}");
            
            // 尝试根据角色名称查找对应的角色档案
            if (!string.IsNullOrEmpty(character))
            {
                try
                {
                    Utils.LogManager.WriteDebugLog("CharacterSceneControl", $"尝试根据角色名称 '{character}' 查找对应的角色档案");
                    CurrentProfile = DTwoMFTimerHelper.Services.DataManager.FindProfileByName(character);
                    if (CurrentProfile != null)
                    {
                        Utils.LogManager.WriteDebugLog("CharacterSceneControl", $"已关联角色档案: {character}, 档案名称={CurrentProfile.Name}");
                    }
                    else
                    {
                        Utils.LogManager.WriteDebugLog("CharacterSceneControl", $"未找到角色档案: {character}");
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogManager.WriteDebugLog("CharacterSceneControl", $"查找角色档案失败: {ex.Message}");
                }
            }
            
            // 保存当前角色、场景和难度到设置
            SaveCharacterSceneSettings(character, scene);
            
            UpdateUI();
        }
        
        /// <summary>
        /// 保存角色和场景设置
        /// </summary>
        /// <param name="character">角色名称</param>
        /// <param name="scene">场景名称</param>
        private void SaveCharacterSceneSettings(string character, string scene)
        {
            if (!string.IsNullOrEmpty(character))
            {
                try
                {
                    var settings = DTwoMFTimerHelper.Services.SettingsManager.LoadSettings();
                    // 提取纯角色名称，去除可能包含的职业信息 (如 "AAA (刺客)" -> "AAA")
                    string pureCharacterName = character;
                    if (character.Contains(" ("))
                    {
                        int index = character.IndexOf(" (");
                        pureCharacterName = character.Substring(0, index);
                        Utils.LogManager.WriteDebugLog("CharacterSceneControl", $"已从角色名称中提取纯名称: 原名称='{character}', 提取后='{pureCharacterName}'");
                    }
                    
                    // 获取当前难度（默认地狱难度）
                    string difficulty = Models.GameDifficulty.Hell.ToString();
                    
                    settings.LastUsedProfile = pureCharacterName;
                    settings.LastUsedScene = scene;
                    settings.LastUsedDifficulty = difficulty;
                    DTwoMFTimerHelper.Services.SettingsManager.SaveSettings(settings);
                    Utils.LogManager.WriteDebugLog("CharacterSceneControl", $"已保存设置到配置文件: LastUsedProfile={pureCharacterName}, LastUsedScene={scene}, LastUsedDifficulty={difficulty}");
                }
                catch (Exception ex)
                {
                    Utils.LogManager.WriteDebugLog("CharacterSceneControl", $"保存设置失败: {ex.Message}");
                }
            }
        }

        public void UpdateUI()
        {
            // 更新角色显示
            if (lblCharacterDisplay != null)
            {
                if (string.IsNullOrEmpty(CurrentCharacter))
                {
                    lblCharacterDisplay.Text = "";
                }
                else
                {
                    // 获取角色职业信息
                    string characterClass = "";
                    if (CurrentProfile != null)
                    {
                        // 使用LogManager中的统一方法获取本地化职业名称
                        characterClass = Utils.LanguageManager.GetLocalizedClassName(CurrentProfile.Class);
                    }

                    // 检查currentCharacter是否已经包含职业信息格式
                    if (!string.IsNullOrEmpty(characterClass))
                    {
                        // 如果currentCharacter已经包含括号格式，只显示纯角色名称加职业
                        string displayName = CurrentCharacter;
                        if (CurrentCharacter.Contains(" ("))
                        {
                            int index = CurrentCharacter.IndexOf(" (");
                            displayName = CurrentCharacter.Substring(0, index);
                        }
                        lblCharacterDisplay.Text = $"{displayName} ({characterClass})";
                    }
                    else
                    {
                        // 如果没有职业信息，只显示角色名称
                        lblCharacterDisplay.Text = CurrentCharacter;
                    }
                }
            }

            // 更新场景显示
            if (lblSceneDisplay != null)
            {
                if (string.IsNullOrEmpty(CurrentScene))
                {
                    lblSceneDisplay.Text = "";
                }
                else
                {
                    // 直接使用LanguageManager.GetString获取本地化的场景名称
                    string localizedSceneName = Utils.LanguageManager.GetString(CurrentScene);

                    // 在场景名称前添加难度
                    lblSceneDisplay.Text = $"{DifficultyText} {localizedSceneName}";
                }
            }
        }
    }
}