using DiabloTwoMFTimer.UI.Components;
using DiabloTwoMFTimer.Utils;

namespace DiabloTwoMFTimer.UI.Timer
{
    partial class LootHistoryForm
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
            this.headerControl = new DiabloTwoMFTimer.UI.Components.ThemedWindowHeader();
            this.btnClose = new DiabloTwoMFTimer.UI.Components.ThemedModalButton();

            // 自定义日期面板
            this.pnlCustomDate = new System.Windows.Forms.Panel();
            this.lblFrom = new DiabloTwoMFTimer.UI.Components.ThemedLabel();
            this.dtpStart = new DiabloTwoMFTimer.UI.Components.ThemedDateTimePicker();
            this.lblTo = new DiabloTwoMFTimer.UI.Components.ThemedLabel();
            this.dtpEnd = new DiabloTwoMFTimer.UI.Components.ThemedDateTimePicker();
            this.btnSearch = new DiabloTwoMFTimer.UI.Components.ThemedButton();

            // 表格容器
            this.pnlGridContainer = new System.Windows.Forms.Panel();
            this.gridLoot = new DiabloTwoMFTimer.UI.Components.ThemedDataGridView();

            this.pnlCustomDate.SuspendLayout();
            this.pnlGridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLoot)).BeginInit();
            this.SuspendLayout();

            // 
            // headerControl
            // 
            this.headerControl.BackColor = System.Drawing.Color.Transparent;
            this.headerControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerControl.Location = new System.Drawing.Point(0, 0);
            this.headerControl.Name = "headerControl";
            this.headerControl.Size = new System.Drawing.Size(800, 100); // 这里的高度会被 SizeChanged 里的逻辑覆盖
            this.headerControl.TabIndex = 0;
            this.headerControl.Title = "LOOT HISTORY";

            // 
            // btnClose
            // 
            this.btnClose.Name = "btnClose";
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.SetThemeDanger();
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            // 
            // pnlCustomDate
            // 
            this.pnlCustomDate.BackColor = System.Drawing.Color.Transparent;
            this.pnlCustomDate.Controls.Add(this.lblFrom);
            this.pnlCustomDate.Controls.Add(this.dtpStart);
            this.pnlCustomDate.Controls.Add(this.lblTo);
            this.pnlCustomDate.Controls.Add(this.dtpEnd);
            this.pnlCustomDate.Controls.Add(this.btnSearch);
            // Location 和 Size 在 .cs 中动态计算
            this.pnlCustomDate.Name = "pnlCustomDate";
            this.pnlCustomDate.TabIndex = 1;
            // 【关键】默认 Visible=true，但 Opacity 或 ViewMode 控制显示，或者仅仅隐藏内部控件
            // 这里我们通过 ViewMode 控制 Panel 的 Visible，但 Grid 的位置计算会忽略这个 Visible
            this.pnlCustomDate.Visible = false;

            // 
            // Controls inside pnlCustomDate (Initial properties)
            //
            this.lblFrom.AutoSize = true;
            // this.lblFrom.Text = LanguageManager.GetString("From");
            this.lblFrom.Text = "";

            this.lblTo.AutoSize = true;
            this.lblTo.Text = "-";

            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);

            // 
            // pnlGridContainer
            // 
            this.pnlGridContainer.BackColor = System.Drawing.Color.Transparent;
            this.pnlGridContainer.Controls.Add(this.gridLoot);
            this.pnlGridContainer.Name = "pnlGridContainer";
            this.pnlGridContainer.TabIndex = 2;

            // 
            // gridLoot
            // 
            this.gridLoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLoot.Name = "gridLoot";
            this.gridLoot.TabIndex = 0;

            // 
            // LootHistoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlGridContainer);
            this.Controls.Add(this.pnlCustomDate);
            this.Controls.Add(this.headerControl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LootHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LootHistoryForm";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.pnlCustomDate.ResumeLayout(false);
            this.pnlCustomDate.PerformLayout();
            this.pnlGridContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLoot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ThemedWindowHeader headerControl;
        private ThemedModalButton btnClose;
        private System.Windows.Forms.Panel pnlCustomDate;
        private System.Windows.Forms.Panel pnlGridContainer;
        private ThemedDataGridView gridLoot;

        private ThemedLabel lblFrom;
        private ThemedDateTimePicker dtpStart;
        private ThemedLabel lblTo;
        private ThemedDateTimePicker dtpEnd;
        private ThemedButton btnSearch;
    }
}