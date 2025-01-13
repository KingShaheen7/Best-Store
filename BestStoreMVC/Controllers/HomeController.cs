using BestStoreMVC.Models;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly IWebHostEnvironment environment;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment environment)
    {
        this.context = context;
        this.environment = environment;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var recent = context.Products.OrderByDescending(p => p.Id).Take(7).ToList();
        return View(recent);
    }

    public IActionResult About_Us()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
