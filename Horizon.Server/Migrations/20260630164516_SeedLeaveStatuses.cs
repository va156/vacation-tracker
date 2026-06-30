using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizon.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedLeaveStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            migrationBuilder.InsertData(
                table: "LeaveStatuses",
                columns: new[] { "Code", "Name", "SortOrder", "CreatedAt", "CreatedBy", "IsActive" },
                values: new object[,]
                {
                    { "PLANNED",     "Запланирован",  1, seedDate, 1, true },
                    { "APPROVED",    "Утверждён",     2, seedDate, 1, true },
                    { "USED",        "Использован",   3, seedDate, 1, true },
                    { "CANCELLED",   "Отменён",       4, seedDate, 1, true },
                    { "RESCHEDULED", "Перенесён",     5, seedDate, 1, true },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("LeaveStatuses", "Code",
                new object[] { "PLANNED", "APPROVED", "USED", "CANCELLED", "RESCHEDULED" });
        }
    }
}
