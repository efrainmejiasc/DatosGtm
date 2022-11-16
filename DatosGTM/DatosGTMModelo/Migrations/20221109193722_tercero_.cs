using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatosGTMModelo.Migrations
{
    public partial class tercero_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tercero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    Nombre = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    Nit = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    FechaInicio = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Identificador = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tercero", x => x.Nit);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tercero");
        }
    }
}
