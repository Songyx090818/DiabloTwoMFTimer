#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;
using DiabloTwoMFTimer.UI.Components;
using DiabloTwoMFTimer.UI.Theme;

namespace DiabloTwoMFTimer.UI.Pomodoro;

partial class PomodoroSettingsForm
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

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        // 使用 ThemedLabel
        this.lblWorkTime = new ThemedLabel();
        this.lblShortBreakTime = new ThemedLabel();
        this.lblLongBreakTime = new ThemedLabel();
        this.lblWarningLongTime = new ThemedLabel();
        this.lblWarningShortTime = new ThemedLabel();

        this.lblWorkMinUnit = new ThemedLabel();
        this.lblShortBreakMinUnit = new ThemedLabel();
        this.lblLongBreakMinUnit = new ThemedLabel();
        this.lblWarningLongTimeUnit = new ThemedLabel();
        this.lblWarningShortTimeUnit = new ThemedLabel();

        this.lblWorkSecUnit = new ThemedLabel();
        this.lblShortBreakSecUnit = new ThemedLabel();
        this.lblLongBreakSecUnit = new ThemedLabel();

        // NumericUpDown
        this.nudWorkTimeMin = new System.Windows.Forms.NumericUpDown();
        this.nudWorkTimeSec = new System.Windows.Forms.NumericUpDown();
        this.nudShortBreakTimeMin = new System.Windows.Forms.NumericUpDown();
        this.nudShortBreakTimeSec = new System.Windows.Forms.NumericUpDown();
        this.nudLongBreakTimeMin = new System.Windows.Forms.NumericUpDown();
        this.nudLongBreakTimeSec = new System.Windows.Forms.NumericUpDown();
        this.nudWarningLongTime = new System.Windows.Forms.NumericUpDown();
        this.nudWarningShortTime = new System.Windows.Forms.NumericUpDown();

        ((System.ComponentModel.ISupportInitialize)(this.nudWorkTimeMin)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudWorkTimeSec)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudShortBreakTimeMin)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudShortBreakTimeSec)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudLongBreakTimeMin)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudLongBreakTimeSec)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudWarningLongTime)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudWarningShortTime)).BeginInit();

        this.SuspendLayout();

        // 布局常量
        int labelX = 30;
        int inputMinX = 140;
        int labelMinX = 215;
        int inputSecX = 250;
        int labelSecX = 325;
        int inputWarningX = 140;
        int labelWarningX = 215;

        // Y轴坐标 (全部 +35 偏移)
        int row1Y = 30 + 35;
        int row2Y = 70 + 35;
        int row3Y = 110 + 35;
        int row4Y = 150 + 35;
        int row5Y = 190 + 35;
        int textOffsetY = 4;

        // --- 样式设置辅助 ---
        void SetNudStyle(NumericUpDown nud)
        {
            nud.BackColor = AppTheme.SurfaceColor;
            nud.ForeColor = AppTheme.TextColor;
            nud.BorderStyle = BorderStyle.FixedSingle;
        }

        SetNudStyle(nudWorkTimeMin);
        SetNudStyle(nudWorkTimeSec);
        SetNudStyle(nudShortBreakTimeMin);
        SetNudStyle(nudShortBreakTimeSec);
        SetNudStyle(nudLongBreakTimeMin);
        SetNudStyle(nudLongBreakTimeSec);
        SetNudStyle(nudWarningLongTime);
        SetNudStyle(nudWarningShortTime);

        // --- 第一行：工作时间 ---
        this.lblWorkTime.AutoSize = true;
        this.lblWorkTime.Location = new System.Drawing.Point(labelX, row1Y + textOffsetY);
        this.lblWorkTime.Name = "lblWorkTime";
        this.lblWorkTime.Text = "工作时间:";

        this.nudWorkTimeMin.Location = new System.Drawing.Point(inputMinX, row1Y);
        this.nudWorkTimeMin.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        this.nudWorkTimeMin.Name = "nudWorkTimeMin";
        this.nudWorkTimeMin.Size = new System.Drawing.Size(70, 25);

        this.lblWorkMinUnit.AutoSize = true;
        this.lblWorkMinUnit.Location = new System.Drawing.Point(labelMinX, row1Y + textOffsetY);
        this.lblWorkMinUnit.Name = "lblWorkMinUnit";
        this.lblWorkMinUnit.Text = "分";

        this.nudWorkTimeSec.Location = new System.Drawing.Point(inputSecX, row1Y);
        this.nudWorkTimeSec.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
        this.nudWorkTimeSec.Name = "nudWorkTimeSec";
        this.nudWorkTimeSec.Size = new System.Drawing.Size(70, 25);

        this.lblWorkSecUnit.AutoSize = true;
        this.lblWorkSecUnit.Location = new System.Drawing.Point(labelSecX, row1Y + textOffsetY);
        this.lblWorkSecUnit.Name = "lblWorkSecUnit";
        this.lblWorkSecUnit.Text = "秒";

        // --- 第二行：短休息 ---
        this.lblShortBreakTime.AutoSize = true;
        this.lblShortBreakTime.Location = new System.Drawing.Point(labelX, row2Y + textOffsetY);
        this.lblShortBreakTime.Name = "lblShortBreakTime";
        this.lblShortBreakTime.Text = "短休息时间:";

        this.nudShortBreakTimeMin.Location = new System.Drawing.Point(inputMinX, row2Y);
        this.nudShortBreakTimeMin.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        this.nudShortBreakTimeMin.Name = "nudShortBreakTimeMin";
        this.nudShortBreakTimeMin.Size = new System.Drawing.Size(70, 25);

        this.lblShortBreakMinUnit.AutoSize = true;
        this.lblShortBreakMinUnit.Location = new System.Drawing.Point(labelMinX, row2Y + textOffsetY);
        this.lblShortBreakMinUnit.Name = "lblShortBreakMinUnit";
        this.lblShortBreakMinUnit.Text = "分";

        this.nudShortBreakTimeSec.Location = new System.Drawing.Point(inputSecX, row2Y);
        this.nudShortBreakTimeSec.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
        this.nudShortBreakTimeSec.Name = "nudShortBreakTimeSec";
        this.nudShortBreakTimeSec.Size = new System.Drawing.Size(70, 25);

        this.lblShortBreakSecUnit.AutoSize = true;
        this.lblShortBreakSecUnit.Location = new System.Drawing.Point(labelSecX, row2Y + textOffsetY);
        this.lblShortBreakSecUnit.Name = "lblShortBreakSecUnit";
        this.lblShortBreakSecUnit.Text = "秒";

        // --- 第三行：长休息 ---
        this.lblLongBreakTime.AutoSize = true;
        this.lblLongBreakTime.Location = new System.Drawing.Point(labelX, row3Y + textOffsetY);
        this.lblLongBreakTime.Name = "lblLongBreakTime";
        this.lblLongBreakTime.Text = "长休息时间:";

        this.nudLongBreakTimeMin.Location = new System.Drawing.Point(inputMinX, row3Y);
        this.nudLongBreakTimeMin.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        this.nudLongBreakTimeMin.Name = "nudLongBreakTimeMin";
        this.nudLongBreakTimeMin.Size = new System.Drawing.Size(70, 25);

        this.lblLongBreakMinUnit.AutoSize = true;
        this.lblLongBreakMinUnit.Location = new System.Drawing.Point(labelMinX, row3Y + textOffsetY);
        this.lblLongBreakMinUnit.Name = "lblLongBreakMinUnit";
        this.lblLongBreakMinUnit.Text = "分";

        this.nudLongBreakTimeSec.Location = new System.Drawing.Point(inputSecX, row3Y);
        this.nudLongBreakTimeSec.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
        this.nudLongBreakTimeSec.Name = "nudLongBreakTimeSec";
        this.nudLongBreakTimeSec.Size = new System.Drawing.Size(70, 25);

        this.lblLongBreakSecUnit.AutoSize = true;
        this.lblLongBreakSecUnit.Location = new System.Drawing.Point(labelSecX, row3Y + textOffsetY);
        this.lblLongBreakSecUnit.Name = "lblLongBreakSecUnit";
        this.lblLongBreakSecUnit.Text = "秒";

        // --- 第四行：提示1 ---
        this.lblWarningLongTime.AutoSize = true;
        this.lblWarningLongTime.Location = new System.Drawing.Point(labelX, row4Y + textOffsetY);
        this.lblWarningLongTime.Name = "lblWarningLongTime";
        this.lblWarningLongTime.Text = "提前长时间提示:";

        this.nudWarningLongTime.Location = new System.Drawing.Point(inputWarningX, row4Y);
        this.nudWarningLongTime.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
        this.nudWarningLongTime.Name = "nudWarningLongTime";
        this.nudWarningLongTime.Size = new System.Drawing.Size(70, 25);

        this.lblWarningLongTimeUnit.AutoSize = true;
        this.lblWarningLongTimeUnit.Location = new System.Drawing.Point(labelWarningX, row4Y + textOffsetY);
        this.lblWarningLongTimeUnit.Name = "lblWarningLongTimeUnit";
        this.lblWarningLongTimeUnit.Text = "秒";

        // --- 第五行：提示2 ---
        this.lblWarningShortTime.AutoSize = true;
        this.lblWarningShortTime.Location = new System.Drawing.Point(labelX, row5Y + textOffsetY);
        this.lblWarningShortTime.Name = "lblWarningShortTime";
        this.lblWarningShortTime.Text = "提前短时间提示:";

        this.nudWarningShortTime.Location = new System.Drawing.Point(inputWarningX, row5Y);
        this.nudWarningShortTime.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
        this.nudWarningShortTime.Name = "nudWarningShortTime";
        this.nudWarningShortTime.Size = new System.Drawing.Size(70, 25);

        this.lblWarningShortTimeUnit.AutoSize = true;
        this.lblWarningShortTimeUnit.Location = new System.Drawing.Point(labelWarningX, row5Y + textOffsetY);
        this.lblWarningShortTimeUnit.Name = "lblWarningShortTimeUnit";
        this.lblWarningShortTimeUnit.Text = "秒";

        // --- 按钮位置调整 (继承自 BaseForm) ---
        // 原来是 230，现在改为 265 (+35偏移)
        this.btnConfirm.Location = new System.Drawing.Point(147, 265);
        this.btnCancel.Location = new System.Drawing.Point(273, 265);

        // --- Form ---
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        // 高度增加到 330 以容纳所有内容和底部按钮
        this.ClientSize = new System.Drawing.Size(420, 330);
        this.Name = "PomodoroSettingsForm";
        this.Text = "番茄时钟设置";

        this.Controls.Add(this.lblWorkTime);
        this.Controls.Add(this.nudWorkTimeMin);
        this.Controls.Add(this.lblWorkMinUnit);
        this.Controls.Add(this.nudWorkTimeSec);
        this.Controls.Add(this.lblWorkSecUnit);

        this.Controls.Add(this.lblShortBreakTime);
        this.Controls.Add(this.nudShortBreakTimeMin);
        this.Controls.Add(this.lblShortBreakMinUnit);
        this.Controls.Add(this.nudShortBreakTimeSec);
        this.Controls.Add(this.lblShortBreakSecUnit);

        this.Controls.Add(this.lblLongBreakTime);
        this.Controls.Add(this.nudLongBreakTimeMin);
        this.Controls.Add(this.lblLongBreakMinUnit);
        this.Controls.Add(this.nudLongBreakTimeSec);
        this.Controls.Add(this.lblLongBreakSecUnit);

        this.Controls.Add(this.lblWarningLongTime);
        this.Controls.Add(this.nudWarningLongTime);
        this.Controls.Add(this.lblWarningLongTimeUnit);

        this.Controls.Add(this.lblWarningShortTime);
        this.Controls.Add(this.nudWarningShortTime);
        this.Controls.Add(this.lblWarningShortTimeUnit);

        this.Controls.SetChildIndex(this.btnConfirm, 0);
        this.Controls.SetChildIndex(this.btnCancel, 0);

        ((System.ComponentModel.ISupportInitialize)(this.nudWorkTimeMin)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudWorkTimeSec)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudShortBreakTimeMin)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudShortBreakTimeSec)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudLongBreakTimeMin)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudLongBreakTimeSec)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudWarningLongTime)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.nudWarningShortTime)).EndInit();

        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private ThemedLabel lblWorkTime;
    private ThemedLabel lblShortBreakTime;
    private ThemedLabel lblLongBreakTime;
    private ThemedLabel lblWorkMinUnit;
    private ThemedLabel lblShortBreakMinUnit;
    private ThemedLabel lblLongBreakMinUnit;
    private System.Windows.Forms.NumericUpDown nudWorkTimeMin;
    private System.Windows.Forms.NumericUpDown nudWorkTimeSec;
    private System.Windows.Forms.NumericUpDown nudShortBreakTimeMin;
    private System.Windows.Forms.NumericUpDown nudShortBreakTimeSec;
    private System.Windows.Forms.NumericUpDown nudLongBreakTimeMin;
    private System.Windows.Forms.NumericUpDown nudLongBreakTimeSec;
    private ThemedLabel lblWorkSecUnit;
    private ThemedLabel lblShortBreakSecUnit;
    private ThemedLabel lblLongBreakSecUnit;
    private ThemedLabel lblWarningLongTime;
    private ThemedLabel lblWarningShortTime;
    private ThemedLabel lblWarningLongTimeUnit;
    private ThemedLabel lblWarningShortTimeUnit;
    private System.Windows.Forms.NumericUpDown nudWarningLongTime;
    private System.Windows.Forms.NumericUpDown nudWarningShortTime;
}