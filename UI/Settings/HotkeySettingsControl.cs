using System;
using System.Drawing;
using System.Windows.Forms;
using DiabloTwoMFTimer.Interfaces;
using DiabloTwoMFTimer.Utils;
using DiabloTwoMFTimer.UI.Theme; // 引用主题

namespace DiabloTwoMFTimer.UI.Settings;

public partial class HotkeySettingsControl : UserControl
{
    // 修改为使用主题颜色
    private readonly Color ColorNormal = AppTheme.SurfaceColor;
    // 编辑状态用稍亮一点的颜色，或者带点颜色的背景
    private readonly Color ColorEditing = Color.FromArgb(60, 60, 70);

    private bool _isUpdating = false;

    public Keys StartOrNextRunHotkey { get; private set; }
    public Keys PauseHotkey { get; private set; }
    public Keys DeleteHistoryHotkey { get; private set; }
    public Keys RecordLootHotkey { get; private set; }

    public HotkeySettingsControl()
    {
        InitializeComponent();
        InitializeTextBoxStyles(); // 初始化样式
    }

    private void InitializeTextBoxStyles()
    {
        // 确保初始加载时颜色正确
        ApplyStyle(txtStartNext);
        ApplyStyle(txtPause);
        ApplyStyle(txtDeleteHistory);
        ApplyStyle(txtRecordLoot);
    }

    private void ApplyStyle(TextBox tb)
    {
        tb.BackColor = ColorNormal;
        tb.ForeColor = AppTheme.TextColor;
        tb.BorderStyle = BorderStyle.FixedSingle;
    }

    private void OnTextBoxEnter(object? sender, EventArgs e)
    {
        if (sender is not TextBox textBox)
            return;

        if (_isUpdating)
        {
            _isUpdating = false;
            return;
        }

        textBox.BackColor = ColorEditing;
        textBox.ForeColor = AppTheme.AccentColor; // 编辑时高亮文字
        textBox.Text = LanguageManager.GetString("HotkeyPressToSet") ?? "请按快捷键 (Esc取消)";
    }

    private void OnTextBoxLeave(object? sender, EventArgs e)
    {
        if (sender is not TextBox textBox)
            return;

        textBox.BackColor = ColorNormal;
        textBox.ForeColor = AppTheme.TextColor; // 恢复文字颜色

        string tag = textBox.Tag?.ToString() ?? "";
        Keys currentKey = Keys.None;
        switch (tag)
        {
            case "StartNext":
                currentKey = StartOrNextRunHotkey;
                break;
            case "Pause":
                currentKey = PauseHotkey;
                break;
            case "Delete":
                currentKey = DeleteHistoryHotkey;
                break;
            case "Record":
                currentKey = RecordLootHotkey;
                break;
        }
        textBox.Text = FormatKeyString(currentKey);
        _isUpdating = false;
    }

    private void OnHotkeyInput(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox)
            return;

        e.SuppressKeyPress = true;

        if (e.KeyCode == Keys.Escape)
        {
            this.Focus();
            return;
        }

        if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
        {
            _isUpdating = true;
            UpdateHotkey(textBox, Keys.None);
            this.Focus();
            return;
        }

        if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Menu)
        {
            return;
        }

        Keys keyData = e.KeyCode;
        if (e.Control) keyData |= Keys.Control;
        if (e.Shift) keyData |= Keys.Shift;
        if (e.Alt) keyData |= Keys.Alt;

        _isUpdating = true;
        UpdateHotkey(textBox, keyData);
        this.Focus();
    }

    private void UpdateHotkey(TextBox textBox, Keys newKey)
    {
        string tag = textBox.Tag?.ToString() ?? "";
        switch (tag)
        {
            case "StartNext":
                StartOrNextRunHotkey = newKey;
                break;
            case "Pause":
                PauseHotkey = newKey;
                break;
            case "Delete":
                DeleteHistoryHotkey = newKey;
                break;
            case "Record":
                RecordLootHotkey = newKey;
                break;
        }
        textBox.Text = FormatKeyString(newKey);
        textBox.BackColor = ColorNormal;
        textBox.ForeColor = AppTheme.TextColor;
    }

    // ... 其余 LoadHotkeys, RefreshUI, FormatKeyString 方法保持不变 ...

    private string FormatKeyString(Keys key)
    {
        if (key == Keys.None)
            return "无 (None)";
        var converter = new KeysConverter();
        return converter.ConvertToString(key) ?? "None";
    }

    public void LoadHotkeys(IAppSettings settings)
    {
        StartOrNextRunHotkey = settings.HotkeyStartOrNext;
        PauseHotkey = settings.HotkeyPause;
        DeleteHistoryHotkey = settings.HotkeyDeleteHistory;
        RecordLootHotkey = settings.HotkeyRecordLoot;

        txtStartNext.Text = FormatKeyString(StartOrNextRunHotkey);
        txtPause.Text = FormatKeyString(PauseHotkey);
        txtDeleteHistory.Text = FormatKeyString(DeleteHistoryHotkey);
        txtRecordLoot.Text = FormatKeyString(RecordLootHotkey);
    }

    public void RefreshUI()
    {
        this.SafeInvoke(() =>
        {
            if (grpHotkeys == null)
                return;
            try
            {
                grpHotkeys.Text = LanguageManager.GetString("HotkeySettingsGroup");
                lblStartNext.Text = LanguageManager.GetString("HotkeyStartNext");
                lblPause.Text = LanguageManager.GetString("HotkeyPause");
                lblDeleteHistory.Text = LanguageManager.GetString("HotkeyDeleteHistory");
                lblRecordLoot.Text = LanguageManager.GetString("HotkeyRecordLoot");

                LoadHotkeys(
                    new Services.AppSettings
                    {
                        HotkeyStartOrNext = StartOrNextRunHotkey,
                        HotkeyPause = PauseHotkey,
                        HotkeyDeleteHistory = DeleteHistoryHotkey,
                        HotkeyRecordLoot = RecordLootHotkey,
                    }
                );
            }
            catch { }
        });
    }
}