using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeeklyMansion.Migrations
{
    /// <inheritdoc />
    public partial class RenameRoomStatusToIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_PropertyId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rooms");

            migrationBuilder.RenameIndex(
                name: "UX_Rooms_PropertyId_RoomNo",
                table: "Rooms",
                newName: "IX_Rooms_PropertyId_RoomNo");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Rooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rooms");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_PropertyId_RoomNo",
                table: "Rooms",
                newName: "UX_Rooms_PropertyId_RoomNo");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_PropertyId",
                table: "Rooms",
                column: "PropertyId");
        }
    }
}
