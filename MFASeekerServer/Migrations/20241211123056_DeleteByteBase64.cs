using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MFASeekerServer.Migrations
{
    /// <inheritdoc />
    public partial class DeleteByteBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToiletImages_ImageFiles_ImageID",
                table: "ToiletImages");

            migrationBuilder.AlterColumn<string>(
                name: "ByteBase64",
                table: "ImageFiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_ToiletImages_ImageFiles_ImageID",
                table: "ToiletImages",
                column: "ImageID",
                principalTable: "ImageFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToiletImages_ImageFiles_ImageID",
                table: "ToiletImages");

            migrationBuilder.AlterColumn<string>(
                name: "ByteBase64",
                table: "ImageFiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToiletImages_ImageFiles_ImageID",
                table: "ToiletImages",
                column: "ImageID",
                principalTable: "ImageFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
