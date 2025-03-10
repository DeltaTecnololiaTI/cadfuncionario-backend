using CadFuncionario.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CadFuncionario.API.Controllers
{
    /// <summary>
    /// Controlador responsável pela autenticação de usuários.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Autentica um usuário e gera um token JWT.
        /// </summary>
        /// <param name="login">Objeto contendo credenciais do usuário.</param>
        /// <returns>Token JWT se a autenticação for bem-sucedida.</returns>
        /// <response code="200">Retorna o token JWT quando a autenticação é bem-sucedida.</response>
        /// <response code="401">Retorna erro se as credenciais forem inválidas.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Autenticação de usuário", Description = "Autentica um usuário e retorna um token JWT.")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            // Simulação de validação de usuário
            if (login.Username == "admin" && login.Password == "123456")
            {
                var token = _tokenService.GenerateToken(login.Username);
                return Ok(new { token });
            }
            return Unauthorized();
        }
    }

    /// <summary>
    /// Objeto de transferência para autenticação de usuário.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Nome de usuário para autenticação.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        public string Password { get; set; }
    }
}
