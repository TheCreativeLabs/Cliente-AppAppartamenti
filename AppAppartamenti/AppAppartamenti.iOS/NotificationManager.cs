
using System;
using AppAppartamenti.Notify;
using Foundation;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppAppartamenti.iOS.iOSNotificationManager))]
namespace AppAppartamenti.iOS
{
    
        public class iOSNotificationManager : INotificationManager
        {
            int messageId = -1;

            //bool hasNotificationsPermission;

            public event EventHandler NotificationReceived;

            //public void Initialize()
            //{
            //    UNUserNotificationCenter center = UNUserNotificationCenter.Current;

            //    center.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge, (bool arg1, NSError arg2) =>
            //    {
            //        hasNotificationsPermission = arg1;
            //    });
            //}

            public int ScheduleNotification(string title, string message, bool hasNotificationsPermission)
            {
                // EARLY OUT: app doesn't have permissions
                if (!hasNotificationsPermission)
                {
                    return -1;
                }

                messageId++;

                var content = new UNMutableNotificationContent()
                {
                    Title = title,
                    Subtitle = "",
                    Body = message,
                    Badge = 1
                };

                // Local notifications can be time or location based
                // Create a time-based trigger, interval is in seconds and must be greater than 0
                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(20, false);

                var request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);
                UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
                {
                    if (err != null)
                    {
                        throw new Exception($"Failed to schedule notification: {err}");
                    }
                });

                return messageId;
            }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message
            };
            NotificationReceived?.Invoke(null, args);
        }
    }

    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            DependencyService.Get<INotificationManager>().ReceiveNotification(notification.Request.Content.Title, notification.Request.Content.Body);

            // alerts are always shown for demonstration but this can be set to "None"
            // to avoid showing alerts if the app is in the foreground
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}
