namespace DiabloTwoMFTimer.UI.Settings;
partial class GeneralSettingsControl
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
        groupBoxPosition = new DiabloTwoMFTimer.UI.Components.ThemedGroupBox();
        radioTopLeft = new DiabloTwoMFTimer.UI.Components.ThemedRadioButton();
        radioTopCenter = new DiabloTwoMFTimer.UI.Components.ThemedRadioButton();
        radioTopRight = new DiabloTwoMFTimer.UI.Components.ThemedRadioButton();
        radioBottomLeft = new DiabloTwoMFTimer.UI.Components.ThemedRadioButton();
        radioBottomCenter = new DiabloTwoMFTimer.UI.Components.ThemedRadioButton();
        radioBottomRight = new DiabloTwoMFTimer.UI.Components.ThemedRadioButton();
        groupBoxLanguage = new DiabloTwoMFTimer.UI.Components.ThemedGroupBox();
        chineseRadioButton = new DiabloTwoMFTimer.UI.Components.ThemedRadioButton();
        englishRadioButton = new DiabloTwoMFTimer.UI.Components.ThemedRadioButton();
        alwaysOnTopCheckBox = new DiabloTwoMFTimer.UI.Components.ThemedCheckBox();

        this.BackColor = DiabloTwoMFTimer.UI.Theme.AppTheme.BackColor;
        groupBoxPosition.SuspendLayout();
        groupBoxLanguage.SuspendLayout();
        SuspendLayout();
        // 
        // groupBoxPosition
        // 
        groupBoxPosition.Controls.Add(radioTopLeft);
        groupBoxPosition.Controls.Add(radioTopCenter);
        groupBoxPosition.Controls.Add(radioTopRight);
        groupBoxPosition.Controls.Add(radioBottomLeft);
        groupBoxPosition.Controls.Add(radioBottomCenter);
        groupBoxPosition.Controls.Add(radioBottomRight);
        groupBoxPosition.Location = new System.Drawing.Point(10, 10);
        groupBoxPosition.Name = "groupBoxPosition";
        groupBoxPosition.Size = new System.Drawing.Size(320, 130); // 增加高度
        groupBoxPosition.TabIndex = 0;
        groupBoxPosition.TabStop = false;
        groupBoxPosition.Text = "窗口位置";
        // 
        // radioTopLeft
        // 
        radioTopLeft.AutoSize = true;
        radioTopLeft.Checked = true;
        radioTopLeft.Location = new System.Drawing.Point(20, 35); // Y=35
        radioTopLeft.Name = "radioTopLeft";
        radioTopLeft.Size = new System.Drawing.Size(79, 32);
        radioTopLeft.TabIndex = 0;
        radioTopLeft.TabStop = true;
        radioTopLeft.Text = "左上";
        // 
        // radioTopCenter
        // 
        radioTopCenter.AutoSize = true;
        radioTopCenter.Location = new System.Drawing.Point(120, 35);
        radioTopCenter.Name = "radioTopCenter";
        radioTopCenter.Size = new System.Drawing.Size(79, 32);
        radioTopCenter.TabIndex = 1;
        radioTopCenter.Text = "上中";
        // 
        // radioTopRight
        // 
        radioTopRight.AutoSize = true;
        radioTopRight.Location = new System.Drawing.Point(220, 35);
        radioTopRight.Name = "radioTopRight";
        radioTopRight.Size = new System.Drawing.Size(79, 32);
        radioTopRight.TabIndex = 2;
        radioTopRight.Text = "右上";
        // 
        // radioBottomLeft
        // 
        radioBottomLeft.AutoSize = true;
        radioBottomLeft.Location = new System.Drawing.Point(20, 80); // Y=80 (间距增大)
        radioBottomLeft.Name = "radioBottomLeft";
        radioBottomLeft.Size = new System.Drawing.Size(79, 32);
        radioBottomLeft.TabIndex = 3;
        radioBottomLeft.Text = "左下";
        // 
        // radioBottomCenter
        // 
        radioBottomCenter.AutoSize = true;
        radioBottomCenter.Location = new System.Drawing.Point(120, 80);
        radioBottomCenter.Name = "radioBottomCenter";
        radioBottomCenter.Size = new System.Drawing.Size(79, 32);
        radioBottomCenter.TabIndex = 4;
        radioBottomCenter.Text = "下中";
        // 
        // radioBottomRight
        // 
        radioBottomRight.AutoSize = true;
        radioBottomRight.Location = new System.Drawing.Point(220, 80);
        radioBottomRight.Name = "radioBottomRight";
        radioBottomRight.Size = new System.Drawing.Size(79, 32);
        radioBottomRight.TabIndex = 5;
        radioBottomRight.Text = "右下";
        // 
        // groupBoxLanguage
        // 
        groupBoxLanguage.Controls.Add(chineseRadioButton);
        groupBoxLanguage.Controls.Add(englishRadioButton);
        groupBoxLanguage.Location = new System.Drawing.Point(10, 155); // 下移
        groupBoxLanguage.Name = "groupBoxLanguage";
        groupBoxLanguage.Size = new System.Drawing.Size(320, 85); // 增加高度
        groupBoxLanguage.TabIndex = 1;
        groupBoxLanguage.TabStop = false;
        groupBoxLanguage.Text = "语言";
        // 
        // chineseRadioButton
        // 
        chineseRadioButton.AutoSize = true;
        chineseRadioButton.Checked = true;
        chineseRadioButton.Location = new System.Drawing.Point(20, 35);
        chineseRadioButton.Name = "chineseRadioButton";
        chineseRadioButton.Size = new System.Drawing.Size(117, 32);
        chineseRadioButton.TabIndex = 0;
        chineseRadioButton.TabStop = true;
        chineseRadioButton.Text = "Chinese";
        // 
        // englishRadioButton
        // 
        englishRadioButton.AutoSize = true;
        englishRadioButton.Location = new System.Drawing.Point(160, 35);
        englishRadioButton.Name = "englishRadioButton";
        englishRadioButton.Size = new System.Drawing.Size(110, 32);
        englishRadioButton.TabIndex = 1;
        englishRadioButton.Text = "English";
        // 
        // alwaysOnTopCheckBox
        // 
        alwaysOnTopCheckBox.AutoSize = true;
        alwaysOnTopCheckBox.Checked = true;
        alwaysOnTopCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
        alwaysOnTopCheckBox.Location = new System.Drawing.Point(20, 260); // 下移
        alwaysOnTopCheckBox.Name = "alwaysOnTopCheckBox";
        alwaysOnTopCheckBox.Size = new System.Drawing.Size(120, 30);
        alwaysOnTopCheckBox.TabIndex = 3;
        alwaysOnTopCheckBox.Text = "总在最前";
        // 
        // GeneralSettingsControl
        // 
        AutoScroll = true;
        Controls.Add(groupBoxPosition);
        Controls.Add(groupBoxLanguage);
        Controls.Add(alwaysOnTopCheckBox);
        Name = "GeneralSettingsControl";
        Size = new System.Drawing.Size(350, 320);
        groupBoxPosition.ResumeLayout(false);
        groupBoxPosition.PerformLayout();
        groupBoxLanguage.ResumeLayout(false);
        groupBoxLanguage.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DiabloTwoMFTimer.UI.Components.ThemedGroupBox groupBoxPosition;
    private DiabloTwoMFTimer.UI.Components.ThemedGroupBox groupBoxLanguage;
    private DiabloTwoMFTimer.UI.Components.ThemedRadioButton radioTopLeft;
    private DiabloTwoMFTimer.UI.Components.ThemedRadioButton radioTopCenter;
    private DiabloTwoMFTimer.UI.Components.ThemedRadioButton radioTopRight;
    private DiabloTwoMFTimer.UI.Components.ThemedRadioButton radioBottomLeft;
    private DiabloTwoMFTimer.UI.Components.ThemedRadioButton radioBottomCenter;
    private DiabloTwoMFTimer.UI.Components.ThemedRadioButton radioBottomRight;
    private DiabloTwoMFTimer.UI.Components.ThemedRadioButton chineseRadioButton;
    private DiabloTwoMFTimer.UI.Components.ThemedRadioButton englishRadioButton;
    private DiabloTwoMFTimer.UI.Components.ThemedCheckBox alwaysOnTopCheckBox;
}