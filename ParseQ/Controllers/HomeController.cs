using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ParseDeepSeekFormat;
using ParseQ.Helpers;
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
    public async Task<IActionResult> Index(IFormFile file)
    {
        try
        {
            if(file.Length == 0)
            {
                ViewBag.ErrorMessage = "Please upload a valid text file.";
                return View("Index");
            }

            var path = Path.GetTempFileName();

            var text = await file.ReadAllBytesAsync();

           // var parse = new ParseDeepSeekFormat();
           var parser = new Parse();
           var parsedSuccessfully = parser.ParseTextFile(text, Path.GetFileNameWithoutExtension(file.FileName));
           //parser.ConvertTextToQtiAsync(text, Path.GetFileNameWithoutExtension(file.FileName));
           //var result = parser.ConvertTextToQtiAsync(stream, path);

           //await file.CopyToAsync(stream);
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