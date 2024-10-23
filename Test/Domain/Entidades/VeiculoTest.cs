using MinimalApi.Dominio.Entidades;

namespace Test.Domain.Entidades;

[TestClass]
public class VeiculoTest
{
    [TestMethod]
    public void TestarGetSetPropiedades()
    {
        // Arrange
        var veiculo = new Veiculo();

        // Act
        veiculo.Id = 1;
        veiculo.Nome = "teste";
        veiculo.Marca = "testando";
        veiculo.Ano = 2000;

        // Assert

        Assert.AreEqual(1, veiculo.Id);
        Assert.AreEqual("teste", veiculo.Nome);
        Assert.AreEqual("testando", veiculo.Marca);
        Assert.AreEqual(2000, veiculo.Ano);
    
    }
}