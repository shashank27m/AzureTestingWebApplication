using AzureTestingWebApplication.Models;
using System.Data.SqlClient;
using Microsoft.FeatureManagement;

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

    private SqlConnection GetConnection()
    {
        //var _sqlbuilder = new SqlConnectionStringBuilder();
        //_sqlbuilder.DataSource = db_source;
        //_sqlbuilder.UserID = db_user;
        //_sqlbuilder.Password = db_password;
        //_sqlbuilder.InitialCatalog = db_database;
        //return new SqlConnection(_configuration.GetConnectionString("SQLConnection1"));
        // After giving connection string in "AppConfiguration" it is injected directly to IConfiguration after installing nuget package microsoft.extensions.configuration.azureappconfiguration 6.0.0
        return new SqlConnection(_configuration["SQLConnection1"]);
    }
}
