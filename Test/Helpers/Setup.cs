using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Dominio.Interfaces;
using Test.Mocks;

namespace Api.Test.Helpers;

public class Setup
{
    public const string PORT = "5120";
    public static TestContext textContext = default!;
    public static WebApplicationFactory<Startup> http = default!;
    public static HttpClient cliente = default!;

    public static void ClassInit(TestContext testContext)
    {
        Setup.textContext = testContext;
        Setup.http = new WebApplicationFactory<Startup>();

        Setup.http = Setup.http.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
                services.AddScoped<IVeiculoServico, VeiculoServicoMock>();
            });
        });

        Setup.cliente = Setup.http.CreateClient();
    }

    public static void ClassCleanup()
    {
        Setup.http.Dispose();
    }


}