using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DiabloTwoMFTimer.Models;
using DiabloTwoMFTimer.UI;
using DiabloTwoMFTimer.UI.Form;

namespace DiabloTwoMFTimer.Utils;

public static class Toast
{
    private static readonly List<ToastForm> _openToasts = [];

    // 移除 title 参数
    public static void Show(string message, ToastType type = ToastType.Info)
    {
        var toast = new ToastForm(message, type);

        var screen = Screen.PrimaryScreen;
        if (screen == null)
        {
            toast.Location = new Point(100, 100);
        }
        else
        {
            var workingArea = screen.WorkingArea;

            // 重新居中计算
            int x = workingArea.X + (workingArea.Width - toast.Width) / 2;
            int startY = workingArea.Y + (int)(workingArea.Height * 0.15); // 稍微往上提一点，0.2 -> 0.15

            int offset = _openToasts.Count * (toast.Height + 10);

            toast.Location = new Point(x, startY + offset);
        }

        toast.FormClosed += (s, e) =>
        {
            _openToasts.Remove(toast);
        };
        _openToasts.Add(toast);

        toast.Show();
    }

    // 更新所有辅助方法，移除 title 参数
    public static void Success(string msg) => Show(msg, ToastType.Success);

    public static void Error(string msg) => Show(msg, ToastType.Error);

    public static void Info(string msg) => Show(msg, ToastType.Info);

    public static void Warning(string msg) => Show(msg, ToastType.Warning);
}