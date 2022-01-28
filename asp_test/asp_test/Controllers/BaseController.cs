using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using asp_test.Models.Data;
using asp_test.Models;

namespace asp_test.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;
        protected readonly asp_testContext _context;
        public BaseController(asp_testContext context, ILogger<BaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// エラー発生時に処理する共通クラス
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>Error.cshtml</returns>
        protected virtual IActionResult Error(Exception ex)
        {
            // Logを残す
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);

            return RedirectToAction("Error", "Home");
        }
    }
}
