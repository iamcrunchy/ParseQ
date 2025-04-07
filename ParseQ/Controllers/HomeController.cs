using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ParseQ.Models;

namespace ParseQ.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> OpenTextFile(IFormFile file)
    {
        try
        {
            if(file == null || file.Length == 0)
            {
                ViewBag.ErrorMessage = "Please upload a valid text file.";
                return View("Index");
            }

            var path = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
        }

        return View("Index");
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