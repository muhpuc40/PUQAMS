using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PUQAMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_programs_SourceId",
                table: "programs");

            migrationBuilder.DropIndex(
                name: "IX_departments_SourceId",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "programs");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "departments");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "programs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "programs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "departments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "programs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "programs",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "programs",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "departments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "departments",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_programs_SourceId",
                table: "programs",
                column: "SourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departments_SourceId",
                table: "departments",
                column: "SourceId",
                unique: true);
        }
    }
}
