using System;
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

    public static void Show(string message, ToastType type = ToastType.Info)
    {
        // ---------------------------------------------------------
        // 【核心修复】线程安全检查
        // ---------------------------------------------------------
        // 检查当前是否有打开的主窗体，用来判断和获取 UI 线程
        if (Application.OpenForms.Count > 0)
        {
            var mainForm = Application.OpenForms[0];

            // 如果当前调用线程不是 UI 线程 (InvokeRequired = true)
            // 我们必须将操作“封送 (Marshal)”回 UI 线程执行
            if (mainForm != null && mainForm.InvokeRequired && !mainForm.IsDisposed)
            {
                // 使用 BeginInvoke 异步执行，避免阻塞后台计时器线程
                mainForm.BeginInvoke(new Action(() => Show(message, type)));
                return;
            }
        }
        // ---------------------------------------------------------

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
            int startY = workingArea.Y + (int)(workingArea.Height * 0.15);

            // 简单的堆叠逻辑：如果有多个提示，向下偏移
            // 注意：这里需要清理已关闭的 toast，防止无限下移
            _openToasts.RemoveAll(t => t.IsDisposed);

            int offset = _openToasts.Count * (toast.Height + 10);
            toast.Location = new Point(x, startY + offset);
        }

        toast.FormClosed += (s, e) =>
        {
            _openToasts.Remove(toast);
        };
        _openToasts.Add(toast);

        // 此时在 UI 线程，Show() 能正常建立消息循环连接
        toast.Show();
    }

    public static void Success(string msg) => Show(msg, ToastType.Success);

    public static void Error(string msg) => Show(msg, ToastType.Error);

    public static void Info(string msg) => Show(msg, ToastType.Info);

    public static void Warning(string msg) => Show(msg, ToastType.Warning);
}