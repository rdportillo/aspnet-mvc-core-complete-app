using Dev.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dev.App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotifier _notifier;

        public SummaryViewComponent(INotifier notifier)
        {
            _notifier = notifier;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await Task.FromResult(_notifier.GetNotifications());
            notifications.ForEach(n => ViewData.ModelState.AddModelError(string.Empty, n.Message));

            return View();
        }
    }
}
