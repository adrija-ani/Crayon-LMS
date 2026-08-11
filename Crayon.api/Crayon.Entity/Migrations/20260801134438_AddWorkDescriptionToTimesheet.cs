using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crayon.Entity.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkDescriptionToTimesheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkDescription",
                table: "Timesheet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkDescription",
                table: "Timesheet");
        }
    }
}
