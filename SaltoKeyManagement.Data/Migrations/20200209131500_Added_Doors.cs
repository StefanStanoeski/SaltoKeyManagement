using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SaltoKeyManagement.Data.Migrations
{
    public partial class Added_Doors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersDbSet");

            migrationBuilder.CreateTable(
                name: "DoorsDbSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorsDbSet", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoorsDbSet");

            migrationBuilder.CreateTable(
                name: "UsersDbSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDbSet", x => x.Id);
                });
        }
    }
}
