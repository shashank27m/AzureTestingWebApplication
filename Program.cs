using AzureTestingWebApplication.Services;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//After creating "AppConfiguration" service in azure resource and giving key value pair of connection string in "Configuration Explorer", We take this key from "Access Keys"
var connectionstring = "Endpoint=https://azurewebappconfig.azconfig.io;Id=SuCt-l0-s0:79vrltvHAU1Na3t6v3rz;Secret=Z3Rjtl2Hss32lQwZlfALagsW1A1fuTsnpsiaxCLFVsA=";

// After giving connection string in "AppConfiguration" to inject it to IConfiguration, install nuget package microsoft.extensions.configuration.azureappconfiguration 6.0.0 and use below code.
//builder.Host.ConfigureAppConfiguration(builder =>
//{
//    builder.AddAzureAppConfiguration(connectionstring);
//});
//Use below instead of above one
//builder.Configuration.AddAzureAppConfiguration(connectionstring);

//To use "AppConfiguration" service along with feature flags in "Feature Manager" in current "AppConfiguration"(under operations). To use "Feature Manager", install nuget package microsoft.featuremanagement.aspnetcore\2.5.1\
//We can also use this feature flags on controller level also for that see documentation https://learn.microsoft.com/en-us/azure/azure-app-configuration/use-feature-flags-dotnet-core?tabs=core5x.
builder.Configuration.AddAzureAppConfiguration(options=>options.Connect(connectionstring).UseFeatureFlags());
//Add this, to use feature flag service.
builder.Services.AddFeatureManagement();

builder.Services.AddTransient<IProductServices, ProductServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
