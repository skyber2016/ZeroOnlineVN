using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum_API.Migrations
{
    public partial class announcement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryEntityTopicEntity");

            migrationBuilder.DropColumn(
                name: "AllwayShow",
                table: "Post");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnnouncement",
                table: "Topic",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Topic_CategoryId",
                table: "Topic",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Category_CategoryId",
                table: "Topic",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Category_CategoryId",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_CategoryId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "IsAnnouncement",
                table: "Topic");

            migrationBuilder.RenameColumn(
                name: "IsPinned",
                table: "Category",
                newName: "IsPin");

            migrationBuilder.AddColumn<bool>(
                name: "AllwayShow",
                table: "Post",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CategoryEntityTopicEntity",
                columns: table => new
                {
                    CategoriesId = table.Column<long>(type: "bigint", nullable: false),
                    TopicsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntityTopicEntity", x => new { x.CategoriesId, x.TopicsId });
                    table.ForeignKey(
                        name: "FK_CategoryEntityTopicEntity_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryEntityTopicEntity_Topic_TopicsId",
                        column: x => x.TopicsId,
                        principalTable: "Topic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryEntityTopicEntity_TopicsId",
                table: "CategoryEntityTopicEntity",
                column: "TopicsId");
        }
    }
}
