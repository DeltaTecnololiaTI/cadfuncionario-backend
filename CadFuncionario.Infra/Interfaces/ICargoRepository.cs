using CadFuncionario.Domain.Entities;

namespace CadFuncionario.Infra.Interfaces
{
    public interface ICargoRepository
    {
        Task<Cargo?> ObterPorIdAsync(int id);
        Task<IEnumerable<Cargo>> ObterTodosAsync();
        Task<Cargo> AdicionarAsync(Cargo cargo);
        Task<Cargo> AtualizarAsync(Cargo cargo);
        Task<bool> DeletarAsync(int id);
        Task<bool> ExisteNomeAsync(string nome);
    }
}
