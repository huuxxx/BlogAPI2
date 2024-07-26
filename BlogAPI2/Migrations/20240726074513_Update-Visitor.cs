using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPI2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVisitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ViewedSearch",
                table: "Visitors",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewedSearch",
                table: "Visitors");
        }
    }
}
