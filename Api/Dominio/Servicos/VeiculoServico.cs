using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos;

public class VeiculoServico : IVeiculoServico
{
    private readonly DbContexto _contexto;
    public VeiculoServico(DbContexto contexto)
    {
        _contexto = contexto;

    }
    public void Apagar(Veiculo veiculo)
    {
        _contexto.Veiculos.Remove(veiculo);
        _contexto.SaveChanges();

    }

    public void Atualizar(Veiculo veiculo)
    {
        _contexto.Veiculos.Update(veiculo);
        _contexto.SaveChanges();
    }

    public Veiculo? BuscaPorId(int id)
    {
        return _contexto.Veiculos.Find(id);
    }

    public void Incluir(Veiculo veiculo)
    {
        _contexto.Veiculos.Add(veiculo);
        _contexto.SaveChanges();
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
    {
        var query = _contexto.Veiculos.AsQueryable();
        if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(marca))
        {
            query = _contexto.Veiculos.Where(v => v.Nome.ToLower().Contains(nome) && v.Marca.ToLower().Contains(marca));
        }
        else if (!string.IsNullOrEmpty(nome))
        {
            query = _contexto.Veiculos.Where(v => v.Nome.ToLower().Contains(nome));
        }
        
        else if (!string.IsNullOrEmpty(marca))
        {
            query = _contexto.Veiculos.Where(v => v.Marca.ToLower().Contains(marca));
        }

        int itemsPorPagina = 10;

        if (pagina != null)
        {
            query = query.Skip(((int)pagina - 1) * itemsPorPagina).Take(itemsPorPagina);
        }

        return query.ToList();
    }
}
