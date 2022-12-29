using Dev.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dev.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("error/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelError = new ErrorViewModel();

            if (id == 500)
            {
                modelError.Message = "An error has occurred! Try again later on or contact our Support Team.";
                modelError.Title = "An error has occurred!";
                modelError.ErrorCode = id;
            }
            else if (id == 404)
            {
                modelError.Message = "The page does not exist! <br />If you have any questions or concerns contact our Support Team.";
                modelError.Title = "Ops! Page not found.";
                modelError.ErrorCode = id;
            }
            else if (id == 403)
            {
                modelError.Message = "You don't have access to perform this action.";
                modelError.Title = "Forbidden";
                modelError.ErrorCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelError);
        }
    }
}