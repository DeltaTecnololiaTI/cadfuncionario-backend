using System.ComponentModel.DataAnnotations;

namespace CadFuncionario.Application.DTOs
{
    public class FuncionarioDTO
    {
        public int FuncionarioId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O sobrenome pode ter no máximo 100 caracteres.")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(150, ErrorMessage = "O e-mail pode ter no máximo 150 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O documento é obrigatório.")]
        [StringLength(20, ErrorMessage = "O documento pode ter no máximo 20 caracteres.")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O cargo é obrigatório.")]
        public int CargoId { get; set; }

        public string? NomeCargo { get; set; }

        public int? GestorId { get; set; }

        public string? NomeGestor { get; set; }

        public List<TelefoneDTO>? Telefones { get; set; }

        //[MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string? SenhaHash { get; set; }

        public DateTime DataCriacao { get; set; }
    }
}
