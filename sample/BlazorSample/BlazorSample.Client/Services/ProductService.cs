using BlazorSample.Client.Models;
using Light.Contracts;

namespace BlazorSample.Client.Services;

public class ProductService
{
    private readonly IEnumerable<Product> _products;

    public ProductService()
    {
        var products = new List<Product>();

        for (var i = 1; i <= 100; i++)
        {
            products.Add(new Product
            {
                Id = i,
                Name = $"Product {i}",
                Description = $"Description for product {i}",
                Price = i * 10.0,
            });
        }

        _products = products;
    }
    
    public Paged<Product> GetPagedProduct(ProductSearch search)
    {
        return _products.ToPaged(search.Page, search.PageSize);
    }
}
