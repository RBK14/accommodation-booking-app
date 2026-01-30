using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccommodationBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ListingReservationNamingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Start",
                table: "ScheduleSlots",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "ScheduleSlots",
                newName: "EndDate");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleSlots_Start_End",
                table: "ScheduleSlots",
                newName: "IX_ScheduleSlots_StartDate_EndDate");

            migrationBuilder.RenameColumn(
                name: "Listing_Street",
                table: "Reservations",
                newName: "ListingStreet");

            migrationBuilder.RenameColumn(
                name: "Listing_PostalCode",
                table: "Reservations",
                newName: "ListingPostalCode");

            migrationBuilder.RenameColumn(
                name: "Listing_Country",
                table: "Reservations",
                newName: "ListingCountry");

            migrationBuilder.RenameColumn(
                name: "Listing_City",
                table: "Reservations",
                newName: "ListingCity");

            migrationBuilder.RenameColumn(
                name: "Listing_BuildingNumber",
                table: "Reservations",
                newName: "ListingBuildingNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ScheduleSlots",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "ScheduleSlots",
                newName: "End");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleSlots_StartDate_EndDate",
                table: "ScheduleSlots",
                newName: "IX_ScheduleSlots_Start_End");

            migrationBuilder.RenameColumn(
                name: "ListingStreet",
                table: "Reservations",
                newName: "Listing_Street");

            migrationBuilder.RenameColumn(
                name: "ListingPostalCode",
                table: "Reservations",
                newName: "Listing_PostalCode");

            migrationBuilder.RenameColumn(
                name: "ListingCountry",
                table: "Reservations",
                newName: "Listing_Country");

            migrationBuilder.RenameColumn(
                name: "ListingCity",
                table: "Reservations",
                newName: "Listing_City");

            migrationBuilder.RenameColumn(
                name: "ListingBuildingNumber",
                table: "Reservations",
                newName: "Listing_BuildingNumber");
        }
    }
}
