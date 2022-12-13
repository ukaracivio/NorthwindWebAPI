using Microsoft.AspNetCore.Mvc;
using NorthwindWebAPI.Models;
using System.Diagnostics;

namespace NorthwindWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindContext _context;
        public HomeController(ILogger<HomeController> logger,NorthwindContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        //[HttpGet]
        //public IActionResult Login()
        //{
        //    User _user = new User(); 


        //    return View(_user);
        //}

        //[HttpPost]
        //public IActionResult Login(User _user)
        //{
        //    // LINQ : Language Integrated Query
        //    var status = _context.Users.Where
        //        (m =>m.UserName == _user.UserName && m.UserPass == _user.UserPass).FirstOrDefault();

        //    if (status == null)
        //    {
        //        ViewBag.LoginStatus = 0;
        //        return RedirectToAction("Unsuccess", "Home");

        //    }
        //    else
        //    {
        //        return RedirectToAction("Success", "Home");
        //    }


        //    return View(_user);
        //}

        //public IActionResult Success()
        //{

        //    return View();
        //}
        //public IActionResult Unsuccess()
        //{

        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}