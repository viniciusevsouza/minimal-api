using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Api.Test.Helpers;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;

namespace Test.TestResults;


[TestClass]
public class VeiculoRequestTest
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
    public async Task TestarPostVeiculos()
    {
        // Arrange
        var veiculo = new VeiculoDTO
        {
            Nome = "teste x",
            Marca = "testando",
            Ano = 2005
        };

        var token = await GetToken();

        var request = new HttpRequestMessage(HttpMethod.Post, "/Veiculos")
        {
            Content = new StringContent(JsonSerializer.Serialize(veiculo), Encoding.UTF8, "Application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act

        var response = await Setup.cliente.SendAsync(request);

        // Assert

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var administradorView = JsonSerializer.Deserialize<AdministradorModelView>(result, options);

        Assert.AreEqual("teste x", veiculo?.Nome ?? "");


    }


    [TestMethod]
    public async Task TestarGetVeiculosPorId()
    {
        // Arrange

        var token = await GetToken();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/Veiculos/{1}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act

        var response = await Setup.cliente.SendAsync(request);

        // Assert

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var veiculo = JsonSerializer.Deserialize<Veiculo>(result, options);

        Assert.AreEqual("teste1", veiculo?.Nome ?? "");

    }

    [TestMethod]
    public async Task TestarGetVeiculos()
    {
        // Arrange

        var token = await GetToken();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/Veiculos");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act

        var response = await Setup.cliente.SendAsync(request);

        // Assert

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var veiculo = JsonSerializer.Deserialize<List<Veiculo>>(result, options);

        Assert.AreEqual("teste1", veiculo?.FirstOrDefault()?.Nome ?? "");

    }

    [TestMethod]
    public async Task TestarPutVeiculos()
    {
        // Arrange
        var veiculo = new VeiculoDTO
        {
            Nome = "teste01",
            Marca = "testando",
            Ano = 2005
        };

        var token = await GetToken();

        var request = new HttpRequestMessage(HttpMethod.Put, $"/Veiculos/{1}")
        {
            Content = new StringContent(JsonSerializer.Serialize(veiculo), Encoding.UTF8, "Application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act

        var response = await Setup.cliente.SendAsync(request);

        // Assert

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var veiculoAtualizado = JsonSerializer.Deserialize<Veiculo>(result, options);

        Assert.AreEqual(veiculo.Nome, veiculoAtualizado?.Nome ?? "");
        Assert.AreEqual(veiculo.Marca, veiculoAtualizado?.Marca ?? "");
        Assert.AreEqual(veiculo.Ano, veiculoAtualizado?.Ano ?? 1980);
    }


    [TestMethod]
    public async Task TestarDeleteVeiculos()
    {
        // Arrange

        var token = await GetToken();

        var request = new HttpRequestMessage(HttpMethod.Delete, $"/Veiculos/{1}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act

        var response = await Setup.cliente.SendAsync(request);

        // Assert

        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);


    }


}
