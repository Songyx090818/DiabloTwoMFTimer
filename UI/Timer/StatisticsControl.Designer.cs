using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DiabloTwoMFTimer.UI.Components;

namespace DiabloTwoMFTimer.UI.Timer
{
    partial class StatisticsControl
    {
        private IContainer components = null;
        private Label lblRunCount;
        private Label lblFastestTime;
        private Label lblAverageTime;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                Utils.LanguageManager.OnLanguageChanged -= LanguageManager_OnLanguageChanged;
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblRunCount = new ThemedLabel();
            lblFastestTime = new ThemedLabel();
            lblAverageTime = new ThemedLabel();
            SuspendLayout();

            // 
            // lblRunCount
            // 
            // 1. 设置 Dock=Top 让它占满顶部宽度
            lblRunCount.Dock = DockStyle.Top;
            lblRunCount.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 2. 关闭 AutoSize 以便由 Dock 控制宽度
            lblRunCount.AutoSize = false;
            // 3. 设定固定高度
            lblRunCount.Height = 40;
            lblRunCount.Name = "lblRunCount";
            // 4. 核心：文字居中对齐
            lblRunCount.TextAlign = ContentAlignment.MiddleCenter;
            lblRunCount.Text = "--- Run count 0 ---";

            // 
            // lblFastestTime
            // 
            lblFastestTime.Dock = DockStyle.Top;
            lblFastestTime.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblFastestTime.AutoSize = false;
            lblFastestTime.Height = 30; // 稍微紧凑一点
            lblFastestTime.Name = "lblFastestTime";
            lblFastestTime.TextAlign = ContentAlignment.MiddleCenter;
            lblFastestTime.Text = "Fastest time: --:--:--.-";

            // 
            // lblAverageTime
            // 
            lblAverageTime.Dock = DockStyle.Top;
            lblAverageTime.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblAverageTime.AutoSize = false;
            lblAverageTime.Height = 30;
            lblAverageTime.Name = "lblAverageTime";
            lblAverageTime.TextAlign = ContentAlignment.MiddleCenter;
            lblAverageTime.Text = "Average time: --:--:--.-";

            // 
            // StatisticsControl
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;

            // 注意添加顺序：在 Dock=Top 模式下，后添加的控件会位于更底层（视觉上的下方）
            // 或者先添加的位于 Z-Order 顶层，Dock=Top 时会占据最顶部
            // 这里的顺序会导致：
            // 1. Add(lblAverageTime) -> 占据顶部
            // 2. Add(lblFastestTime) -> 插入到 Average 之上
            // 3. Add(lblRunCount)    -> 插入到 Fastest 之上
            // 最终视觉顺序：RunCount -> Fastest -> Average (符合预期)
            Controls.Add(lblAverageTime);
            Controls.Add(lblFastestTime);
            Controls.Add(lblRunCount);

            Margin = new Padding(4);
            Name = "StatisticsControl";
            // 稍微增加一点 Padding 让内容不贴边（可选）
            Padding = new Padding(0, 10, 0, 0);
            Size = new Size(419, 120);
            ResumeLayout(false);
        }
    }
}