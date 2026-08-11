using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crayon.Entity.Migrations
{
    /// <inheritdoc />
    public partial class reportingemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportingEmployeeId",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ReportingEmployeeId",
                table: "Employee",
                column: "ReportingEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_ReportingEmployeeId",
                table: "Employee",
                column: "ReportingEmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_ReportingEmployeeId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ReportingEmployeeId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ReportingEmployeeId",
                table: "Employee");
        }
    }
}
