using System;
using System.Windows.Forms;
using System.Drawing;
using DTwoMFTimerHelper.Utils; // 假设存在
using DTwoMFTimerHelper.Services; // 假设存在

namespace DTwoMFTimerHelper.UI.Settings {
    public class GeneralSettingsControl : UserControl {
        private GroupBox groupBoxPosition = null!;
        private GroupBox groupBoxLanguage = null!;
        private RadioButton radioTopLeft = null!;
        private RadioButton radioTopCenter = null!;
        private RadioButton radioTopRight = null!;
        private RadioButton radioBottomLeft = null!;
        private RadioButton radioBottomCenter = null!;
        private RadioButton radioBottomRight = null!;
        private RadioButton chineseRadioButton = null!;
        private RadioButton englishRadioButton = null!;
        private CheckBox alwaysOnTopCheckBox = null!;
        private Label alwaysOnTopLabel = null!;
        public GeneralSettingsControl() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            groupBoxPosition = new GroupBox();
            radioTopLeft = new RadioButton();
            radioTopCenter = new RadioButton();
            radioTopRight = new RadioButton();
            radioBottomLeft = new RadioButton();
            radioBottomCenter = new RadioButton();
            radioBottomRight = new RadioButton();
            groupBoxLanguage = new GroupBox();
            chineseRadioButton = new RadioButton();
            englishRadioButton = new RadioButton();
            alwaysOnTopCheckBox = new CheckBox();
            alwaysOnTopLabel = new Label();
            groupBoxPosition.SuspendLayout();
            groupBoxLanguage.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxPosition
            // 
            groupBoxPosition.Controls.Add(radioTopLeft);
            groupBoxPosition.Controls.Add(radioTopCenter);
            groupBoxPosition.Controls.Add(radioTopRight);
            groupBoxPosition.Controls.Add(radioBottomLeft);
            groupBoxPosition.Controls.Add(radioBottomCenter);
            groupBoxPosition.Controls.Add(radioBottomRight);
            groupBoxPosition.Location = new Point(8, 8);
            groupBoxPosition.Name = "groupBoxPosition";
            groupBoxPosition.Size = new Size(300, 110);
            groupBoxPosition.TabIndex = 0;
            groupBoxPosition.TabStop = false;
            groupBoxPosition.Text = "窗口位置";
            // 
            // radioTopLeft
            // 
            radioTopLeft.Location = new Point(0, 0);
            radioTopLeft.Name = "radioTopLeft";
            radioTopLeft.Size = new Size(104, 24);
            radioTopLeft.TabIndex = 0;
            // 
            // radioTopCenter
            // 
            radioTopCenter.Location = new Point(0, 0);
            radioTopCenter.Name = "radioTopCenter";
            radioTopCenter.Size = new Size(104, 24);
            radioTopCenter.TabIndex = 1;
            // 
            // radioTopRight
            // 
            radioTopRight.Location = new Point(0, 0);
            radioTopRight.Name = "radioTopRight";
            radioTopRight.Size = new Size(104, 24);
            radioTopRight.TabIndex = 2;
            // 
            // radioBottomLeft
            // 
            radioBottomLeft.Location = new Point(0, 0);
            radioBottomLeft.Name = "radioBottomLeft";
            radioBottomLeft.Size = new Size(104, 24);
            radioBottomLeft.TabIndex = 3;
            // 
            // radioBottomCenter
            // 
            radioBottomCenter.Location = new Point(0, 0);
            radioBottomCenter.Name = "radioBottomCenter";
            radioBottomCenter.Size = new Size(104, 24);
            radioBottomCenter.TabIndex = 4;
            // 
            // radioBottomRight
            // 
            radioBottomRight.Location = new Point(0, 0);
            radioBottomRight.Name = "radioBottomRight";
            radioBottomRight.Size = new Size(104, 24);
            radioBottomRight.TabIndex = 5;
            // 
            // groupBoxLanguage
            // 
            groupBoxLanguage.Controls.Add(chineseRadioButton);
            groupBoxLanguage.Controls.Add(englishRadioButton);
            groupBoxLanguage.Location = new Point(8, 144);
            groupBoxLanguage.Name = "groupBoxLanguage";
            groupBoxLanguage.Size = new Size(300, 70);
            groupBoxLanguage.TabIndex = 1;
            groupBoxLanguage.TabStop = false;
            groupBoxLanguage.Text = "语言";
            // 
            // chineseRadioButton
            // 
            chineseRadioButton.Location = new Point(0, 0);
            chineseRadioButton.Name = "chineseRadioButton";
            chineseRadioButton.Size = new Size(104, 24);
            chineseRadioButton.TabIndex = 0;
            // 
            // englishRadioButton
            // 
            englishRadioButton.Location = new Point(0, 0);
            englishRadioButton.Name = "englishRadioButton";
            englishRadioButton.Size = new Size(104, 24);
            englishRadioButton.TabIndex = 1;
            // 
            // alwaysOnTopCheckBox
            // 
            alwaysOnTopCheckBox.Checked = true;
            alwaysOnTopCheckBox.CheckState = CheckState.Checked;
            alwaysOnTopCheckBox.Location = new Point(130, 242);
            alwaysOnTopCheckBox.Name = "alwaysOnTopCheckBox";
            alwaysOnTopCheckBox.Size = new Size(104, 24);
            alwaysOnTopCheckBox.TabIndex = 3;
            // 
            // alwaysOnTopLabel
            // 
            alwaysOnTopLabel.AutoSize = true;
            alwaysOnTopLabel.Location = new Point(8, 242);
            alwaysOnTopLabel.Name = "alwaysOnTopLabel";
            alwaysOnTopLabel.Size = new Size(96, 28);
            alwaysOnTopLabel.TabIndex = 2;
            alwaysOnTopLabel.Text = "总在最前";
            // 
            // GeneralSettingsControl
            // 
            AutoScroll = true;
            Controls.Add(groupBoxPosition);
            Controls.Add(groupBoxLanguage);
            Controls.Add(alwaysOnTopLabel);
            Controls.Add(alwaysOnTopCheckBox);
            Name = "GeneralSettingsControl";
            Size = new Size(388, 406);
            groupBoxPosition.ResumeLayout(false);
            groupBoxLanguage.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void SetupRadio(RadioButton rb, string text, int x, int y, bool isChecked = false) {
            rb.Text = text;
            rb.Location = new Point(x, y);
            rb.AutoSize = true;
            rb.Checked = isChecked;
        }

