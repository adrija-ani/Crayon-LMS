using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crayon.Entity.Migrations
{
    /// <inheritdoc />
    public partial class leavestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AppliedDate",
                table: "LeaveApplication",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "LeaveApplication",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CheckOutTime",
                table: "Attendance",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<string>(
                name: "AttendanceStatus",
                table: "Attendance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "HoursWorked",
                table: "Attendance",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedDate",
                table: "LeaveApplication");

            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "LeaveApplication");

            migrationBuilder.DropColumn(
                name: "AttendanceStatus",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "HoursWorked",
                table: "Attendance");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CheckOutTime",
                table: "Attendance",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }
    }
}
