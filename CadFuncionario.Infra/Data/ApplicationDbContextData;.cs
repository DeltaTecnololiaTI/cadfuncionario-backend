using CadFuncionario.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CadFuncionario.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Telefone> Telefones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Funcionario
            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Email)
                .IsUnique(); // Email único

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Documento)
                .IsUnique(); // Documento único

            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Cargo)
                .WithMany()
                .HasForeignKey(f => f.CargoId)
                .OnDelete(DeleteBehavior.Restrict); // Restrição para não excluir cargos em uso

            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Gestor)
                .WithMany()
                .HasForeignKey(f => f.GestorId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // Configuração da entidade Telefone
            modelBuilder.Entity<Telefone>()
                .Property(t => t.Tipo)
                .HasConversion<string>(); // Armazena enum como string

            modelBuilder.Entity<Telefone>()
                .HasOne(t => t.Funcionario)
                .WithMany(f => f.Telefones)
                .HasForeignKey(t => t.FuncionarioId)
                .OnDelete(DeleteBehavior.Cascade); // Telefones são excluídos junto com o funcionário

            // Configuração da entidade Cargo
            modelBuilder.Entity<Cargo>()
                .HasIndex(c => c.Nome)
                .IsUnique(); // Nome do cargo único

            modelBuilder.Entity<Funcionario>()
                .Property(f => f.SenhaHash)
                .HasColumnName("SenhaHash")
                .HasColumnType("VARCHAR(255)")
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);


            base.OnModelCreating(modelBuilder);
        }

    }
}
