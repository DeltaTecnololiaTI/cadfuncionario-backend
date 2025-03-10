using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CadFuncionario.Domain.Entities
{
    public class Funcionario
    {
        [Key]
        public int FuncionarioId { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; }

        [Required, StringLength(100)]
        public string Sobrenome { get; set; }

        [Required, StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string Documento { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required, ForeignKey("Cargo")]
        public int CargoId { get; set; }
        public Cargo Cargo { get; set; }

        [ForeignKey("Gestor")]
        public int? GestorId { get; set; }
        public Funcionario? Gestor { get; set; }

       // [JsonIgnore]
        public string? SenhaHash { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public List<Telefone> Telefones { get; set; } = new();
    }
}
