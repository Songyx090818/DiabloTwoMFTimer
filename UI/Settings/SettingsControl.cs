using System;
using System.Windows.Forms;
using System.Drawing;
using DTwoMFTimerHelper.Utils; // 假设存在
using DTwoMFTimerHelper.Services; // 假设存在

namespace DTwoMFTimerHelper.UI.Settings {
    public partial class SettingsControl : UserControl {
        // 枚举定义
        public enum WindowPosition { TopLeft, TopCenter, TopRight, BottomLeft, BottomCenter, BottomRight }
        public enum LanguageOption { Chinese, English }

        // 事件定义
        public event EventHandler<WindowPositionChangedEventArgs>? WindowPositionChanged;
        public event EventHandler<LanguageChangedEventArgs>? LanguageChanged;
        public event EventHandler<AlwaysOnTopChangedEventArgs>? AlwaysOnTopChanged;
        public event EventHandler<HotkeyChangedEventArgs>? StartStopHotkeyChanged;
        public event EventHandler<HotkeyChangedEventArgs>? PauseHotkeyChanged;

        // 控件引用
        private TabControl tabControl;
        private TabPage tabPageGeneral;
        private TabPage tabPageHotkeys;
        private Button btnConfirmSettings;
        private Panel panelBottom; // 用于放置按钮的底部面板

        // 子组件引用
        private GeneralSettingsControl generalSettings;
        private HotkeySettingsControl hotkeySettings;

        public SettingsControl() {
            InitializeComponent();
            RefreshUI();
        }

        private void InitializeComponent() {
            tabControl = new TabControl();
            tabPageGeneral = new TabPage();
            generalSettings = new GeneralSettingsControl();
            tabPageHotkeys = new TabPage();
            hotkeySettings = new HotkeySettingsControl();
            btnConfirmSettings = new Button();
            panelBottom = new Panel();
            tabControl.SuspendLayout();
            tabPageGeneral.SuspendLayout();
            tabPageHotkeys.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPageGeneral);
            tabControl.Controls.Add(tabPageHotkeys);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(371, 391);
            tabControl.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            tabPageGeneral.Controls.Add(generalSettings);
            tabPageGeneral.Location = new Point(4, 37);
            tabPageGeneral.Name = "tabPageGeneral";
            tabPageGeneral.Padding = new Padding(3);
            tabPageGeneral.Size = new Size(363, 350);
            tabPageGeneral.TabIndex = 0;
            tabPageGeneral.Text = "通用";
            // 
            // generalSettings
            // 
            generalSettings.AutoScroll = true;
            generalSettings.Dock = DockStyle.Fill;
            generalSettings.Location = new Point(3, 3);
            generalSettings.Name = "generalSettings";
            generalSettings.Size = new Size(357, 344);
            generalSettings.TabIndex = 0;
            // 
            // tabPageHotkeys
            // 
            tabPageHotkeys.Controls.Add(hotkeySettings);
            tabPageHotkeys.Location = new Point(4, 37);
            tabPageHotkeys.Name = "tabPageHotkeys";
            tabPageHotkeys.Padding = new Padding(3);
            tabPageHotkeys.Size = new Size(332, 234);
            tabPageHotkeys.TabIndex = 1;
            tabPageHotkeys.Text = "快捷键";
            // 
            // hotkeySettings
            // 
            hotkeySettings.AutoScroll = true;
            hotkeySettings.Dock = DockStyle.Fill;
            hotkeySettings.Location = new Point(3, 3);
            hotkeySettings.Name = "hotkeySettings";
            hotkeySettings.Size = new Size(326, 228);
            hotkeySettings.TabIndex = 0;
            // 
            // btnConfirmSettings
            // 
            btnConfirmSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConfirmSettings.Location = new Point(321, 8);
            btnConfirmSettings.Name = "btnConfirmSettings";
            btnConfirmSettings.Size = new Size(80, 30);
            btnConfirmSettings.TabIndex = 0;
            btnConfirmSettings.Text = "确认";
            btnConfirmSettings.Click += BtnConfirmSettings_Click;
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(btnConfirmSettings);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 391);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(371, 45);
            panelBottom.TabIndex = 1;
            // 
            // SettingsControl
            // 
            Controls.Add(tabControl);
            Controls.Add(panelBottom);
            Name = "SettingsControl";
            Size = new Size(371, 436);
            tabControl.ResumeLayout(false);
            tabPageGeneral.ResumeLayout(false);
            tabPageHotkeys.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        public void RefreshUI() {
            if (this.InvokeRequired) { this.Invoke(new Action(RefreshUI)); return; }

            // 刷新自身（按钮等）
            btnConfirmSettings.Text = LanguageManager.GetString("ConfirmSettings");
            tabPageGeneral.Text = LanguageManager.GetString("General");
            tabPageHotkeys.Text = LanguageManager.GetString("Hotkeys");

            // 刷新子组件
            generalSettings.RefreshUI();
            hotkeySettings.RefreshUI();
        }

        private void BtnConfirmSettings_Click(object? sender, EventArgs e) {
            // 1. 获取通用设置并触发事件
            WindowPositionChanged?.Invoke(this, new WindowPositionChangedEventArgs(generalSettings.SelectedPosition));
            LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(generalSettings.SelectedLanguage));
            AlwaysOnTopChanged?.Invoke(this, new AlwaysOnTopChangedEventArgs(generalSettings.IsAlwaysOnTop));

            // 2. 获取快捷键设置并触发事件
            StartStopHotkeyChanged?.Invoke(this, new HotkeyChangedEventArgs(hotkeySettings.StartStopHotkey));
            PauseHotkeyChanged?.Invoke(this, new HotkeyChangedEventArgs(hotkeySettings.PauseHotkey));
        }

        public void ApplyWindowPosition(Form form) {
            MoveWindowToPosition(form, generalSettings.SelectedPosition);
        }

        // 静态辅助方法保持不变
        public static void MoveWindowToPosition(Form form, WindowPosition position) {
            Rectangle screenBounds = Screen.GetWorkingArea(form);
            int x, y;
            switch (position) {
                case WindowPosition.TopLeft: x = screenBounds.Left; y = screenBounds.Top; break;
                case WindowPosition.TopCenter: x = screenBounds.Left + (screenBounds.Width - form.Width) / 2; y = screenBounds.Top; break;
                case WindowPosition.TopRight: x = screenBounds.Right - form.Width; y = screenBounds.Top; break;
                case WindowPosition.BottomLeft: x = screenBounds.Left; y = screenBounds.Bottom - form.Height; break;
                case WindowPosition.BottomCenter: x = screenBounds.Left + (screenBounds.Width - form.Width) / 2; y = screenBounds.Bottom - form.Height; break;
                case WindowPosition.BottomRight: x = screenBounds.Right - form.Width; y = screenBounds.Bottom - form.Height; break;
                default: return;
            }
            form.Location = new Point(x, y);
        }

        // 事件参数类
        public class WindowPositionChangedEventArgs(WindowPosition position) : EventArgs { public WindowPosition Position { get; } = position; }
        public class LanguageChangedEventArgs(LanguageOption language) : EventArgs { public LanguageOption Language { get; } = language; }
        public class AlwaysOnTopChangedEventArgs(bool isAlwaysOnTop) : EventArgs { public bool IsAlwaysOnTop { get; } = isAlwaysOnTop; }
        public class HotkeyChangedEventArgs(Keys hotkey) : EventArgs { public Keys Hotkey { get; } = hotkey; }
    }
}