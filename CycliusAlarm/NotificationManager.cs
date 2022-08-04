using System;
using Microsoft.Toolkit.Uwp.Notifications;

namespace CycliusAlarm;

public class NotificationManager
{
    public static ToastContentBuilder CreateNotification(string title, string body)
    {
        if (IsUWPSupported()) throw new NotSupportedException($"Current running os distribute dose not support notifications! Your OS: {Environment.OSVersion.Platform}-{Environment.OSVersion.Version.Major}");
        return new ToastContentBuilder().AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
            .AddText(title)
            .AddText(body);
    }

    public static void PushNotification(string title, string body)
    {
        CreateNotification(title, body).Show();
    }

    public static bool IsUWPSupported()
    {
        return (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 10)
               || Environment.OSVersion.Platform == PlatformID.Xbox;
    }
}