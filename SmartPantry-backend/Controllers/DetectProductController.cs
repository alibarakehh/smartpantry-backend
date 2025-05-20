using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // For EF Core methods
using SmartPantry_backend;           // Your DbContext namespace
using SmartPantry_backend.Models;    // Your Product model namespace
using System.Net.Http.Headers;

[ApiController]
[Route("api/[controller]")]
public class DetectProductController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppDbContext _context;

    public DetectProductController(IHttpClientFactory httpClientFactory, AppDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> DetectProduct([FromForm] IFormFile image)
    {
        if (image == null || image.Length == 0)
            return BadRequest("No image uploaded.");

        using var client = _httpClientFactory.CreateClient();

        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(image.OpenReadStream());
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
        content.Add(streamContent, "file", image.FileName);

        var mlApiUrl = "http://localhost:8000/predict"; // Your ML FastAPI endpoint

        var response = await client.PostAsync(mlApiUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "ML API error");
        }

        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

        if (!result.TryGetValue("label", out var detectedLabel) || string.IsNullOrWhiteSpace(detectedLabel))
        {
            return BadRequest("No label detected");
        }

        // Save product name if not already in DB
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Name.ToLower() == detectedLabel.ToLower());

        if (product == null)
        {
            product = new Product { Name = detectedLabel };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();  // Save new product
        }

        return Ok(new
        {
            label = detectedLabel,
            productId = product.ProductId
        });
    }
}


