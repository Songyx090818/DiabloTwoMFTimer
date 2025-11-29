namespace DiabloTwoMFTimer.Models;
// 这是一个空消息，仅作为信号
public class LanguageChangedMessage
{
    public string LanguageCode { get; }
    public LanguageChangedMessage(string code) => LanguageCode = code;
}