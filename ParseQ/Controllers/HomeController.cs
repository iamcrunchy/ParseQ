using System.Diagnostics;
using System.Xml.Linq;
using FileOperations;
using Microsoft.AspNetCore.Mvc;
using Parse;
using ParseQ.Helpers;
using ParseQ.Models;

namespace ParseQ.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(IFormFile file)
    {
        XDocument result;
        try
        {
            if (file.Length == 0)
            {
                ViewBag.ErrorMessage = "Please upload a valid text file.";
                return View("Index");
            }


            var text = await file.ReadAllBytesAsync();
            var parser = new ParseTextToQuestions();
            var questions = parser.BuildQuestionList(text);

            var qtiGenerator = new QtiGenerator();
            result = qtiGenerator.GenerateQtiXml(
                questions,
                Path.GetFileNameWithoutExtension(file.FileName));
            
            // print the QTI XML to the screen. 
            // todo: display the quiz in a new window/page, add link to download the file
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