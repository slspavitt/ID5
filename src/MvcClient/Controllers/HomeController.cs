using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Managers;
using MvcClient.Models;

namespace MvcClient.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConnectToLocker _connectToLocker;
    private readonly IConnectToAPI _connectToAPI;
    private readonly IConnectToAuth _connectToAuth;

    public HomeController(ILogger<HomeController> logger, IConnectToLocker connectToLocker, IConnectToAPI connectToAPI, IConnectToAuth connectToAuth)
    {
        _logger = logger;
        _connectToAPI = connectToAPI;
        _connectToLocker = connectToLocker;
        _connectToAuth = connectToAuth;
    }

    public IActionResult Index()
    {
        return View();
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

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

    [HttpGet]
    [Route("Home/CallApi")]
    public async Task<IActionResult> CallApi()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return await CallApi(accessToken);
    }


    [Route("Home/CallApi/{accessToken}")]
    public async Task<IActionResult> CallApi(string accessToken)
    {
        ViewBag.Json = await _connectToAPI.Connect(accessToken);
        return View("CallApi");
    }

    //
    [HttpGet]
    [Route("Home/CallLocker")]
    public async Task<IActionResult> CallLocker()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return await CallLocker(accessToken);
    }


    [Route("Home/CallLocker/{accessToken}")]
    public async Task<IActionResult> CallLocker(string accessToken)
    {
        ViewBag.Json = await _connectToLocker.Connect(accessToken);
        return View("CallApi");
    }

  
    [HttpGet]
    [Route("Home/ReAuth")]

    public ActionResult ReAuth(string scope)
    {
        return new RedirectResult($"https://localhost:5001/connect/authorize?client_id=mvc&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%3A5002%2FHome%2FCallback&scope={scope}");
    }

    [HttpPost]
    [HttpGet]
    [Route("Home/Callback")]
    public async Task<IActionResult> CallbackAsync(string code, string state)
    {
        var response = await _connectToAuth.RequestTokenForREAuth(code);

        // This is a bit messy 
        if (response.Scope.Contains("api"))
        {
            return await CallApi(response.AccessToken);
        }
        return await CallLocker(response.AccessToken);
    }
}
