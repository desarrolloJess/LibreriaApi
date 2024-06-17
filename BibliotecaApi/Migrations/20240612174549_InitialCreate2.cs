using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BibliotecaApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "autor",
                columns: new[] { "AutorId", "Nombre", "Pais" },
                values: new object[,]
                {
                    { 1, "J.K. Rowling", "Mexico" },
                    { 2, "Stephen King", "Mexico" }
                });

            migrationBuilder.InsertData(
                table: "editorial",
                columns: new[] { "EditorialId", "Nombre" },
                values: new object[,]
                {
                    { 1, "Oxford University Press" },
                    { 2, "Pearson Education" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "autor",
                keyColumn: "AutorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "autor",
                keyColumn: "AutorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "editorial",
                keyColumn: "EditorialId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "editorial",
                keyColumn: "EditorialId",
                keyValue: 2);
        }
    }
}
