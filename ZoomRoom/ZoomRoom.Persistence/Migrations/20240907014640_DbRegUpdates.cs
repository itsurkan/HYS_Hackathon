using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomRoom.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DbRegUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Meetings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Meetings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TimeZone",
                table: "Meetings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Meetings");
        }
    }
}
