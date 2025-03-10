using CadFuncionario.Domain.Entities;
using CadFuncionario.Infra.Data;
using CadFuncionario.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CadFuncionario.Infra.Repositories
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly ApplicationDbContext _context;

        public FuncionarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Funcionario?> ObterPorIdAsync(int id)
        {
            return await _context.Funcionarios.Include(f => f.Cargo).Include(f => f.Telefones)
                                              .FirstOrDefaultAsync(f => f.FuncionarioId == id);
        }

        public async Task<IEnumerable<Funcionario>> ObterTodosAsync()
        {
            return await _context.Funcionarios.Include(f => f.Cargo).Include(f => f.Telefones)
                                              .ToListAsync();
        }

        public async Task<Funcionario> CriarAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();
            return funcionario;
        }

        public async Task<Funcionario> AtualizarAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();
            return funcionario;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null) return false;

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Funcionario> AdicionarAsync(Funcionario funcionario)
        {
            await _context.Funcionarios.AddAsync(funcionario);
            await _context.SaveChangesAsync();
            return funcionario;
        }
    }
}
