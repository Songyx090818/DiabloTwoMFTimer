namespace DTwoMFTimerHelper.UI.Pomodoro {
    partial class PomodoroControl {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.btnPomodoroReset = new System.Windows.Forms.Button();
            this.btnStartPomodoro = new System.Windows.Forms.Button();
            this.btnPomodoroSettings = new System.Windows.Forms.Button();
            this.lblPomodoroCount = new System.Windows.Forms.Label();
            // 实例化我们自定义的 Label
            this.lblPomodoroTime = new PomodoroTimeDisplayLabel();

            this.SuspendLayout();
            // 
            // btnPomodoroReset
            // 
            this.btnPomodoroReset.Location = new System.Drawing.Point(28, 376);
            this.btnPomodoroReset.Name = "btnPomodoroReset";
            this.btnPomodoroReset.Size = new System.Drawing.Size(205, 75);
            this.btnPomodoroReset.TabIndex = 4;
            this.btnPomodoroReset.Text = "重置";
            this.btnPomodoroReset.UseVisualStyleBackColor = true;
            this.btnPomodoroReset.Click += new System.EventHandler(this.BtnPomodoroReset_Click);
            // 
            // btnStartPomodoro
            // 
            this.btnStartPomodoro.Location = new System.Drawing.Point(28, 168);
            this.btnStartPomodoro.Name = "btnStartPomodoro";
            this.btnStartPomodoro.Size = new System.Drawing.Size(205, 75);
            this.btnStartPomodoro.TabIndex = 2;
            this.btnStartPomodoro.Text = "开始";
            this.btnStartPomodoro.UseVisualStyleBackColor = true;
            this.btnStartPomodoro.Click += new System.EventHandler(this.BtnStartPomodoro_Click);
            // 
            // btnPomodoroSettings
            // 
            this.btnPomodoroSettings.Location = new System.Drawing.Point(28, 277);
            this.btnPomodoroSettings.Name = "btnPomodoroSettings";
            this.btnPomodoroSettings.Size = new System.Drawing.Size(205, 75);
            this.btnPomodoroSettings.TabIndex = 3;
            this.btnPomodoroSettings.Text = "设置";
            this.btnPomodoroSettings.UseVisualStyleBackColor = true;
            this.btnPomodoroSettings.Click += new System.EventHandler(this.BtnPomodoroSettings_Click);
            // 
            // lblPomodoroCount
            // 
            this.lblPomodoroCount.AutoSize = true;
            this.lblPomodoroCount.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblPomodoroCount.Location = new System.Drawing.Point(69, 102);
            this.lblPomodoroCount.Name = "lblPomodoroCount";
            this.lblPomodoroCount.Size = new System.Drawing.Size(124, 31);
            this.lblPomodoroCount.TabIndex = 1;
            this.lblPomodoroCount.Text = "0个大番茄";
            // 
            // lblPomodoroTime (自定义组件)
            // 
            this.lblPomodoroTime.AutoSize = true;
            this.lblPomodoroTime.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblPomodoroTime.Location = new System.Drawing.Point(40, 32);
            this.lblPomodoroTime.Name = "lblPomodoroTime";
            this.lblPomodoroTime.Size = new System.Drawing.Size(203, 50);
            this.lblPomodoroTime.TabIndex = 0;
            this.lblPomodoroTime.Text = "25:00:00:0";
            // 
            // PomodoroControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblPomodoroCount);
            this.Controls.Add(this.btnPomodoroSettings);
            this.Controls.Add(this.btnPomodoroReset);
            this.Controls.Add(this.btnStartPomodoro);
            this.Controls.Add(this.lblPomodoroTime);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "PomodoroControl";
            this.Size = new System.Drawing.Size(549, 562);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnPomodoroReset;
        private System.Windows.Forms.Button btnStartPomodoro;
        private System.Windows.Forms.Button btnPomodoroSettings;
        private System.Windows.Forms.Label lblPomodoroCount;
        // 关键：声明类型为我们自定义的 PomodoroTimeDisplayLabel
        private PomodoroTimeDisplayLabel lblPomodoroTime;
    }
}