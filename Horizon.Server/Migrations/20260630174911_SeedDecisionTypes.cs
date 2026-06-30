using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizon.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedDecisionTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.InsertData(
                table: "DecisionTypes",
                columns: new[] { "Code", "Name", "IsFinal", "SortOrder", "CreatedAt", "CreatedBy", "IsActive" },
                values: new object[,]
                {
                    { "APPROVE",   "Согласовать",           false, 1, seedDate, 1, true },
                    { "REJECT",    "Отклонить",             true,  2, seedDate, 1, true },
                    { "SEND_BACK", "Вернуть на доработку",  false, 3, seedDate, 1, true },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "DecisionTypes", keyColumn: "Code", keyValues: new object[] { "APPROVE", "REJECT", "SEND_BACK" });
        }
    }
}
