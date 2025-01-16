using Microsoft.AspNetCore.Mvc;

namespace MVCBanXeDap.Controllers
{
    public class DashboardController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        public DashboardController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
