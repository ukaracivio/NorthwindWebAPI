using Microsoft.AspNetCore.Mvc;
using NorthwindWebAPI.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NorthwindWebAPI.ViewModels;
using Scrypt;

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

        [HttpPost]
        public IActionResult Register(RegisterVM registerVM)
        {
            // Şifreleme yaparak kullanıcıyı register etme.
            // Kullanılacak Paket Scrypt

            ScryptEncoder encoder = new ScryptEncoder();

            var result = _context.Customers
                        .Where(c => c.CustomerId == registerVM.CustomerID)
                        .SingleOrDefault();

            if (result == null)
            {
                ViewBag.Message = "Customer Id bulunamadı";
                return View();
            }

            if (result.UserName != null)
            {
                ViewBag.Message = "Bu Customer Id zaten kayıtlı...";
            }

            var checkname = _context.Customers
                        .Where(c => c.UserName == registerVM.UserName)
                        .SingleOrDefault();

            if (checkname != null)
            {
                ViewBag.Message = "Bu kullanıcı adı (UserName) zaten kayıtlı...";
                return View();

            }


            result.UserName = registerVM.UserName;
            result.UserPass = encoder.Encode(registerVM.UserPass); // view ekranından verilen şifreyi şifreliyor.

            _context.SaveChanges();

            ViewBag.Message = "Kayıt işlemi başarılı....Sisteme giriş yapabilirsiniz...";

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            ScryptEncoder encoder = new ScryptEncoder();

            var validCustomer = (from c in _context.Customers
                                 where c.UserName.Equals(loginVM.UserName)
                                 select c).SingleOrDefault();

            if (validCustomer == null) 
            {
                ViewBag.Message = " Geçersiz kullanıcı adı veya şifre.......";
                return View();
            }

            bool isValidCustomer = encoder.Compare(loginVM.UserPass, validCustomer.UserPass);

            if (isValidCustomer) 
            {
                ViewBag.Message = " Kullanıcı girişi onaylandı...Ana sayfaya dönebilirsiniz....";
                return View();
            }
            else
            {
                ViewBag.Message = " Geçersiz kullanıcı adı veya şifre.......";
                return View();
            }




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