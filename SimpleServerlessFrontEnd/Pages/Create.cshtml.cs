using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleServerlessFrontEnd.Models;
using System.Text.Json;
using System.Text;

public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public CreateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("SimpleCrudApiClient");
    }

    [BindProperty]
    public Guitar Guitar { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guitar guitar)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var content = new StringContent(JsonSerializer.Serialize(guitar), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Guitars", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }

            return Content("Error creating guitar");
        }

        catch (Exception ex)
        {
            return Content($"Error creating guitar: {ex}");
        }
    }    
}
