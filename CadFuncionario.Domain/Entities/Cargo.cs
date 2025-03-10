using System.ComponentModel.DataAnnotations;

namespace CadFuncionario.Domain.Entities
{
    public class Cargo
    {
        [Key]
        public int CargoId { get; set; }

        [Required, StringLength(50)]
        public string Nome { get; set; }

        [Required]
        public int Nivel { get; set; } // Define hierarquia dos cargos
    }
}
