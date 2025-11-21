using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshChoice.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "Shift",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TotalTime",
                table: "Shift",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Shift_DepartmentId",
                table: "Shift",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Department_DepartmentId",
                table: "Shift",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Department_DepartmentId",
                table: "Shift");

            migrationBuilder.DropIndex(
                name: "IX_Shift_DepartmentId",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "Shift");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
