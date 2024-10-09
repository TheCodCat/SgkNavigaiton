using System.Collections;
using UnityEngine;
using Unity.Notifications.Android;

public class Notification : MonoBehaviour
{
    private void Start()
    {
        var group = new AndroidNotificationChannelGroup()
        {
            Id = "Main",
            Name = "Main notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannelGroup(group);
        var channel = new AndroidNotificationChannel()
        {
            Id = "1",
            Name = "SgkNavigation",
            Importance = Importance.Default,
            Description = "Generic notifications",
            Group = "Main",  // must be same as Id of previously registered group
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        StartCoroutine(RequestNotificationPermission());
    }

    IEnumerator RequestNotificationPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;
        // here use request.Status to determine users response
    }

    public void SendNotification(string text)
    {
        var notification = new AndroidNotification();
        notification.Title = "Навигатор СГК";
        notification.Text = text;
        notification.FireTime = System.DateTime.Now;

        AndroidNotificationCenter.SendNotification(notification, "1");
    }
}
