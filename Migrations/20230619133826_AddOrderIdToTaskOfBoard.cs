using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POC_TaskBoard.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIdToTaskOfBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Tasks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Tasks");
        }
    }
}
