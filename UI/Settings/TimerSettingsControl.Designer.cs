namespace DiabloTwoMFTimer.UI.Settings;
partial class TimerSettingsControl
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
        this.grpTimerSettings = new DiabloTwoMFTimer.UI.Components.ThemedGroupBox();
        this.chkSyncPausePomodoro = new DiabloTwoMFTimer.UI.Components.ThemedCheckBox();
        this.chkGenerateRoomName = new DiabloTwoMFTimer.UI.Components.ThemedCheckBox();
        this.chkShowPomodoro = new DiabloTwoMFTimer.UI.Components.ThemedCheckBox();
        this.chkShowLootDrops = new DiabloTwoMFTimer.UI.Components.ThemedCheckBox();
        this.chkSyncStartPomodoro = new DiabloTwoMFTimer.UI.Components.ThemedCheckBox();
        this.grpTimerSettings.SuspendLayout();
        this.BackColor = DiabloTwoMFTimer.UI.Theme.AppTheme.BackColor;
        this.SuspendLayout();
        // 
        // grpTimerSettings
        // 
        this.grpTimerSettings.Controls.Add(this.chkGenerateRoomName);
        this.grpTimerSettings.Controls.Add(this.chkSyncPausePomodoro);
        this.grpTimerSettings.Controls.Add(this.chkSyncStartPomodoro);
        this.grpTimerSettings.Controls.Add(this.chkShowLootDrops);
        this.grpTimerSettings.Controls.Add(this.chkShowPomodoro);
        this.grpTimerSettings.Location = new System.Drawing.Point(10, 10);
        this.grpTimerSettings.Name = "grpTimerSettings";
        this.grpTimerSettings.Size = new System.Drawing.Size(330, 280); // 增加高度
        this.grpTimerSettings.TabIndex = 0;
        this.grpTimerSettings.TabStop = false;
        this.grpTimerSettings.Text = "计时器设置";
        // 
        // chkShowPomodoro
        // 
        this.chkShowPomodoro.AutoSize = true;
        this.chkShowPomodoro.Location = new System.Drawing.Point(20, 40); // 增加边距
        this.chkShowPomodoro.Name = "chkShowPomodoro";
        this.chkShowPomodoro.Size = new System.Drawing.Size(150, 30);
        this.chkShowPomodoro.TabIndex = 0;
        this.chkShowPomodoro.Text = "是否显示番茄钟";
        this.chkShowPomodoro.CheckedChanged += new System.EventHandler(this.OnShowPomodoroChanged);
        // 
        // chkShowLootDrops
        // 
        this.chkShowLootDrops.AutoSize = true;
        this.chkShowLootDrops.Location = new System.Drawing.Point(20, 85); // 增加间距
        this.chkShowLootDrops.Name = "chkShowLootDrops";
        this.chkShowLootDrops.Size = new System.Drawing.Size(150, 30);
        this.chkShowLootDrops.TabIndex = 1;
        this.chkShowLootDrops.Text = "是否展示掉落";
        this.chkShowLootDrops.CheckedChanged += new System.EventHandler(this.OnShowLootDropsChanged);
        // 
        // chkSyncStartPomodoro
        // 
        this.chkSyncStartPomodoro.AutoSize = true;
        this.chkSyncStartPomodoro.Location = new System.Drawing.Point(20, 130);
        this.chkSyncStartPomodoro.Name = "chkSyncStartPomodoro";
        this.chkSyncStartPomodoro.Size = new System.Drawing.Size(240, 30);
        this.chkSyncStartPomodoro.TabIndex = 2;
        this.chkSyncStartPomodoro.Text = "同步开启番茄钟";
        this.chkSyncStartPomodoro.CheckedChanged += new System.EventHandler(this.OnSyncStartPomodoroChanged);
        // 
        // chkSyncPausePomodoro
        // 
        this.chkSyncPausePomodoro.AutoSize = true;
        this.chkSyncPausePomodoro.Location = new System.Drawing.Point(20, 175);
        this.chkSyncPausePomodoro.Name = "chkSyncPausePomodoro";
        this.chkSyncPausePomodoro.Size = new System.Drawing.Size(240, 30);
        this.chkSyncPausePomodoro.TabIndex = 3;
        this.chkSyncPausePomodoro.Text = "同步暂停番茄钟";
        this.chkSyncPausePomodoro.CheckedChanged += new System.EventHandler(this.OnSyncPausePomodoroChanged);
        // 
        // chkGenerateRoomName
        // 
        this.chkGenerateRoomName.AutoSize = true;
        this.chkGenerateRoomName.Location = new System.Drawing.Point(20, 220);
        this.chkGenerateRoomName.Name = "chkGenerateRoomName";
        this.chkGenerateRoomName.Size = new System.Drawing.Size(150, 30);
        this.chkGenerateRoomName.TabIndex = 4;
        this.chkGenerateRoomName.Text = "生成房间名称";
        this.chkGenerateRoomName.CheckedChanged += new System.EventHandler(this.OnGenerateRoomNameChanged);
        // 
        // TimerSettingsControl
        // 
        this.Controls.Add(this.grpTimerSettings);
        this.Name = "TimerSettingsControl";
        this.Size = new System.Drawing.Size(350, 300);
        this.grpTimerSettings.ResumeLayout(false);
        this.grpTimerSettings.PerformLayout();
        this.ResumeLayout(false);
    }

    #endregion

    private DiabloTwoMFTimer.UI.Components.ThemedGroupBox grpTimerSettings;
    private DiabloTwoMFTimer.UI.Components.ThemedCheckBox chkShowPomodoro;
    private DiabloTwoMFTimer.UI.Components.ThemedCheckBox chkShowLootDrops;
    private DiabloTwoMFTimer.UI.Components.ThemedCheckBox chkSyncStartPomodoro;
    private DiabloTwoMFTimer.UI.Components.ThemedCheckBox chkSyncPausePomodoro;
    private DiabloTwoMFTimer.UI.Components.ThemedCheckBox chkGenerateRoomName;
}