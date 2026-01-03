using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharingModule.Migrations
{
    /// <inheritdoc />
    public partial class AddShareLinkRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "AppShareLinks",
                type: "bytea",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "AppShareLinks");
        }
    }
}
