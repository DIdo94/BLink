using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BLink.Data.Migrations
{
    public partial class AddedClubEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EventType = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClubEvents_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubEventMember",
                columns: table => new
                {
                    ClubEventId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubEventMember", x => new { x.ClubEventId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_ClubEventMember_ClubEvents_ClubEventId",
                        column: x => x.ClubEventId,
                        principalTable: "ClubEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubEventMember_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubEvents_ClubId",
                table: "ClubEvents",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubEventMember_MemberId",
                table: "ClubEventMember",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubEventMember");

            migrationBuilder.DropTable(
                name: "ClubEvents");
        }
    }
}
