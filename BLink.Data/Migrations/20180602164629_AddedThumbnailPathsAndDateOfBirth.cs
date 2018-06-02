using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BLink.Data.Migrations
{
    public partial class AddedThumbnailPathsAndDateOfBirth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoThumbnailPath",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoThumbnailPath",
                table: "Clubs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PhotoThumbnailPath",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PhotoThumbnailPath",
                table: "Clubs");
        }
    }
}
