using asp_test.Models;
using asp_test.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace asp_test.Controllers
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
            var context = new asp_testContext();

            var comment = context.Comments.Find(1);
            var ddd = context.Comments.Join(context.Users, c => c.Userid, u => u.Id, (comments, users) => new CommentUserViewModel 
            {
                Id = c.id,

            })

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}