using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;

namespace MvcClient.Controllers;

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

    public async Task<IActionResult> CallApi()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var content = await client.GetStringAsync("https://localhost:6001/identity");

        var parsed = JsonDocument.Parse(content);
        var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

        ViewBag.Json = formatted;
        return View();
    }

    [HttpPost]
    [HttpGet]
    [Route("Home/Callback")]
    public async Task<IActionResult> CallbackAsync(string code, string state)
    {
        var x = HttpContext.Request;

        return View("Index");
    }

    //public async Task<IActionResult> ReAuthFlow()
    //{
    //    var accessToken = await HttpContext.GetTokenAsync("access_token");

    //    var client = new HttpClient();
    //    var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
    //    var request = new AuthorizationCodeTokenRequest
    //    {
    //        Address = disco.AuthorizeEndpoint,
    //        ClientId = "mvc",
    //        ClientSecret = "secret",
    //        RedirectUri = "https://localhost:5002/Home/Callback",
    //        Code = "code",
    //        GrantType = OidcConstants.GrantTypes.AuthorizationCode,


    //        Parameters =
    //            {
    //                { "scope", "openid api1" },
    //                { "response_type", "code" },
    //            },
    //    };

    //    var response = await client.RequestAuthorizationCodeTokenAsync(request);
    //    return View("Index");

    //}

}
