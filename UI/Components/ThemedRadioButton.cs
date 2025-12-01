using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DiabloTwoMFTimer.UI.Theme;

namespace DiabloTwoMFTimer.UI.Components;

public class ThemedRadioButton : RadioButton
{
    public ThemedRadioButton()
    {
        this.SetStyle(
            ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
            true
        );

        this.Cursor = Cursors.Hand;
        this.Font = AppTheme.MainFont;
        this.BackColor = AppTheme.BackColor;
        this.ForeColor = AppTheme.TextColor;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(this.BackColor);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

        // 1. 绘制外圆
        int circleSize = 16;
        // 计算圆形的垂直居中位置
        int yOffset = (this.Height - circleSize) / 2;
        var circleRect = new Rectangle(1, yOffset, circleSize, circleSize);

        using (var pen = new Pen(AppTheme.AccentColor))
        using (var brush = new SolidBrush(AppTheme.SurfaceColor))
        {
            g.FillEllipse(brush, circleRect);
            g.DrawEllipse(pen, circleRect);
        }

        // 2. 绘制选中点
        if (this.Checked)
        {
            var dotRect = new Rectangle(circleRect.X + 4, circleRect.Y + 4, circleSize - 8, circleSize - 8);
            using (var brush = new SolidBrush(AppTheme.AccentColor))
            {
                g.FillEllipse(brush, dotRect);
            }
        }

        // 3. 绘制文字 (精准垂直居中)
        using (var brush = new SolidBrush(this.ForeColor))
        {
            // 测量文字高度
            var textSize = g.MeasureString(this.Text, this.Font);
            // 计算文字的垂直居中 Y 坐标
            float textY = (this.Height - textSize.Height) / 2;

            // +1 是为了视觉上的微调，因为字体基线原因，数学居中往往看起来偏高
            g.DrawString(this.Text, this.Font, brush, circleSize + 8, textY + 1);
        }
    }
}
