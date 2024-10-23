using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Test.Domain.Servico;

[TestClass]
public class VeiculoServicoTeste
{
    private DbContexto CriarContextoDeTeste()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DbContexto(configuration);
    }

    [TestMethod]
    public void TestandoSalvarVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo();
        veiculo.Nome = "teste";
        veiculo.Marca = "testando";
        veiculo.Ano = 2000;

        var veiculoServico = new VeiculoServico(context);

        // Act

        veiculoServico.Incluir(veiculo);


        // Assert

        Assert.AreEqual(1, veiculoServico.Todos(1).Count());

    }

    [TestMethod]
    public void TestandoBuscaPorId()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo();
        veiculo.Nome = "teste";
        veiculo.Marca = "testando";
        veiculo.Ano = 2000;

        var veiculoServico = new VeiculoServico(context);

        // Act

        veiculoServico.Incluir(veiculo);
        veiculo = veiculoServico.BuscaPorId(veiculo.Id);


        // Assert

        Assert.AreEqual(1, veiculo.Id);

    }

    [TestMethod]
    public void TestandAtualizarVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo();
        veiculo.Nome = "teste";
        veiculo.Marca = "testando";
        veiculo.Ano = 2000;

        var veiculo2 = new Veiculo();
        veiculo2.Nome = "teste2";
        veiculo2.Marca = "testando2";
        veiculo2.Ano = 2002;

        var veiculoServico = new VeiculoServico(context);

        // Act

        veiculoServico.Incluir(veiculo);
        veiculo.Nome = veiculo2.Nome;
        veiculo.Marca = veiculo2.Marca;
        veiculo.Ano = veiculo2.Ano;

        veiculoServico.Atualizar(veiculo);

        veiculo = veiculoServico.BuscaPorId(veiculo.Id);

        // Assert

        Assert.AreEqual(veiculo2.Nome, veiculo.Nome);
        Assert.AreEqual(veiculo2.Marca, veiculo.Marca);
        Assert.AreEqual(veiculo2.Ano, veiculo.Ano);

    }


    [TestMethod]
    public void ApagarVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

        var veiculo = new Veiculo();
        veiculo.Nome = "teste";
        veiculo.Marca = "testando";
        veiculo.Ano = 2000;

        var veiculoServico = new VeiculoServico(context);

        // Act

        veiculoServico.Incluir(veiculo);

        veiculo = veiculoServico.BuscaPorId(veiculo.Id);
        

        veiculoServico.Apagar(veiculo);

        // Assert

        Assert.AreEqual(1, veiculo.Id);
        Assert.AreEqual(0, veiculoServico.Todos(1).Count());

    }


}
