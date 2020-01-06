using System;
using System.Threading.Tasks;

namespace AppAppartamenti.Notify
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        //void Initialize();

        int ScheduleNotification(string title, string message, bool hasNotificationsPermission);

        void ReceiveNotification(string title, string message);
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}


namespace AppAppartamenti.NotificationSample
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
