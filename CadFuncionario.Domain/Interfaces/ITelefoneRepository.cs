using CadFuncionario.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CadFuncionario.Domain.Interfaces
{
    public interface ITelefoneRepository
    {
        Task<IEnumerable<Telefone>> ObterPorFuncionarioIdAsync(int funcionarioId);
        Task<Telefone> AdicionarAsync(Telefone telefone);
        Task<bool> DeletarAsync(int id);
    }
}
