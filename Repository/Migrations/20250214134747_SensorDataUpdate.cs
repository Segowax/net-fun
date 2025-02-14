using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class SensorDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "SensorData",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "SensorId",
                table: "SensorData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "SensorData");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SensorData",
                newName: "PropertyId");
        }
    }
}
