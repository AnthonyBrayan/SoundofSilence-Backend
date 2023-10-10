using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AudioFiles_Category_Id_category",
                table: "AudioFiles");

            migrationBuilder.DropIndex(
                name: "IX_AudioFiles_Id_category",
                table: "AudioFiles");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "AudioFiles",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AudioFiles",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Video",
                table: "AudioFiles",
                newName: "videoSrc");

            migrationBuilder.RenameColumn(
                name: "Audio",
                table: "AudioFiles",
                newName: "audioSrc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "AudioFiles",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "AudioFiles",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "videoSrc",
                table: "AudioFiles",
                newName: "Video");

            migrationBuilder.RenameColumn(
                name: "audioSrc",
                table: "AudioFiles",
                newName: "Audio");

            migrationBuilder.CreateIndex(
                name: "IX_AudioFiles_Id_category",
                table: "AudioFiles",
                column: "Id_category");

            migrationBuilder.AddForeignKey(
                name: "FK_AudioFiles_Category_Id_category",
                table: "AudioFiles",
                column: "Id_category",
                principalTable: "Category",
                principalColumn: "Id_category",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
