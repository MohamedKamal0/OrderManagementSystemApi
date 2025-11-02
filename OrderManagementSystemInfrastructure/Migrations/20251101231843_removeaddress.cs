using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementSystemInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "AddressLine2",
                table: "Addresses",
                newName: "AddressLine");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressLine",
                table: "Addresses",
                newName: "AddressLine2");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Addresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Addresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
