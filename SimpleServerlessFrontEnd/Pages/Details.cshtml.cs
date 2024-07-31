using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleServerlessFrontEnd.Models;
using System.Text.Json;

public class DetailsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public DetailsModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("SimpleCrudApiClient");
    }

    [BindProperty]
    public Guitar Guitar { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Guitar = await FetchGuitarAsync(id);

            if (Guitar == null)
            {
                return Content("Error fetching guitar details");
            }

            return Page();
        }

        catch (Exception ex)
        {
            return Content($"Error fetching guitar details: {ex}");
        }
    }

    private async Task<Guitar> FetchGuitarAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/Guitars/{id}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var guitar = JsonSerializer.Deserialize<Guitar>(content);

            return guitar;
        }

        return null;
    }
}
