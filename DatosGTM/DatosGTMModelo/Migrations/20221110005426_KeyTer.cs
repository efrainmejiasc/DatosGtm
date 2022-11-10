using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatosGTMModelo.Migrations
{
    public partial class KeyTer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tercero",
                table: "Tercero");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tercero",
                table: "Tercero",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tercero",
                table: "Tercero");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tercero",
                table: "Tercero",
                column: "Nit");
        }
    }
}
