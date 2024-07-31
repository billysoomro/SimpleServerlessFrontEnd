using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleServerlessFrontEnd.Models;
using System.Text.Json;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
        
    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("SimpleCrudApiClient");
    }

    [BindProperty]
    public List<Guitar> Guitars { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Guitars = await FetchGuitarsAsync();

            if (Guitars == null)
            {
                return Content("Error fetching guitars");
            }

            return Page();
        }

        catch (Exception ex) 
        {
            return Content($"Error fetching guitars: {ex}");
        }
    }

    private async Task<List<Guitar>> FetchGuitarsAsync()
    {
        var response = await _httpClient.GetAsync("/api/Guitars");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var guitars = JsonSerializer.Deserialize<List<Guitar>>(content);

            return guitars;
        }

        return null;
    }
}