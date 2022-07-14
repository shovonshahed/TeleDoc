using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeleDoc.API.Migrations
{
    public partial class addedSchedulToBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Schedules_ScheduleId",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "Booking",
                newName: "SchedulesScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_ScheduleId",
                table: "Booking",
                newName: "IX_Booking_SchedulesScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Schedules_SchedulesScheduleId",
                table: "Booking",
                column: "SchedulesScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Schedules_SchedulesScheduleId",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "SchedulesScheduleId",
                table: "Booking",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_SchedulesScheduleId",
                table: "Booking",
                newName: "IX_Booking_ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Schedules_ScheduleId",
                table: "Booking",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId");
        }
    }
}
