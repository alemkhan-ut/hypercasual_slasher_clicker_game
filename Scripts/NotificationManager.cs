using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{
    void Start()
    {
        CreateNotificationChannel();
    }


    public void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default channel",
            Importance = Importance.High,
            Description = "Generic notifications"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    public void SendGetDailyRewardNotification(double fireTime)
    {
        var notification = new AndroidNotification();
        notification.Title = "Ежедневная награда!";
        notification.Text = "У Вас готова ежедневная награда. Не забудь её взять пока она не изчезла!";
        notification.LargeIcon = "notification1icon";
        notification.FireTime = System.DateTime.Now.AddSeconds(fireTime);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
}
