using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizon.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedReferenceData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalHistories_Requests_RequestId",
                table: "ApprovalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStages_Departments_DepartmentId",
                table: "ApprovalStages");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStages_Positions_PositionId",
                table: "ApprovalStages");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStages_Roles_RoleId",
                table: "ApprovalStages");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceTransactions_Leaves_LeaveId",
                table: "BalanceTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveBalances_Employees_EmployeeId",
                table: "LeaveBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_LeaveStatuses_StatusId",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Requests_RequestId",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestStatuses_StatusId",
                table: "Requests");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccrualRate",
                table: "LeaveTypes",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DurationDays",
                table: "Leaves",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,1)",
                oldPrecision: 5,
                oldScale: 1);

            migrationBuilder.AlterColumn<decimal>(
                name: "Used",
                table: "LeaveBalances",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,1)",
                oldPrecision: 5,
                oldScale: 1,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Planned",
                table: "LeaveBalances",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,1)",
                oldPrecision: 5,
                oldScale: 1,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Entitled",
                table: "LeaveBalances",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,1)",
                oldPrecision: 5,
                oldScale: 1,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "FTE",
                table: "EmployeeDepartments",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 1.0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,2)",
                oldPrecision: 3,
                oldScale: 2,
                oldDefaultValue: 1.0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BalanceTransactions",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,1)",
                oldPrecision: 5,
                oldScale: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalHistories_Requests_RequestId",
                table: "ApprovalHistories",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalStages_Departments_DepartmentId",
                table: "ApprovalStages",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalStages_Positions_PositionId",
                table: "ApprovalStages",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalStages_Roles_RoleId",
                table: "ApprovalStages",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceTransactions_Leaves_LeaveId",
                table: "BalanceTransactions",
                column: "LeaveId",
                principalTable: "Leaves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveBalances_Employees_EmployeeId",
                table: "LeaveBalances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_LeaveStatuses_StatusId",
                table: "Leaves",
                column: "StatusId",
                principalTable: "LeaveStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Requests_RequestId",
                table: "Leaves",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestStatuses_StatusId",
                table: "Requests",
                column: "StatusId",
                principalTable: "RequestStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // ── Seed: RequestStatuses ─────────────────────────────────────────────
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "Code", "Name", "SortOrder", "CreatedAt", "CreatedBy", "IsActive" },
                values: new object[,]
                {
                    { "DRAFT",           "Черновик",                   1, seedDate, 1, true },
                    { "PENDING_MANAGER", "На согласовании (менеджер)", 2, seedDate, 1, true },
                    { "PENDING_HR",      "На согласовании (HR)",        3, seedDate, 1, true },
                    { "APPROVED",        "Утверждено",                  4, seedDate, 1, true },
                    { "REJECTED",        "Отклонено",                   5, seedDate, 1, true },
                    { "SENT_BACK",       "Возвращено на доработку",     6, seedDate, 1, true },
                }
            );

            // ── Seed: OperationTypes ──────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "OperationTypes",
                columns: new[] { "Code", "Name", "SortOrder", "CreatedAt", "CreatedBy", "IsActive" },
                values: new object[,]
                {
                    { "NEW_LEAVE",  "Новый отпуск",        1, seedDate, 1, true },
                    { "RESCHEDULE", "Перенос отпуска",     2, seedDate, 1, true },
                    { "CANCEL",     "Отзыв из отпуска",   3, seedDate, 1, true },
                    { "PLAN_YEAR",  "Планирование на год", 4, seedDate, 1, true },
                }
            );

            // ── Seed: LeaveTypes ──────────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Code", "Name", "IsPaid", "AffectsBalance", "MinDays", "MaxDays", "SortOrder", "CreatedAt", "CreatedBy", "IsActive" },
                values: new object[,]
                {
                    { "ANNUAL",    "Ежегодный оплачиваемый",  true,  true,  1, 28,  1, seedDate, 1, true },
                    { "SICK",      "Больничный",              true,  false, 1, 30,  2, seedDate, 1, true },
                    { "UNPAID",    "Без сохранения зарплаты", false, false, 1, 14,  3, seedDate, 1, true },
                    { "MATERNITY", "Декретный",               true,  false, 1, 140, 4, seedDate, 1, true },
                }
            );

            // ── Seed: ApprovalTemplates ───────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "ApprovalTemplates",
                columns: new[] { "Code", "Name", "Description", "SortOrder", "CreatedAt", "CreatedBy", "IsActive" },
                values: new object[,]
                {
                    { "STANDARD", "Стандартное согласование", "Менеджер → HR", 1, seedDate, 1, true },
                }
            );

            // ── Seed: ApprovalStages (resolve FK IDs by code) ─────────────────────
            migrationBuilder.Sql(@"
                INSERT INTO ""ApprovalStages"" (""TemplateId"", ""StageNumber"", ""RoleId"", ""StageName"", ""IsRequired"", ""CreatedAt"", ""CreatedBy"", ""IsActive"")
                SELECT t.""Id"", 1, r.""Id"", 'Согласование менеджером', TRUE, '2026-01-01 00:00:00', 1, TRUE
                FROM ""ApprovalTemplates"" t
                CROSS JOIN ""Roles"" r
                WHERE t.""Code"" = 'STANDARD' AND r.""Code"" = 'DepartmentManager';

                INSERT INTO ""ApprovalStages"" (""TemplateId"", ""StageNumber"", ""RoleId"", ""StageName"", ""IsRequired"", ""CreatedAt"", ""CreatedBy"", ""IsActive"")
                SELECT t.""Id"", 2, r.""Id"", 'Согласование HR', TRUE, '2026-01-01 00:00:00', 1, TRUE
                FROM ""ApprovalTemplates"" t
                CROSS JOIN ""Roles"" r
                WHERE t.""Code"" = 'STANDARD' AND r.""Code"" = 'HRManager';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ── Remove seed data ──────────────────────────────────────────────────
            migrationBuilder.Sql(@"
                DELETE FROM ""ApprovalStages""
                WHERE ""TemplateId"" IN (SELECT ""Id"" FROM ""ApprovalTemplates"" WHERE ""Code"" = 'STANDARD');
            ");
            migrationBuilder.DeleteData("ApprovalTemplates", "Code", "STANDARD");
            migrationBuilder.DeleteData("LeaveTypes",        "Code", new object[] { "ANNUAL", "SICK", "UNPAID", "MATERNITY" });
            migrationBuilder.DeleteData("OperationTypes",    "Code", new object[] { "NEW_LEAVE", "RESCHEDULE", "CANCEL", "PLAN_YEAR" });
            migrationBuilder.DeleteData("RequestStatuses",   "Code", new object[] { "DRAFT", "PENDING_MANAGER", "PENDING_HR", "APPROVED", "REJECTED", "SENT_BACK" });
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalHistories_Requests_RequestId",
                table: "ApprovalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStages_Departments_DepartmentId",
                table: "ApprovalStages");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStages_Positions_PositionId",
                table: "ApprovalStages");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStages_Roles_RoleId",
                table: "ApprovalStages");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceTransactions_Leaves_LeaveId",
                table: "BalanceTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveBalances_Employees_EmployeeId",
                table: "LeaveBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_LeaveStatuses_StatusId",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Requests_RequestId",
                table: "Leaves");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestStatuses_StatusId",
                table: "Requests");

            migrationBuilder.AlterColumn<decimal>(
                name: "AccrualRate",
                table: "LeaveTypes",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DurationDays",
                table: "Leaves",
                type: "numeric(5,1)",
                precision: 5,
                scale: 1,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Used",
                table: "LeaveBalances",
                type: "numeric(5,1)",
                precision: 5,
                scale: 1,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Planned",
                table: "LeaveBalances",
                type: "numeric(5,1)",
                precision: 5,
                scale: 1,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Entitled",
                table: "LeaveBalances",
                type: "numeric(5,1)",
                precision: 5,
                scale: 1,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "FTE",
                table: "EmployeeDepartments",
                type: "numeric(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 1.0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldDefaultValue: 1.0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "BalanceTransactions",
                type: "numeric(5,1)",
                precision: 5,
                scale: 1,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalHistories_Requests_RequestId",
                table: "ApprovalHistories",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalStages_Departments_DepartmentId",
                table: "ApprovalStages",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalStages_Positions_PositionId",
                table: "ApprovalStages",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalStages_Roles_RoleId",
                table: "ApprovalStages",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceTransactions_Leaves_LeaveId",
                table: "BalanceTransactions",
                column: "LeaveId",
                principalTable: "Leaves",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveBalances_Employees_EmployeeId",
                table: "LeaveBalances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_LeaveStatuses_StatusId",
                table: "Leaves",
                column: "StatusId",
                principalTable: "LeaveStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Requests_RequestId",
                table: "Leaves",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestStatuses_StatusId",
                table: "Requests",
                column: "StatusId",
                principalTable: "RequestStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
