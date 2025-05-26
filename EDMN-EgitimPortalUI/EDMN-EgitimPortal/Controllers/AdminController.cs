using AspNetCoreHero.ToastNotification.Abstractions;
using EgitimPortalFinal.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

public class AdminController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly INotyfService _notify;
    private readonly IHubContext<GeneralHub> _hubContext;

    public AdminController(IConfiguration configuration, INotyfService notify, IHubContext<GeneralHub> hubContext)
    {
        _configuration = configuration;
        _notify = notify;
        _hubContext = hubContext;
    }

    private string ApiBaseUrl => _configuration["ApiBaseUrl"]!;

    public IActionResult List()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        _notify.Information("Kurs listesi yüklendi.");
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        _notify.Information("Yeni kurs oluşturma sayfasına yönlendirildiniz.");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IFormCollection formData)
    {
        try
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(ApiBaseUrl);

            var courseData = new Dictionary<string, string>();
            foreach (var key in formData.Keys)
            {
                courseData.Add(key, formData[key]);
            }

            var response = await client.PostAsJsonAsync("courses", courseData);

            if (response.IsSuccessStatusCode)
            {
                _notify.Success("Kurs başarıyla eklendi.");
                return RedirectToAction("List");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _notify.Error($"Kurs eklenemedi: {error}");
                ViewBag.ApiBaseUrl = ApiBaseUrl;
                return View();
            }
        }
        catch (Exception ex)
        {
            _notify.Error($"Kurs eklenirken hata oluştu: {ex.Message}");
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }
    }

    public IActionResult Edit(int id)
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.CourseId = id;
        _notify.Information($"Kurs düzenleme sayfası açıldı (ID: {id}).");
        return View();
    }

    public IActionResult Delete(int id)
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.CourseId = id;
        _notify.Warning($"Kurs silme sayfasına yönlendirildiniz (ID: {id}).");
        return View();
    }

    public IActionResult GetUserList()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        _notify.Information("Kullanıcı listesi yüklendi.");
        return View();
    }

    public IActionResult RoleAdd()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        _notify.Information("Rol ekleme sayfası açıldı.");
        return View();
    }

    public IActionResult ManageUserRoles()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        _notify.Information("Kullanıcı rol yönetimi sayfası yüklendi.");
        return View();
    }

    public IActionResult CreateLesson()
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        _notify.Information("Yeni ders oluşturma sayfasına yönlendirildiniz.");
        return View();
    }

    public IActionResult LessonList(int? courseId = null)
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.CourseId = courseId;
        _notify.Information("Ders listesi yüklendi.");
        return View();
    }

    public IActionResult EditLesson(int courseId)
    {
        ViewBag.ApiBaseUrl = ApiBaseUrl;
        ViewBag.CourseId = courseId;
        _notify.Information($"Ders düzenleme sayfası açıldı (Kurs ID: {courseId}).");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadCoursePhoto(IFormFile photoFile)
    {
        if (photoFile == null || photoFile.Length == 0)
        {
            _notify.Error("Fotoğraf yüklenemedi. Lütfen geçerli bir dosya seçin.");
            return BadRequest("Dosya yüklenemedi.");
        }

        try
        {
            var fileName = Path.GetFileName(photoFile.FileName);
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "courses");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fullPath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await photoFile.CopyToAsync(stream);
            }

            _notify.Success("Fotoğraf başarıyla yüklendi.");

            return Ok(fileName);
        }
        catch (Exception ex)
        {
            _notify.Error($"Fotoğraf yüklenirken hata oluştu: {ex.Message}");
            return StatusCode(500, "Fotoğraf yüklenirken hata oluştu.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadLessonVideo(IFormFile videoFile)
    {
        if (videoFile == null || videoFile.Length == 0)
        {
            _notify.Error("Video yüklenemedi. Lütfen geçerli bir dosya seçin.");
            return BadRequest(new { message = "Video dosyası yüklenemedi." });
        }

        try
        {
            // For unique filenames, consider:
            // var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
            var fileName = Path.GetFileName(videoFile.FileName); // Using original filename for consistency for now
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "lessons", "videos");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fullPath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            _notify.Success("Video başarıyla yüklendi: " + fileName);
            return Ok(new { fileName = fileName });
        }
        catch (Exception ex)
        {
            _notify.Error($"Video yüklenirken hata oluştu: {ex.Message}");
            return StatusCode(500, new { message = $"Video yüklenirken sunucu hatası oluştu: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadLessonThumbnail(IFormFile thumbnailFile)
    {
        if (thumbnailFile == null || thumbnailFile.Length == 0)
        {
            _notify.Error("Thumbnail yüklenemedi. Lütfen geçerli bir dosya seçin.");
            return BadRequest(new { message = "Thumbnail dosyası yüklenemedi." });
        }

        try
        {
            // For unique filenames, consider:
            // var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(thumbnailFile.FileName);
            var fileName = Path.GetFileName(thumbnailFile.FileName); // Using original filename for consistency for now
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "lessons", "thumbnails");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fullPath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await thumbnailFile.CopyToAsync(stream);
            }

            _notify.Success("Thumbnail başarıyla yüklendi: " + fileName);
            return Ok(new { fileName = fileName });
        }
        catch (Exception ex)
        {
            _notify.Error($"Thumbnail yüklenirken hata oluştu: {ex.Message}");
            return StatusCode(500, new { message = $"Thumbnail yüklenirken sunucu hatası oluştu: {ex.Message}" });
        }
    }
}
