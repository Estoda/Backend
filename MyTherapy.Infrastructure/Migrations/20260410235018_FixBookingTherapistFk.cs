using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTherapy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingTherapistFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_TherapistId",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Therapists_TherapistId",
                table: "Bookings",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Therapists_TherapistId",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_TherapistId",
                table: "Bookings",
                column: "TherapistId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
