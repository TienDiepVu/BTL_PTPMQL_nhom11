using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL.Migrations
{
    /// <inheritdoc />
    public partial class Create_Table_Thuoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thuoc_NhomThuoc_MaNhomThuoc",
                table: "Thuoc");

            migrationBuilder.DropIndex(
                name: "IX_Thuoc_MaNhomThuoc",
                table: "Thuoc");

            migrationBuilder.DropColumn(
                name: "MaNhomThuoc",
                table: "Thuoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaNhomThuoc",
                table: "Thuoc",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Thuoc_MaNhomThuoc",
                table: "Thuoc",
                column: "MaNhomThuoc");

            migrationBuilder.AddForeignKey(
                name: "FK_Thuoc_NhomThuoc_MaNhomThuoc",
                table: "Thuoc",
                column: "MaNhomThuoc",
                principalTable: "NhomThuoc",
                principalColumn: "MaNhomThuoc");
        }
    }
}
