namespace DiabloTwoMFTimer.UI.Profiles
{
    partial class CreateCharacterForm
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
            // 使用 Themed 控件
            this.lblCharacterName = new DiabloTwoMFTimer.UI.Components.ThemedLabel();
            this.txtCharacterName = new DiabloTwoMFTimer.UI.Components.ThemedTextBox();
            this.lblCharacterClass = new DiabloTwoMFTimer.UI.Components.ThemedLabel();
            this.cmbCharacterClass = new DiabloTwoMFTimer.UI.Components.ThemedComboBox();
            this.SuspendLayout();

            // 
            // lblCharacterName
            // 
            this.lblCharacterName.AutoSize = true;
            this.lblCharacterName.Location = new System.Drawing.Point(50, 70); // Y坐标稍微下移以适应标题栏
            this.lblCharacterName.Name = "lblCharacterName";
            this.lblCharacterName.Size = new System.Drawing.Size(60, 15);
            this.lblCharacterName.TabIndex = 0;
            this.lblCharacterName.Text = "Name:";

            // 
            // txtCharacterName
            // 
            this.txtCharacterName.Location = new System.Drawing.Point(150, 67);
            this.txtCharacterName.Name = "txtCharacterName";
            this.txtCharacterName.Size = new System.Drawing.Size(180, 25);
            this.txtCharacterName.TabIndex = 1;

            // 
            // lblCharacterClass
            // 
            this.lblCharacterClass.AutoSize = true;
            this.lblCharacterClass.Location = new System.Drawing.Point(50, 113);
            this.lblCharacterClass.Name = "lblCharacterClass";
            this.lblCharacterClass.Size = new System.Drawing.Size(60, 15);
            this.lblCharacterClass.TabIndex = 2;
            this.lblCharacterClass.Text = "Class:";

            // 
            // cmbCharacterClass
            // 
            this.cmbCharacterClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCharacterClass.FormattingEnabled = true;
            this.cmbCharacterClass.Location = new System.Drawing.Point(150, 110);
            this.cmbCharacterClass.Name = "cmbCharacterClass";
            this.cmbCharacterClass.Size = new System.Drawing.Size(180, 23);
            this.cmbCharacterClass.TabIndex = 3;

            // 
            // CreateCharacterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 300);
            this.Controls.Add(this.cmbCharacterClass);
            this.Controls.Add(this.lblCharacterClass);
            this.Controls.Add(this.txtCharacterName);
            this.Controls.Add(this.lblCharacterName);
            this.Name = "CreateCharacterForm";
            this.Text = "创建角色档案";

            // 重要：保留基类的控件
            this.Controls.SetChildIndex(this.lblCharacterName, 0);
            this.Controls.SetChildIndex(this.txtCharacterName, 0);
            this.Controls.SetChildIndex(this.lblCharacterClass, 0);
            this.Controls.SetChildIndex(this.cmbCharacterClass, 0);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private DiabloTwoMFTimer.UI.Components.ThemedLabel lblCharacterName;
        private DiabloTwoMFTimer.UI.Components.ThemedTextBox txtCharacterName;
        private DiabloTwoMFTimer.UI.Components.ThemedLabel lblCharacterClass;
        private DiabloTwoMFTimer.UI.Components.ThemedComboBox cmbCharacterClass;
    }
}