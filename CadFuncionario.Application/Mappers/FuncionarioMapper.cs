using CadFuncionario.Application.DTOs;
using CadFuncionario.Domain.Entities;

namespace CadFuncionario.Application.Mappers
{
    public static class FuncionarioMapper
    {
        public static FuncionarioDTO ToDTO(Funcionario funcionario)
        {
            return new FuncionarioDTO
            {
                FuncionarioId = funcionario.FuncionarioId,
                Nome = funcionario.Nome,
                Sobrenome = funcionario.Sobrenome,
                Email = funcionario.Email,
                Documento = funcionario.Documento,
                DataNascimento = funcionario.DataNascimento,
                CargoId = funcionario.CargoId,
                NomeCargo = funcionario.Cargo?.Nome,
                GestorId = funcionario.GestorId,
                NomeGestor = funcionario.Gestor != null ? $"{funcionario.Gestor.Nome}" : null,
                Telefones = funcionario.Telefones?.Select(t => new TelefoneDTO
                {
                    TelefoneId = t.TelefoneId,
                    Numero = t.Numero,
                    Tipo = t.Tipo
                }).ToList(),
                DataCriacao = funcionario.DataCriacao
            };
        }

        public static Funcionario ToEntity(FuncionarioDTO dto, Funcionario? gestor = null)
        {
            return new Funcionario
            {
                FuncionarioId = dto.FuncionarioId,
                Nome = dto.Nome,
                Sobrenome = dto.Sobrenome,
                Email = dto.Email,
                Documento = dto.Documento,
                DataNascimento = dto.DataNascimento,
                CargoId = dto.CargoId,
                GestorId = gestor?.FuncionarioId,
                Gestor = gestor,
                Telefones = dto.Telefones?.Select(t => new Telefone
                {
                    TelefoneId = t.TelefoneId,
                    Numero = t.Numero,
                    Tipo = t.Tipo,
                    FuncionarioId = dto.FuncionarioId
                }).ToList(),
                DataCriacao = dto.DataCriacao
            };
        }

    }
}
