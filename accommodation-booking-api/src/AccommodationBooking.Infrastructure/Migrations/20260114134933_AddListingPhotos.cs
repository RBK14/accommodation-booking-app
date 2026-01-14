using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccommodationBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddListingPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrls",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrls",
                table: "Listings");
        }
    }
}
