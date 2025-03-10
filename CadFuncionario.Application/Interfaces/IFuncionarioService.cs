using CadFuncionario.Application.DTOs;

namespace CadFuncionario.Application.Interfaces
{
    public interface IFuncionarioService
    {
        Task<IEnumerable<FuncionarioDTO>> ObterTodosFuncionariosAsync();
        Task<FuncionarioDTO?> ObterFuncionarioPorIdAsync(int id);
        Task<FuncionarioDTO> CriarFuncionarioAsync(FuncionarioDTO funcionarioDTO);
        Task<FuncionarioDTO> AtualizarFuncionarioAsync(FuncionarioDTO funcionarioDTO);
        Task<bool> DeletarFuncionarioAsync(int id);
    }
}

