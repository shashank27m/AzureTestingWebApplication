using AzureTestingWebApplication.Models;
using System.Data.SqlClient;
using Microsoft.FeatureManagement;
using System.Text.Json;

namespace AzureTestingWebApplication.Services;

public class ProductServices : IProductServices
{
    //private static string db_source = "webappsampledb.database.windows.net";
    //private static string db_user = "sqladmin";
    //private static string db_password = "adminsql@123";
    //private static string db_database = "WebAppDB";

    //Instead above we can use IConfiguation to get connection string from appsettings.json or from Azure web app configuration
    private readonly IConfiguration _configuration;
    private readonly IFeatureManager _featureManager;

    public ProductServices(IConfiguration configuration, IFeatureManager featureManager)
    {
        _configuration = configuration;
        _featureManager = featureManager;
    }

    public async Task<bool> IsBeta()
    {
        // "beta" is the Feature flag name given in the "Feature Manager".

        //if (await _featureManager.IsEnabledAsync("beta"))
        //    return true;
        //else
        //    return false;
        return await _featureManager.IsEnabledAsync("beta");
    }

    public List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();

        //If mentioned in appsettings.
        if (bool.Parse(_configuration["IsFunctionsEnabled"]))
        {
            return GetProductsFromFunction().Result;
        }

        ////If Feature flag is enabled.
        //if (_featureManager.IsEnabledAsync("IsFunctionsEnabled").Result)
        //{
        //    return GetProductsFromFunction().Result;
        //}
            

        using (var conn = GetConnection())
        {
            string sqlstatement = "select * from Products";
            conn.Open();
            SqlCommand cmd = new SqlCommand(sqlstatement, conn);
            using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    Product product = new Product
                    {
                        ProductID = sqlDataReader.GetInt32(0),
                        ProductName = sqlDataReader.GetString(1),
                        Quantity = sqlDataReader.GetInt32(2),
                    };
                    products.Add(product);
                }
            }
        }
        return products;
    }

    private async Task<List<Product>> GetProductsFromFunction()
    {
        string functionURL = "https://azuresamplefunction.azurewebsites.net/api/GetProducts?code=Ap49E3tcWFtCbYlHlgGUliG-TbdnT0zybfbZ_yiC55ozAzFuiuuLgg==";
        using (HttpClient httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync(functionURL);
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Product>>(content);
        }
    }

    private SqlConnection GetConnection()
    {
        //var _sqlbuilder = new SqlConnectionStringBuilder();
        //_sqlbuilder.DataSource = db_source;
        //_sqlbuilder.UserID = db_user;
        //_sqlbuilder.Password = db_password;
        //_sqlbuilder.InitialCatalog = db_database;
        //return new SqlConnection(_sqlbuilder.ConnectionString);

        //When we are connecting with WebApp --> Settings --> Configuration --> Connection Strings
        //return new SqlConnection(_configuration.GetConnectionString("SQLConnection1"));

        // After giving connection string in "AppConfiguration" it is injected directly to IConfiguration after installing nuget package microsoft.extensions.configuration.azureappconfiguration 6.0.0
        return new SqlConnection(_configuration["SQLConnection1"]);
    }

}
