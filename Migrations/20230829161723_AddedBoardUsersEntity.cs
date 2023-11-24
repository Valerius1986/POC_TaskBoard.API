using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POC_TaskBoard.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedBoardUsersEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardUsers_Bords_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Bords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardUsers_BoardId",
                table: "BoardUsers",
                column: "BoardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardUsers");
        }
    }
}
