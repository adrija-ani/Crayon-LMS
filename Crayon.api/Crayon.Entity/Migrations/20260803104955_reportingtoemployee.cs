using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crayon.Entity.Migrations
{
    /// <inheritdoc />
    public partial class reportingtoemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportingToEmployeeId",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ReportingToEmployeeId",
                table: "Employee",
                column: "ReportingToEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_ReportingToEmployeeId",
                table: "Employee",
                column: "ReportingToEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_ReportingToEmployeeId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ReportingToEmployeeId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ReportingToEmployeeId",
                table: "Employee");
        }
    }
}
