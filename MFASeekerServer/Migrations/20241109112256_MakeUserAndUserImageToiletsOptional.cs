using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MFASeekerServer.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserAndUserImageToiletsOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Toilets_Users_UserID",
                table: "Toilets");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ImageFiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Toilets_Users_UserID",
                table: "Toilets",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Toilets_Users_UserID",
                table: "Toilets");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "ImageFiles");

            migrationBuilder.AddForeignKey(
                name: "FK_Toilets_Users_UserID",
                table: "Toilets",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
