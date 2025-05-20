using CoreKit.EventHandler.Interfaces;

namespace Application.Services.Notification
{
    public class NotificationEventListener : IEventListener
    {
        public async Task OnSendEventAsync(dynamic payload, string process, string action)
        {
            await Task.Run(() => {
                Console.WriteLine("Notify Users");
            });
        }
    }
}
