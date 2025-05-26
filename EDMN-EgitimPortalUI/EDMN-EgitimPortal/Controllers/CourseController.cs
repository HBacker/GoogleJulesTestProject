using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

public class CourseController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly INotyfService _notify;
    private readonly IWebHostEnvironment _env;

    public CourseController(IConfiguration configuration, INotyfService notify, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _notify = notify;
        _env = env;
    }

    private string ApiBaseUrl => _configuration["ApiBaseUrl"]!;

    public IActionResult Index(string searchString)
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.SearchString = searchString;
        return View();
    }

    public IActionResult Index()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        return View();
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.CourseId = id.Value;
        return View();
    }

    public IActionResult Course(string searchString)
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.SearchString = searchString;
        return View();
    }

    public IActionResult Wordpress()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.Tag = "Wordpress";
        return View("Course");
    }

    public IActionResult Programlama()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.Tag = "Programlama";
        return View("Course");
    }

    public IActionResult SEO()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.Tag = "SEO";
        return View("Course");
    }

    public IActionResult Search(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            _notify.Warning("Lütfen bir anahtar kelime girin.");
            return RedirectToAction("Course");
        }
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.Keyword = keyword;
        return View("Course");
    }

    [HttpGet]
    [Route("Course/Details/{id:int}/{lessonId?}")]
    public IActionResult Details(int id, int? lessonId = null)
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.CourseId = id;
        ViewBag.CurrentLessonId = lessonId;
        return View();
    }

    [Route("[controller]/v1/ListCourses")]
    [HttpGet]
    public IActionResult GetCourses([FromQuery] string? tag)
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.Tag = tag;
        return View("Course");
    }

    [HttpPost("/PhotoUpload/Course")]
    public async Task<IActionResult> UploadCoursePhoto(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Dosya seçilmedi.");

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "courses");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { fileName = uniqueName });
    }
}
