using AzureTestingWebApplication.Models;
using AzureTestingWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureTestingWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;
        public List<Product> Products;
        public bool IsBeta;
        private readonly IProductServices _services;

        public IndexModel(/*ILogger<IndexModel> logger*/IProductServices services)
        {
            _services = services;
            //_logger = logger;
        }

        public void OnGet()
        {
            IsBeta = _services.IsBeta().Result;
            Products = _services.GetProducts();
        }

    }
}