using System.ComponentModel.DataAnnotations;

namespace CadFuncionario.Application.DTOs
{
    public class TelefoneDTO
    {
        public int TelefoneId { get; set; }

        [Required(ErrorMessage = "O ID do funcionário é obrigatório.")]
        public int FuncionarioId { get; set; }

        [Required(ErrorMessage = "O número do telefone é obrigatório.")]
        [StringLength(20, ErrorMessage = "O número do telefone pode ter no máximo 20 caracteres.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "O tipo do telefone é obrigatório.")]
        [StringLength(15, ErrorMessage = "O tipo do telefone pode ter no máximo 15 caracteres.")]
        public string Tipo { get; set; }
    }
}
