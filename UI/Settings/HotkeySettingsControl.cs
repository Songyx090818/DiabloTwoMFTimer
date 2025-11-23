using System;
using System.Windows.Forms;
using System.Drawing;
using DTwoMFTimerHelper.Utils; // 假设存在
using DTwoMFTimerHelper.Services; // 假设存在

namespace DTwoMFTimerHelper.UI.Settings {
    public class HotkeySettingsControl : UserControl {
        private GroupBox? groupBoxHotkeys;
        private Label? labelStartStopTitle;
        private Label? labelPauseTitle;
        private Label? labelStartStopDisplay;
        private Label? labelPauseDisplay;

        // 数据字段
        public Keys StartStopHotkey { get; private set; } = Keys.Q | Keys.Alt;
        public Keys PauseHotkey { get; private set; } = Keys.Space | Keys.Control;

        private bool isSettingHotkey = false;
        private string currentHotkeySetting = string.Empty;

        public event EventHandler<SettingsControl.HotkeyChangedEventArgs>? HotkeyChanged; // 可选：如果需要实时预览

        public HotkeySettingsControl() {
            InitializeComponent();
            UpdateHotkeyLabels();
        }

        private void InitializeComponent() {
            this.groupBoxHotkeys = new GroupBox();
            this.labelStartStopTitle = new Label();
            this.labelPauseTitle = new Label();
            this.labelStartStopDisplay = new Label();
            this.labelPauseDisplay = new Label();

            this.AutoScroll = true; // 开启滚动条支持
            this.Size = new Size(320, 280);

            // GroupBox
            this.groupBoxHotkeys.Text = "Hotkey Settings";
            this.groupBoxHotkeys.Location = new Point(8, 8);
            this.groupBoxHotkeys.Size = new Size(300, 120);

            // Labels Layout
            this.labelStartStopTitle.Location = new Point(10, 30);
            this.labelStartStopTitle.AutoSize = true;
            this.labelStartStopTitle.Text = "Start/Stop";

            this.labelStartStopDisplay.Location = new Point(80, 26);
            this.labelStartStopDisplay.Size = new Size(200, 24);
            this.labelStartStopDisplay.BorderStyle = BorderStyle.FixedSingle;
            this.labelStartStopDisplay.TextAlign = ContentAlignment.MiddleCenter;
            this.labelStartStopDisplay.Cursor = Cursors.Hand;

            this.labelPauseTitle.Location = new Point(10, 70);
            this.labelPauseTitle.AutoSize = true;
            this.labelPauseTitle.Text = "Pause";

            this.labelPauseDisplay.Location = new Point(80, 66);
            this.labelPauseDisplay.Size = new Size(200, 24);
            this.labelPauseDisplay.BorderStyle = BorderStyle.FixedSingle;
            this.labelPauseDisplay.TextAlign = ContentAlignment.MiddleCenter;
            this.labelPauseDisplay.Cursor = Cursors.Hand;

            this.groupBoxHotkeys.Controls.AddRange(new Control[] {
                labelStartStopTitle, labelStartStopDisplay,
                labelPauseTitle, labelPauseDisplay
            });

            this.Controls.Add(groupBoxHotkeys);
        }

        public void RefreshUI() {
            if (this.InvokeRequired) { this.Invoke(new Action(RefreshUI)); return; }

            groupBoxHotkeys!.Text = LanguageManager.GetString("HotkeySettings");
            labelStartStopTitle!.Text = LanguageManager.GetString("StartStop");
            labelPauseTitle!.Text = LanguageManager.GetString("Pause");
            // 保持当前显示的快捷键文本
            UpdateHotkeyLabels();
        }

        private void UpdateHotkeyLabels() {
            labelStartStopDisplay!.Text = GetHotkeyDisplayText(StartStopHotkey);
            labelPauseDisplay!.Text = GetHotkeyDisplayText(PauseHotkey);
        }

        private static string GetHotkeyDisplayText(Keys keys) {
            // (复用原逻辑)
            string text = string.Empty;
            if ((keys & Keys.Control) == Keys.Control) text += "Ctrl + ";
            if ((keys & Keys.Alt) == Keys.Alt) text += "Alt + ";
            if ((keys & Keys.Shift) == Keys.Shift) text += "Shift + ";
            Keys key = keys & ~Keys.Control & ~Keys.Alt & ~Keys.Shift;
            if (key != Keys.None) text += key.ToString();
            return text;
        }

        private void StartHotkeySetup(string type) {
            isSettingHotkey = true;
            currentHotkeySetting = type;

            string prompt = "Press Key...";
            if (type == "StartStop") labelStartStopDisplay!.Text = prompt;
            else labelPauseDisplay!.Text = prompt;

            // 确保控件获得焦点以捕获键盘事件
            this.Focus();
            // 移除旧的处理程序防止多重绑定
            this.KeyDown -= OnKeyDownWhileSettingHotkey;
            this.KeyDown += OnKeyDownWhileSettingHotkey;
        }

        private void OnKeyDownWhileSettingHotkey(object? sender, KeyEventArgs e) {
            if (!isSettingHotkey) return;
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu || e.KeyCode == Keys.ShiftKey) return;

            Keys newHotkey = e.KeyCode;
            if (e.Control) newHotkey |= Keys.Control;
            if (e.Alt) newHotkey |= Keys.Alt;
            if (e.Shift) newHotkey |= Keys.Shift;

            if (currentHotkeySetting == "StartStop") StartStopHotkey = newHotkey;
            else PauseHotkey = newHotkey;

            isSettingHotkey = false;
            this.KeyDown -= OnKeyDownWhileSettingHotkey;
            UpdateHotkeyLabels();

            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        // 确保 UserControl 可以接收焦点
        protected override bool IsInputKey(Keys keyData) {
            if (isSettingHotkey) return true;
            return base.IsInputKey(keyData);
        }
    }
}