        public void RefreshUI() {
            if (this.InvokeRequired) { this.Invoke(new Action(RefreshUI)); return; }

            groupBoxPosition!.Text = LanguageManager.GetString("WindowPosition");
            radioTopLeft!.Text = LanguageManager.GetString("TopLeft");
            radioTopCenter!.Text = LanguageManager.GetString("TopCenter");
            radioTopRight!.Text = LanguageManager.GetString("TopRight");
            radioBottomLeft!.Text = LanguageManager.GetString("BottomLeft");
            radioBottomCenter!.Text = LanguageManager.GetString("BottomCenter");
            radioBottomRight!.Text = LanguageManager.GetString("BottomRight");

            groupBoxLanguage!.Text = LanguageManager.GetString("Language");
            chineseRadioButton!.Text = LanguageManager.GetString("Chinese");
            englishRadioButton!.Text = LanguageManager.GetString("English");

            alwaysOnTopLabel!.Text = LanguageManager.GetString("AlwaysOnTop");
        }


        // --- 公开属性供父控件获取数据 ---

        public SettingsControl.WindowPosition SelectedPosition {
            get {
                if (radioTopLeft.Checked) return SettingsControl.WindowPosition.TopLeft;
                if (radioTopCenter.Checked) return SettingsControl.WindowPosition.TopCenter;
                if (radioTopRight.Checked) return SettingsControl.WindowPosition.TopRight;
                if (radioBottomLeft.Checked) return SettingsControl.WindowPosition.BottomLeft;
                if (radioBottomCenter.Checked) return SettingsControl.WindowPosition.BottomCenter;
                if (radioBottomRight.Checked) return SettingsControl.WindowPosition.BottomRight;
                return SettingsControl.WindowPosition.TopLeft;
            }
        }

        public SettingsControl.LanguageOption SelectedLanguage {
            get {
                return chineseRadioButton.Checked ? SettingsControl.LanguageOption.Chinese : SettingsControl.LanguageOption.English;
            }
        }
        public bool IsAlwaysOnTop => alwaysOnTopCheckBox?.Checked ?? false;
    }
}