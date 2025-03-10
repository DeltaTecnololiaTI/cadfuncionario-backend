using CadFuncionario.Application.DTOs;
using CadFuncionario.Application.Interfaces;
using CadFuncionario.Application.Mappers;
using CadFuncionario.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CadFuncionario.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;

        public FuncionarioController(IFuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        /// <param name="funcionarioDTO">Dados do funcionário a ser criado.</param>
        /// <returns>O funcionário criado.</returns>
        /// <response code="201">Funcionário criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, "Funcionário criado com sucesso", typeof(FuncionarioDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Dados inválidos")]
        public async Task<IActionResult> CriarFuncionario([FromBody] FuncionarioDTO funcionarioDTO)
        {
            if (funcionarioDTO == null || !ModelState.IsValid)
                return BadRequest("Dados inválidos");

            Funcionario? gestor = null;

            if (!string.IsNullOrEmpty(funcionarioDTO.NomeGestor))
            {
                var funcionarios = await _funcionarioService.ObterTodosFuncionariosAsync();
                var gestorDTO = funcionarios.FirstOrDefault(f => (f.Nome + " " + f.Sobrenome) == funcionarioDTO.NomeGestor);

                if (gestorDTO != null)
                    gestor = FuncionarioMapper.ToEntity(gestorDTO);
            }

            var novoFuncionario = await _funcionarioService.CriarFuncionarioAsync(funcionarioDTO);

            return CreatedAtAction(nameof(ObterFuncionarioPorId), new { id = novoFuncionario.FuncionarioId }, novoFuncionario);
        }

        /// <summary>
        /// Obtém um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <returns>O funcionário correspondente.</returns>
        /// <response code="200">Funcionário encontrado.</response>
        /// <response code="404">Funcionário não encontrado.</response>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Funcionário encontrado", typeof(FuncionarioDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Funcionário não encontrado")]
        public async Task<IActionResult> ObterFuncionarioPorId(int id)
        {
            var funcionario = await _funcionarioService.ObterFuncionarioPorIdAsync(id);
            if (funcionario == null)
                return NotFound();

            return Ok(funcionario);
        }

        /// <summary>
        /// Obtém todos os funcionários.
        /// </summary>
        /// <returns>Lista de funcionários.</returns>
        /// <response code="200">Lista de funcionários obtida com sucesso.</response>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lista de funcionários", typeof(List<FuncionarioDTO>))]
        public async Task<IActionResult> ObterTodosFuncionarios()
        {
            var funcionarios = await _funcionarioService.ObterTodosFuncionariosAsync();

            return Ok(funcionarios);
        }

        /// <summary>
        /// Atualiza um funcionário existente.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <param name="funcionarioDTO">Dados atualizados do funcionário.</param>
        /// <returns>O funcionário atualizado.</returns>
        /// <response code="200">Funcionário atualizado com sucesso.</response>
        /// <response code="400">IDs não coincidem.</response>
        [HttpPut("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Funcionário atualizado", typeof(FuncionarioDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "IDs não coincidem")]
        public async Task<IActionResult> AtualizarFuncionario(int id, [FromBody] FuncionarioDTO funcionarioDTO)
        {
            if (funcionarioDTO == null || id != funcionarioDTO.FuncionarioId)
                return BadRequest("ID da URL não corresponde ao ID do corpo da requisição.");

            var funcionarioAtualizado = await _funcionarioService.AtualizarFuncionarioAsync(funcionarioDTO);

            return Ok(funcionarioAtualizado);
        }

        /// <summary>
        /// Deleta um funcionário pelo ID.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="204">Funcionário deletado com sucesso.</response>
        /// <response code="404">Funcionário não encontrado.</response>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Funcionário deletado")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Funcionário não encontrado")]
        public async Task<IActionResult> DeletarFuncionario(int id)
        {
            var resultado = await _funcionarioService.DeletarFuncionarioAsync(id);
            if (!resultado)
                return NotFound();

            return NoContent();
        }
    }
}
