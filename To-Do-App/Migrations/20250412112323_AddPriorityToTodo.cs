using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace To_Do_App.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityToTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "ToDos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ToDos");
        }
    }
}
