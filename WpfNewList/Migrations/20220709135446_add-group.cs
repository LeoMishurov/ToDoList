using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfNewList.Migrations
{
    public partial class addgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupModelId",
                table: "ToDoModels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoModels_GroupModelId",
                table: "ToDoModels",
                column: "GroupModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoModels_GroupModel_GroupModelId",
                table: "ToDoModels",
                column: "GroupModelId",
                principalTable: "GroupModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoModels_GroupModel_GroupModelId",
                table: "ToDoModels");

            migrationBuilder.DropTable(
                name: "GroupModel");

            migrationBuilder.DropIndex(
                name: "IX_ToDoModels_GroupModelId",
                table: "ToDoModels");

            migrationBuilder.DropColumn(
                name: "GroupModelId",
                table: "ToDoModels");
        }
    }
}
