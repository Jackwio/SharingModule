using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharingModule.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkspaceIdToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "AppShareLinks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "AppShareLinkAccessLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppShareLinks_WorkspaceId",
                table: "AppShareLinks",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppShareLinkAccessLogs_WorkspaceId",
                table: "AppShareLinkAccessLogs",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppShareLinks_WorkspaceId",
                table: "AppShareLinks");

            migrationBuilder.DropIndex(
                name: "IX_AppShareLinkAccessLogs_WorkspaceId",
                table: "AppShareLinkAccessLogs");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "AppShareLinks");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "AppShareLinkAccessLogs");
        }
    }
}
