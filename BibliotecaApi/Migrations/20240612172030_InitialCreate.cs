using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "autor",
                columns: table => new
                {
                    AutorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autor", x => x.AutorId);
                });

            migrationBuilder.CreateTable(
                name: "editorial",
                columns: table => new
                {
                    EditorialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_editorial", x => x.EditorialId);
                });

            migrationBuilder.CreateTable(
                name: "libro",
                columns: table => new
                {
                    LibroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NumPaginas = table.Column<int>(type: "int", nullable: false),
                    FechaPublicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Edicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    EditorialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libro", x => x.LibroId);
                    table.ForeignKey(
                        name: "FK_libro_editorial_EditorialId",
                        column: x => x.EditorialId,
                        principalTable: "editorial",
                        principalColumn: "EditorialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "libroAutor",
                columns: table => new
                {
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libroAutor", x => new { x.LibroId, x.AutorId });
                    table.ForeignKey(
                        name: "FK_libroAutor_autor_AutorId",
                        column: x => x.AutorId,
                        principalTable: "autor",
                        principalColumn: "AutorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_libroAutor_libro_LibroId",
                        column: x => x.LibroId,
                        principalTable: "libro",
                        principalColumn: "LibroId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_libro_EditorialId",
                table: "libro",
                column: "EditorialId");

            migrationBuilder.CreateIndex(
                name: "IX_libroAutor_AutorId",
                table: "libroAutor",
                column: "AutorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "libroAutor");

            migrationBuilder.DropTable(
                name: "autor");

            migrationBuilder.DropTable(
                name: "libro");

            migrationBuilder.DropTable(
                name: "editorial");
        }
    }
}
