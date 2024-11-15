using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace distributedsystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Locations",
                newName: "MaxCapacity");

            migrationBuilder.AddColumn<int>(
                name: "CurrentCapacity",
                table: "Locations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMatriz",
                table: "Locations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentCapacity",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsMatriz",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "MaxCapacity",
                table: "Locations",
                newName: "Capacity");

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                table: "Locations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
