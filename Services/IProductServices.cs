using AzureTestingWebApplication.Models;

namespace AzureTestingWebApplication.Services
{
    public interface IProductServices
    {
        Task<bool> IsBeta();
        List<Product> GetProducts();
    }
}