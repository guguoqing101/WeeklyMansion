using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WeeklyMansion.Data;

#nullable disable

namespace WeeklyMansion.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20260605000100_AddUniqueRoomNoPerProperty")]
    public partial class AddUniqueRoomNoPerProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UX_Rooms_PropertyId_RoomNo",
                table: "Rooms",
                columns: new[] { "PropertyId", "RoomNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Rooms_PropertyId_RoomNo",
                table: "Rooms");
        }
    }
}
