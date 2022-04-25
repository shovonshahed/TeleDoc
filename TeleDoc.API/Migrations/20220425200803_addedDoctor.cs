using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeleDoc.API.Migrations
{
    public partial class addedDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_AspNetUsers_DoctorId",
                table: "Prescription");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "Prescription");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Prescription",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CertificateUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "College",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_AspNetUsers_DoctorId",
                table: "Prescription",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_AspNetUsers_DoctorId",
                table: "Prescription");

            migrationBuilder.DropColumn(
                name: "CertificateUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "College",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Prescription",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "Prescription",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_AspNetUsers_DoctorId",
                table: "Prescription",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
