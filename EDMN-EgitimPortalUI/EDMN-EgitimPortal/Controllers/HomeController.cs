using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Configuration; // Ensure this is present
using EgitimPortalFinal.Models;

namespace EgitimPortalFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private string ApiBaseUrl => _configuration["ApiBaseUrl"]!;

        // GET: /Home/Index
        [HttpGet] // Explicitly mark as GET
        public IActionResult Index()
        {
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        public IActionResult Course()
        {
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View("../Course/Course");
        }

        public IActionResult Details(int id) 
        {
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            ViewBag.CourseId = id; 
            return View("../Course/Details");
        }

        public IActionResult Search() 
        {
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // ErrorViewModel might still be needed for the error page, or remove if not used in the View.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: /Home/About
        [HttpGet] // Explicitly mark as GET
        public IActionResult About()
        {
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        // --- Authentication Actions (Refactored for API calls with NO C# MODELS) ---

        // GET: /Home/Login (Displays the login form)
        [HttpGet] // Explicitly mark as GET
        public IActionResult Login()
        {
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        // POST: /Home/Login (Receives login form submission - handled by client-side JS)
        [HttpPost] // Explicitly mark as POST
        public IActionResult LoginSubmit() // Changed action name to avoid collision and clarify purpose
        {
            // This POST action will just serve the view (or redirect) after the client-side JS has handled the API call.
            // It's primarily a landing point for the form submission if you have a traditional form.
            // However, since we're using AJAX, this action might not even be hit by a real browser POST
            // if the JS handles everything. It can simply redirect or reload the view.
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View("Login"); // Return the Login view, or redirect after successful AJAX login
        }

        // GET: /Home/Register (Displays the registration form)
        [HttpGet] // Explicitly mark as GET
        public IActionResult Register()
        {
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        // POST: /Home/Register (Receives registration form submission - handled by client-side JS)
        [HttpPost] // Explicitly mark as POST
        public IActionResult RegisterSubmit() // Changed action name to avoid collision and clarify purpose
        {
            // Similar to Login, this POST action serves the view after client-side JS handles API call.
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View("Register"); // Return the Register view, or redirect after successful AJAX registration
        }

        // GET: /Home/AccessDenied
        [HttpGet] // Explicitly mark as GET
        public IActionResult AccessDenied()
        {
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        // GET: /Home/Logout (Triggered by a link/button, handled by client-side JS)
        [HttpGet] // Explicitly mark as GET
        public IActionResult Logout()
        {
            // Client-side JavaScript will handle removing the token from localStorage
            // and redirecting the user. This action just serves as a redirect target.
            return RedirectToAction("Login");
        }
    }
}