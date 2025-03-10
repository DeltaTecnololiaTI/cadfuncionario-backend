using CadFuncionario.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CadFuncionario.Domain.Interfaces
{
    public interface IFuncionarioRepository
    {
        Task<IEnumerable<Funcionario>> ObterTodosAsync();
        Task<Funcionario?> ObterPorIdAsync(int id);
        Task<Funcionario> AdicionarAsync(Funcionario funcionario);
        Task<Funcionario> AtualizarAsync(Funcionario funcionario);
        Task<bool> DeletarAsync(Funcionario funcionario);
    }
}
