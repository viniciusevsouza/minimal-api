using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Api.Test.Helpers;
using MinimalApi.Dominio.Entidades;
using MinimalAPi.Dominio.Enuns;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;

namespace Test.Requests;

[TestClass]
public class AdministradorRequestTest
{

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private async Task<string> GetToken()
    {
        var loginDTO = new LoginDTO
        {
            Email = "teste@adm.com",
            Senha = "teste"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");
        var response = await Setup.cliente.PostAsync("Administradores/login", content);
        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, options);
        var token = admLogado?.Token ?? "";

        return token;
    }

    [TestMethod]
    public async Task TestarPostLogin()
    {
        // Arrange
        var loginDTO = new LoginDTO
        {
            Email = "teste@adm.com",
            Senha = "teste"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");


        // Act

        var response = await Setup.cliente.PostAsync("Administradores/login", content);

        // Assert

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, options);

        Assert.IsNotNull(admLogado?.Email ?? "");
        Assert.IsNotNull(admLogado?.Perfil ?? "");
        Assert.IsNotNull(admLogado?.Token ?? "");
    }

    [TestMethod]
    public async Task TestarPostAdministradores()
    {
        // Arrange
        var administrador = new AdministradorDTO
        {
            Email = "testes@adm.com",
            Senha = "testes",
            Perfil = Enum.Parse<Perfil>("Adm")
        };

        var token = await GetToken();

        var request = new HttpRequestMessage(HttpMethod.Post, "/Administradores")
        {
            Content = new StringContent(JsonSerializer.Serialize(administrador), Encoding.UTF8, "Application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act

        var response = await Setup.cliente.SendAsync(request);

        // Assert

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var administradorView = JsonSerializer.Deserialize<AdministradorModelView>(result, options);

        Assert.AreEqual("testes@adm.com", administrador?.Email ?? "");


    }

    [TestMethod]
    public async Task TestarGetAdministradoresPorId()
    {
        // Arrange
        var token = await GetToken();
        var request = new HttpRequestMessage(HttpMethod.Get, $"/Administradores/{1}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act

        var response = await Setup.cliente.SendAsync(request);

        // Assert

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var administrador = JsonSerializer.Deserialize<Administrador>(result, options);

        Assert.AreEqual("teste@adm.com", administrador?.Email ?? "");


    }

    [TestMethod]
    public async Task TestarGetAdministradores()
    {
        // Arrange
        var token = await GetToken();
        var request = new HttpRequestMessage(HttpMethod.Get, "/Administradores");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act

        var response = await Setup.cliente.SendAsync(request);

        // Assert

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var administradores = JsonSerializer.Deserialize<List<Administrador>>(result, options);

        Assert.AreEqual("teste@adm.com", administradores?.FirstOrDefault()?.Email ?? "");


    }

}