using CadFuncionario.Domain.Entities;

namespace CadFuncionario.Infra.Interfaces
{
    public interface IFuncionarioRepository
    {
        Task<Funcionario?> ObterPorIdAsync(int id);
        Task<IEnumerable<Funcionario>> ObterTodosAsync();
        Task<Funcionario> CriarAsync(Funcionario funcionario);
        Task<Funcionario> AtualizarAsync(Funcionario funcionario);
        Task<bool> DeletarAsync(int id);
        Task<Funcionario> AdicionarAsync(Funcionario funcionario);
    }
}
