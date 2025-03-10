using CadFuncionario.Application.DTOs;
using CadFuncionario.Application.Interfaces;
using CadFuncionario.Domain.Entities;
using CadFuncionario.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace CadFuncionario.Application.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly ICargoRepository _cargoRepository;
        private readonly ITelefoneRepository _telefoneRepository;

        public FuncionarioService(
            IFuncionarioRepository funcionarioRepository,
            ICargoRepository cargoRepository,
            ITelefoneRepository telefoneRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _cargoRepository = cargoRepository;
            _telefoneRepository = telefoneRepository;
        }

        public async Task<IEnumerable<FuncionarioDTO>> ObterTodosFuncionariosAsync()
        {
            var funcionarios = await _funcionarioRepository.ObterTodosAsync();
            return funcionarios.Select(f => new FuncionarioDTO
            {
                FuncionarioId = f.FuncionarioId,
                Nome = f.Nome,
                Sobrenome = f.Sobrenome,
                Email = f.Email,
                Documento = f.Documento,
                DataNascimento = f.DataNascimento,
                CargoId = f.CargoId,
                NomeCargo = f.Cargo?.Nome,
                GestorId = f.GestorId,
                NomeGestor = f.Gestor != null ? $"{f.Gestor.Nome}" : null,
                Telefones = f.Telefones?.Select(t => new TelefoneDTO
                {
                    TelefoneId = t.TelefoneId,
                    FuncionarioId = t.FuncionarioId,
                    Numero = t.Numero,
                    Tipo = t.Tipo
                }).ToList(),
                DataCriacao = f.DataCriacao
            });
        }

        public async Task<FuncionarioDTO?> ObterFuncionarioPorIdAsync(int id)
        {
            var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
            if (funcionario == null) return null;

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
                    FuncionarioId = t.FuncionarioId,
                    Numero = t.Numero,
                    Tipo = t.Tipo
                }).ToList(),
                DataCriacao = funcionario.DataCriacao
            };
        }

        public async Task<FuncionarioDTO> CriarFuncionarioAsync(FuncionarioDTO funcionarioDTO)
        {


            // Verifica se o funcionário tem pelo menos 18 anos
            var idade = DateTime.Now.Year - funcionarioDTO.DataNascimento.Year;
            if (funcionarioDTO.DataNascimento.Date > DateTime.Now.AddYears(-idade))
            {
                idade--; // Ajusta caso o aniversário ainda não tenha ocorrido este ano
            }

            if (idade < 18)
                throw new Exception("O funcionário deve ter pelo menos 18 anos para ser cadastrado.");

            // Verifica se o cargo existe
            var cargo = await _cargoRepository.ObterPorIdAsync(funcionarioDTO.CargoId);
            if (cargo == null)
                throw new Exception("Cargo inválido.");

            Funcionario? gestor = null;
            if (funcionarioDTO.GestorId.HasValue)
            {
                gestor = await _funcionarioRepository.ObterPorIdAsync(funcionarioDTO.GestorId.Value);
                if (gestor == null)
                    throw new Exception("Gestor não encontrado.");

                var gestorCargo = await _cargoRepository.ObterPorIdAsync(gestor.CargoId);
                if (cargo?.Nivel < gestorCargo?.CargoId)
                    throw new Exception($"Um {gestorCargo.Nome} não pode cadastrar um {cargo.Nome}.");
            }
            else if (funcionarioDTO.CargoId != 1)
            {
                throw new Exception("Gestor não encontrado.");
            };

            if (string.IsNullOrWhiteSpace(funcionarioDTO.SenhaHash))
                throw new ArgumentException("A senha não pode ser nula ou vazia.");

            string hash = BCrypt.Net.BCrypt.HashPassword(funcionarioDTO.SenhaHash); // Gera um hash seguro
            bool senhaValida = BCrypt.Net.BCrypt.Verify(funcionarioDTO.SenhaHash, hash); // Valida a senha com o hash armazenado
            if (!senhaValida)
                throw new Exception("Senha inválida.");

            var funcionario = new Funcionario
            {
                Nome = funcionarioDTO.Nome,
                Sobrenome = funcionarioDTO.Sobrenome,
                Email = funcionarioDTO.Email,
                Documento = funcionarioDTO.Documento,
                DataNascimento = funcionarioDTO.DataNascimento,
                CargoId = funcionarioDTO.CargoId,
                GestorId = funcionarioDTO.GestorId,
                DataCriacao = DateTime.UtcNow,
                SenhaHash = hash
            };

            funcionario = await _funcionarioRepository.AdicionarAsync(funcionario);

            if (funcionarioDTO.Telefones != null)
            {
                foreach (var telefoneDTO in funcionarioDTO.Telefones)
                {
                    var telefone = new Telefone
                    {
                        FuncionarioId = funcionario.FuncionarioId,
                        Numero = telefoneDTO.Numero,
                        Tipo = telefoneDTO.Tipo
                    };
                    await _telefoneRepository.AdicionarAsync(telefone);
                }
            }

            return await ObterFuncionarioPorIdAsync(funcionario.FuncionarioId);
        }

        public async Task<FuncionarioDTO> AtualizarFuncionarioAsync(FuncionarioDTO funcionarioDTO)
        {
            var funcionario = await _funcionarioRepository.ObterPorIdAsync(funcionarioDTO.FuncionarioId);
            if (funcionario == null)
                throw new Exception("Funcionário não encontrado.");

            // Atualiza os dados básicos do funcionário
            funcionario.Nome = funcionarioDTO.Nome;
            funcionario.Sobrenome = funcionarioDTO.Sobrenome;
            funcionario.Email = funcionarioDTO.Email;
            funcionario.Documento = funcionarioDTO.Documento;
            funcionario.DataNascimento = funcionarioDTO.DataNascimento;
            funcionario.CargoId = funcionarioDTO.CargoId;
            funcionario.GestorId = funcionarioDTO.GestorId;
            funcionario.SenhaHash = funcionario.SenhaHash;

            // Telefones do banco de dados associados ao funcionário
            var telefonesExistentes = await _telefoneRepository.DeletarAsync(funcionario.FuncionarioId);

            // Telefones recebidos no DTO
            var telefonesDTO = funcionarioDTO.Telefones ?? new List<TelefoneDTO>();

            foreach (var telefoneDTO in telefonesDTO)
            {
                // Adiciona novo telefone
                var novoTelefone = new Telefone
                {
                    FuncionarioId = funcionario.FuncionarioId,
                    Numero = telefoneDTO.Numero,
                    Tipo = telefoneDTO.Tipo
                };
                await _telefoneRepository.AdicionarAsync(novoTelefone);
                
            }

            funcionario = await _funcionarioRepository.AtualizarAsync(funcionario);

            return await ObterFuncionarioPorIdAsync(funcionario.FuncionarioId);
        }


        public async Task<bool> DeletarFuncionarioAsync(int id)
        {
            var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
            if (funcionario == null)
                return false;

            return await _funcionarioRepository.DeletarAsync(funcionario.FuncionarioId);
        }
    }
}
