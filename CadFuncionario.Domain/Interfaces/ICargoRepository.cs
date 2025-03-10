using CadFuncionario.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CadFuncionario.Domain.Interfaces
{
    public interface ICargoRepository
    {
        Task<IEnumerable<Cargo>> ObterTodosAsync();
        Task<Cargo?> ObterPorIdAsync(int id);
        Task<Cargo> AdicionarAsync(Cargo cargo);
        Task<Cargo> AtualizarAsync(Cargo cargo);
        Task<bool> DeletarAsync(int id);
        Task<bool> ExisteNomeAsync(string nome);
    }
}
