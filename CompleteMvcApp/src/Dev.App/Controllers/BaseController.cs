using Dev.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dev.App.Controllers
{
    public class BaseController : Controller
    {
        private readonly INotifier _notifier;

        protected BaseController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected bool ValidOperation()
        {
            return !_notifier.HasNotification();
        }
    }
}
