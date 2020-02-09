using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SaltoKeyManagement.Data.Migrations
{
    public partial class Added_Entrance_TimeOfEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfEntry",
                table: "EntrancesDbSet",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfEntry",
                table: "EntrancesDbSet");
        }
    }
}
