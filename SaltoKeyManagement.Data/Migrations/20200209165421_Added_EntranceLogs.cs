using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SaltoKeyManagement.Data.Migrations
{
    public partial class Added_EntranceLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntrancesDbSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    DoorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrancesDbSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntrancesDbSet_DoorsDbSet_DoorId",
                        column: x => x.DoorId,
                        principalTable: "DoorsDbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrancesDbSet_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntrancesDbSet_DoorId",
                table: "EntrancesDbSet",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrancesDbSet_UserId",
                table: "EntrancesDbSet",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntrancesDbSet");
        }
    }
}
