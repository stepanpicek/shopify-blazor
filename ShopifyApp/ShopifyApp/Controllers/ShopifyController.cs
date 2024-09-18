using Microsoft.AspNetCore.Mvc;

namespace ShopifyApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopifyController : Controller
{
    [HttpGet("index")]
    public IActionResult Index()
    {
        return Ok("Hello from ShopifyController!");
    }
}