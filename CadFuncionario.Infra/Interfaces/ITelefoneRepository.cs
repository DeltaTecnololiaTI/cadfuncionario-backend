using CadFuncionario.Domain.Entities;

namespace CadFuncionario.Infra.Interfaces
{
    public interface ITelefoneRepository
    {
        Task<IEnumerable<Telefone>> ObterPorFuncionarioIdAsync(int funcionarioId);
        Task<Telefone> AdicionarAsync(Telefone telefone);
        Task<Telefone> AtualizarAsync(Telefone telefone);
        Task<bool> DeletarAsync(int id);
    }
}
