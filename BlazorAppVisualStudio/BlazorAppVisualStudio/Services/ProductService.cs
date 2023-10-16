using BlazorAppVisualStudio.Models;
using BlazorAppVisualStudio.Pages;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorAppVisualStudio.Services
{
    
    public class ProductService : IProductService
    {
        // Recibir como inyección de dependencia el HttpClient que ya se encuentra configurado de manera global en la app de blazor
        public readonly HttpClient client;
        public readonly JsonSerializerOptions options;

        public ProductService(HttpClient httpClient)
        {
            client = httpClient;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
        }

        //public async Task<List<Product>?> Get()
        //{
        //    var response = await client.GetAsync("/v1/products");
        //    return JsonSerializer.Deserialize<List<Product>>(await response.Content.ReadAsStringAsync());
        //}

        public async Task<List<Product>?> Get()
        {
            var response = await client.GetAsync("v1/products");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            return JsonSerializer.Deserialize<List<Product>>(content, options);
        }

        public async Task Add(Product product)
        {
            var response = await client.PostAsync("v1/products",  JsonContent.Create(product));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }

        public async Task Delete(int productId)
        {
            var response = await client.DeleteAsync($"v1/products/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }

        public async Task<Product?> Get(int productId)
        {
            var response = await client.GetAsync($"v1/products/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
            return JsonSerializer.Deserialize<Product>(content, options);
        }

        public async Task Update(Product product)
        {
            var response = await client.PutAsync($"v1/products/{product.Id}", JsonContent.Create(product));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new ApplicationException(content);
        }

    }

    public interface IProductService
    {
        Task<List<Product>> Get();
        Task Add(Product product);
        Task Delete(int productId);
        Task<Product?> Get(int productId);
        Task Update(Product product);
    }
}
