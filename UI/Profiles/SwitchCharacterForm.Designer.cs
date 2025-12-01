namespace DiabloTwoMFTimer.UI.Profiles
{
    partial class SwitchCharacterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCharacters = new DiabloTwoMFTimer.UI.Components.ThemedLabel();
            this.lstCharacters = new DiabloTwoMFTimer.UI.Components.ThemedListBox(); // 使用新组件
            this.SuspendLayout();
            // 
            // lblCharacters
            // 
            this.lblCharacters.AutoSize = true;
            this.lblCharacters.Location = new System.Drawing.Point(30, 50); // 避开标题栏
            this.lblCharacters.Name = "lblCharacters";
            this.lblCharacters.Size = new System.Drawing.Size(90, 15);
            this.lblCharacters.TabIndex = 0;
            this.lblCharacters.Text = "Select Char:";
            // 
            // lstCharacters
            // 
            this.lstCharacters.FormattingEnabled = true;
            this.lstCharacters.ItemHeight = 24; // ThemedListBox 默认高度
            this.lstCharacters.Location = new System.Drawing.Point(30, 80);
            this.lstCharacters.Name = "lstCharacters";
            this.lstCharacters.Size = new System.Drawing.Size(390, 154);
            this.lstCharacters.TabIndex = 1;
            // 
            // SwitchCharacterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 350);
            this.Controls.Add(this.lstCharacters);
            this.Controls.Add(this.lblCharacters);
            this.Name = "SwitchCharacterForm";
            this.Text = "切换角色档案";

            this.Controls.SetChildIndex(this.lblCharacters, 0);
            this.Controls.SetChildIndex(this.lstCharacters, 0);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private DiabloTwoMFTimer.UI.Components.ThemedListBox lstCharacters;
        private DiabloTwoMFTimer.UI.Components.ThemedLabel lblCharacters;
    }
}