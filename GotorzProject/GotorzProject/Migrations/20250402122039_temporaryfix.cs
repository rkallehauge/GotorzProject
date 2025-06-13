using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GotorzProject.Migrations
{
    /// <inheritdoc />
    public partial class temporaryfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "TravelPackages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentID",
                table: "TravelPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
