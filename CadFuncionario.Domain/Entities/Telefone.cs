using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CadFuncionario.Domain.Entities
{
    public class Telefone
    {
        [Key]
        public int TelefoneId { get; set; }

        [Required, ForeignKey("Funcionario")]
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        [Required, StringLength(20)]
        public string Numero { get; set; }

        [Required, StringLength(15)]
        public string Tipo { get; set; } // Celular, Residencial, Comercial
    }
}
