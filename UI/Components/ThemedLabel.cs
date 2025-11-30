using System.Windows.Forms;
using System.Drawing;
using DiabloTwoMFTimer.UI.Theme;

namespace DiabloTwoMFTimer.UI.Components;

public class ThemedLabel : Label
{
    public bool IsTitle { get; set; } = false;

    public ThemedLabel()
    {
        this.ForeColor = AppTheme.TextColor;
        this.Font = AppTheme.MainFont;
        this.AutoSize = true;

        // 关键修改：不要默认透明，这会降低文字清晰度。
        // 如果你的 Label 是放在 Panel 上的，WinForms 会自动处理背景继承。
        this.BackColor = Color.Transparent;

        // 开启双缓冲优化渲染
        this.DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        this.Font = IsTitle ? AppTheme.TitleFont : AppTheme.MainFont;

        // 提升文字渲染质量
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        base.OnPaint(e);
    }
}