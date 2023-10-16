using BlazorAppVisualStudio.Models;
using System.Text.Json;

namespace BlazorAppVisualStudio.Services
{
    public class CategoryService : ICategoryService
    {
        public readonly HttpClient client;
        public readonly JsonSerializerOptions options;

        public CategoryService(HttpClient client)
        {
            this.client = client;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
        }

        public async Task<List<Category>?> Get()
        {
            var response = await client.GetAsync("v1/categories");
            var content = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return JsonSerializer.Deserialize<List<Category>>(content, options);
        }

    }

    public interface ICategoryService
    {
        Task<List<Category>> Get();
    }
}
