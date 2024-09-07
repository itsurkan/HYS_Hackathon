using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomRoom.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ZoomMeetingId",
                table: "Meetings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZoomMeetingId",
                table: "Meetings",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");
        }
    }
}
