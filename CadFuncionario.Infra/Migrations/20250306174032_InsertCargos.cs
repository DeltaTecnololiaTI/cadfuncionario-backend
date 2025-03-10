using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadFuncionario.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InsertCargos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cargos",
                columns: new[] {  "Nome", "Nivel" },
                values: new object[,]
                {
                    {  "Diretor", 1 },
                    {  "Gerente", 2 },
                    {  "Líder", 3 },
                    { "Funcionário", 4 },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cargos",
                keyColumn: "CargoId",
                keyValues: new object[] { 1, 2, 3, 4 });
        }
    }
}
