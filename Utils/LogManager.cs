using System;
using System.IO;

namespace DTwoMFTimerHelper.Utils
{
    /// <summary>
    /// 日志管理类，提供统一的日志记录功能
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// 写入调试日志
        /// </summary>
        /// <param name="className">调用类名</param>
        /// <param name="message">日志消息</param>
        public static void WriteDebugLog(string className, string message)
        {
            try
            {
                string debugLogPath = Path.Combine(Environment.CurrentDirectory, "debug_log.txt");
                using (StreamWriter writer = new StreamWriter(debugLogPath, true))
                {
                    writer.WriteLine($"[{DateTime.Now}] [{className}] {message}");
                }
            }
            catch (Exception logEx)
            {
                // 避免日志系统本身的异常影响主流程
                Console.WriteLine($"写入日志失败: {logEx.Message}");
            }
        }
    }
}