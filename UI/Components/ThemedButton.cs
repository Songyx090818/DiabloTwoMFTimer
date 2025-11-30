using System.Windows.Forms;
using System.Drawing;
using DiabloTwoMFTimer.UI.Theme;
using System;

namespace DiabloTwoMFTimer.UI.Components
{
    public class ThemedButton : Button
    {
        public ThemedButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.Size = new Size(120, 40);
            this.Cursor = Cursors.Hand;
            this.Font = AppTheme.MainFont;

            // --- 颜色配置优化 ---

            // 1. 正常状态：深色背景，灰色边框（不要一开始就金色边框，太抢眼）
            this.BackColor = AppTheme.SurfaceColor;
            this.ForeColor = AppTheme.TextColor;
            this.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
            this.FlatAppearance.BorderSize = 1;

            // 2. 悬停状态：背景稍微变亮，文字变金，边框变金
            // 使用 Color.FromArgb 调整透明度来混合颜色，看起来更高级
            this.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 65);

            // 3. 按下状态：背景变深
            this.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);

            this.SetStyle(ControlStyles.Selectable, false);
        }

        // 重写鼠标事件来实现更细致的文字/边框颜色变化
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            // 悬停时：字变金，框变金
            this.ForeColor = AppTheme.AccentColor;
            this.FlatAppearance.BorderColor = AppTheme.AccentColor;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            // 离开时：恢复白字，灰框
            this.ForeColor = AppTheme.TextColor;
            this.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        }
    }
}