using System.Diagnostics;
using FS0924_S18_L4.Services;
using FS0924_S18_L4.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FS0924_S18_L4.Controllers;

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

    public async Task<IActionResult> Privacy()
    {
        bool emailSent = await EmailService.SendEmailAsync(
            "email@example.com",
            "Test SendGrid",
            "Questa è una email di prova inviata da ASP.NET MVC."
        );

        if (emailSent)
        {
            Console.WriteLine("Email inviata correttamente!");
        }
        else
        {
            Console.WriteLine("Errore nell'invio della mail!");
        }

        return View();
    }
}
