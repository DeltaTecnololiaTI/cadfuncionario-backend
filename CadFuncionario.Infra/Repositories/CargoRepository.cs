using CadFuncionario.Domain.Entities;
using CadFuncionario.Infra.Data;
using CadFuncionario.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CadFuncionario.Infra.Repositories
{
    public class CargoRepository : ICargoRepository
    {
        private readonly ApplicationDbContext _context;

        public CargoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cargo> AdicionarAsync(Cargo cargo)
        {
            await _context.Cargos.AddAsync(cargo);
            await _context.SaveChangesAsync();
            return cargo;
        }

        public async Task<Cargo> AtualizarAsync(Cargo cargo)
        {
            _context.Cargos.Update(cargo);
            await _context.SaveChangesAsync();
            return cargo;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null)
                return false;

            _context.Cargos.Remove(cargo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteNomeAsync(string nome)
        {
            return await _context.Cargos.AnyAsync(c => c.Nome == nome);
        }

        public async Task<Cargo?> ObterPorIdAsync(int id)
        {
            return await _context.Cargos.FindAsync(id);
        }

        public async Task<IEnumerable<Cargo>> ObterTodosAsync()
        {
            return await _context.Cargos.ToListAsync();
        }
    }
}
