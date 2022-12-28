using Dev.Business.Notifications;

namespace Dev.Business.Interfaces
{
    public interface INotifier
    {
        bool HasNotification();

        List<Notification> GetNotifications();

        void Handle(Notification notification);
    }
}
