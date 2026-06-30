using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizon.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Code", "Name", "Description", "SortOrder", "CreatedAt", "CreatedBy", "IsActive" },
                values: new object[,]
                {
                    { "Admin",             "Администратор",       "Полный доступ к системе",                      1, now, 1, true },
                    { "HRManager",         "HR-менеджер",         "Управление отпусками и кадровыми данными",     2, now, 1, true },
                    { "DepartmentManager", "Руководитель отдела", "Согласование заявок сотрудников своего отдела",3, now, 1, true },
                    { "Employee",          "Сотрудник",           "Создание и просмотр своих заявок",             4, now, 1, true },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Code",
                keyValues: new object[] { "Admin", "HRManager", "DepartmentManager", "Employee" }
            );
        }
    }
}
