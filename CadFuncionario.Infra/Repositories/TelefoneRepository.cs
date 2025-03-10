using CadFuncionario.Domain.Entities;
using CadFuncionario.Infra.Data;
using CadFuncionario.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadFuncionario.Infra.Repositories
{
    public class TelefoneRepository : ITelefoneRepository
    {
        private readonly ApplicationDbContext _context;

        public TelefoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Telefone> AdicionarAsync(Telefone telefone)
        {
            await _context.Telefones.AddAsync(telefone);
            await _context.SaveChangesAsync();
            return telefone;
        }

        public async Task<Telefone> AtualizarAsync(Telefone telefone)
        {
            _context.Telefones.Update(telefone);
            await _context.SaveChangesAsync();
            return telefone;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var telefones = await _context.Telefones
                .Where(t => t.FuncionarioId == id)
                .ToListAsync();

            if (telefones == null)
                return false;
            foreach (var telefone in telefones)
            {
                _context.Telefones.Remove(telefone);
                await _context.SaveChangesAsync();
            }           
            return true;
        }

        public async Task<IEnumerable<Telefone>> ObterPorFuncionarioIdAsync(int funcionarioId)
        {
            return await _context.Telefones
                .Where(t => t.FuncionarioId == funcionarioId)
                .ToListAsync();
        }
    }
}
