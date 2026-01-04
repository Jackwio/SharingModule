using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharingModule.Migrations
{
    /// <inheritdoc />
    public partial class RemoveResourceTypeAndResourceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppShareLinks_ResourceType_ResourceId",
                table: "AppShareLinks");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "AppShareLinks");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "AppShareLinks");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppShareLinks");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppShareLinkAccessLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceId",
                table: "AppShareLinks",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "AppShareLinks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppShareLinks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppShareLinkAccessLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppShareLinks_ResourceType_ResourceId",
                table: "AppShareLinks",
                columns: new[] { "ResourceType", "ResourceId" });
        }
    }
}
