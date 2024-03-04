using Microsoft.AspNetCore.Mvc;

namespace Sprint1HEM.Controllers
{
    public class PaymentGateway : Controller
    {
        public IActionResult Index()
        {
            return View("PaymentGateway");
        }
    }
}
