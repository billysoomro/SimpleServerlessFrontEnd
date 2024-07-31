using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleServerlessFrontEnd.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class EditModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public EditModel(IHttpClientFactory httpClientFactory)
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
        
    public async Task<IActionResult> OnPostAsync(Guitar guitar)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var content = new StringContent(JsonSerializer.Serialize(guitar), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/Guitars", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }

            return Content("Error updating guitar");
        }

        catch (Exception ex)
        {
            return Content($"Error updating guitar: {ex}");
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
