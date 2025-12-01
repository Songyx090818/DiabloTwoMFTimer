using System.Drawing;
using System.Windows.Forms;
using DiabloTwoMFTimer.UI.Theme;

namespace DiabloTwoMFTimer.UI.Components;

public class ThemedGroupBox : GroupBox
{
    public ThemedGroupBox()
    {
        this.DoubleBuffered = true;
        // 关键修复：不要用 Transparent，改用实色
        this.BackColor = AppTheme.BackColor;
        this.ForeColor = AppTheme.TextColor;
        this.Font = AppTheme.MainFont;

        // 启用重绘样式
        this.SetStyle(
            ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
            true
        );
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        // 关键修复：清空画布
        g.Clear(this.BackColor);

        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

        // 1. 测量文字大小
        var textSize = g.MeasureString(this.Text, this.Font);
        var textRect = new Rectangle(10, 0, (int)textSize.Width, (int)textSize.Height);

        // 2. 定义边框区域 (避开文字)
        var borderRect = this.ClientRectangle;
        borderRect.Y += (int)(textSize.Height / 2);
        borderRect.Height -= (int)(textSize.Height / 2);
        borderRect.Width -= 1;
        borderRect.Height -= 1;

        // 3. 绘制边框
        using (var pen = new Pen(AppTheme.BorderColor))
        {
            // 上边框分两段画，中间留空给文字
            g.DrawLine(pen, borderRect.Left, borderRect.Top, textRect.Left - 2, borderRect.Top);
            g.DrawLine(pen, textRect.Right + 2, borderRect.Top, borderRect.Right, borderRect.Top);

            g.DrawLine(pen, borderRect.Left, borderRect.Top, borderRect.Left, borderRect.Bottom); // 左
            g.DrawLine(pen, borderRect.Right, borderRect.Top, borderRect.Right, borderRect.Bottom); // 右
            g.DrawLine(pen, borderRect.Left, borderRect.Bottom, borderRect.Right, borderRect.Bottom); // 底
        }

        // 4. 绘制文字
        using (var brush = new SolidBrush(AppTheme.AccentColor))
        {
            g.DrawString(this.Text, this.Font, brush, textRect.X, 0);
        }
    }
}
