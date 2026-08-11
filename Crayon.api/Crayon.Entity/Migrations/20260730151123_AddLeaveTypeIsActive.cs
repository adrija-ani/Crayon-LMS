using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crayon.Entity.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveTypeIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LeaveType",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LeaveType");
        }
    }
}
