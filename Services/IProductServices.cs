using AzureTestingWebApplication.Models;

namespace AzureTestingWebApplication.Services
{
    public interface IProductServices
    {
        List<Product> GetProducts();
    }
}