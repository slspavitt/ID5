using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;
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

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var content = await client.GetStringAsync("https://localhost:6001/identity");

        var parsed = JsonDocument.Parse(content);
        var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

        ViewBag.Json = formatted;
        return View("CallApi");
    }

    [HttpGet]
    [Route("Home/ReAuth")]

    public ActionResult ReAuth()
    {
        return new RedirectResult("https://localhost:5001/connect/authorize?client_id=mvc&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%3A5002%2FHome%2FCallback&scope=openid%20api1");
    }

    [HttpPost]
    [HttpGet]
    [Route("Home/Callback")]
    public async Task<IActionResult> CallbackAsync(string code, string state)
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
        var request = new AuthorizationCodeTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "mvc",
            ClientSecret = "secret",
            RedirectUri = "https://localhost:5002/Home/Callback",
            Code = code,
            GrantType = OidcConstants.GrantTypes.AuthorizationCode,
        };
        var response = await client.RequestAuthorizationCodeTokenAsync(request);


        return await CallApi(response.AccessToken);        
    }
}
