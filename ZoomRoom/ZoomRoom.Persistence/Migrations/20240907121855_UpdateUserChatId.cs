using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomRoom.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserChatId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "Users",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Users");
        }
    }
}
