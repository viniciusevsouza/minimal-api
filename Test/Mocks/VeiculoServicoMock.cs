using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;

namespace Test.Mocks;

public class VeiculoServicoMock : IVeiculoServico
{
    private static List<Veiculo> veiculos = new List<Veiculo>()
    {
        new Veiculo{
            Id = 1,
            Nome = "teste1",
            Marca = "testado",
            Ano = 2000
        },
        new Veiculo{
            Id = 2,
            Nome = "teste2",
            Marca = "testado",
            Ano = 2001

        }
    };

    public void Apagar(Veiculo veiculo)
    {
        veiculos.Remove(veiculo);
    }

    public void Atualizar(Veiculo veiculo)
    {
        veiculos[veiculo.Id] = veiculo;
    }

    public Veiculo? BuscaPorId(int id)
    {
        return veiculos.Find(a => a.Id == id);
    }

    public void Incluir(Veiculo veiculo)
    {
        veiculo.Id = veiculos.Count() + 1;
        veiculos.Add(veiculo);
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
    {
        return veiculos;
    }
}
