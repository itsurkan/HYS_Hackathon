using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomRoom.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ZoomMeetingId",
                table: "Meetings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZoomMeetingId",
                table: "Meetings");
        }
    }
}
