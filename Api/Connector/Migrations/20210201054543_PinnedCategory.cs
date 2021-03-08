using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum_API.Migrations
{
    public partial class PinnedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllwayShow",
                table: "Post",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPin",
                table: "Category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedBy",
                table: "User",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_CreatedBy",
                table: "Topic",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SystemMessage_CreatedBy",
                table: "SystemMessage",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLog_CreatedBy",
                table: "SystemLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfig_CreatedBy",
                table: "SystemConfig",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ShoutBox_CreatedBy",
                table: "ShoutBox",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedBy",
                table: "Role",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_CreatedBy",
                table: "RefreshToken",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Post_CreatedBy",
                table: "Post",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_CreatedBy",
                table: "Notification",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_CreatedBy",
                table: "Menu",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MailQueue_CreatedBy",
                table: "MailQueue",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Function_CreatedBy",
                table: "Function",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CreatedBy",
                table: "Category",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Area_CreatedBy",
                table: "Area",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_User_CreatedBy",
                table: "Area",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_CreatedBy",
                table: "Category",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Function_User_CreatedBy",
                table: "Function",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MailQueue_User_CreatedBy",
                table: "MailQueue",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_User_CreatedBy",
                table: "Menu",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_CreatedBy",
                table: "Notification",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_CreatedBy",
                table: "Post",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_CreatedBy",
                table: "RefreshToken",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_User_CreatedBy",
                table: "Role",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoutBox_User_CreatedBy",
                table: "ShoutBox",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemConfig_User_CreatedBy",
                table: "SystemConfig",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemLog_User_CreatedBy",
                table: "SystemLog",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemMessage_User_CreatedBy",
                table: "SystemMessage",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_User_CreatedBy",
                table: "Topic",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_User_CreatedBy",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_CreatedBy",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Function_User_CreatedBy",
                table: "Function");

            migrationBuilder.DropForeignKey(
                name: "FK_MailQueue_User_CreatedBy",
                table: "MailQueue");

            migrationBuilder.DropForeignKey(
                name: "FK_Menu_User_CreatedBy",
                table: "Menu");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_CreatedBy",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_CreatedBy",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_CreatedBy",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_User_CreatedBy",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoutBox_User_CreatedBy",
                table: "ShoutBox");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemConfig_User_CreatedBy",
                table: "SystemConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemLog_User_CreatedBy",
                table: "SystemLog");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemMessage_User_CreatedBy",
                table: "SystemMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_User_CreatedBy",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Topic_CreatedBy",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_SystemMessage_CreatedBy",
                table: "SystemMessage");

            migrationBuilder.DropIndex(
                name: "IX_SystemLog_CreatedBy",
                table: "SystemLog");

            migrationBuilder.DropIndex(
                name: "IX_SystemConfig_CreatedBy",
                table: "SystemConfig");

            migrationBuilder.DropIndex(
                name: "IX_ShoutBox_CreatedBy",
                table: "ShoutBox");

            migrationBuilder.DropIndex(
                name: "IX_Role_CreatedBy",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_CreatedBy",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Post_CreatedBy",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Notification_CreatedBy",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Menu_CreatedBy",
                table: "Menu");

            migrationBuilder.DropIndex(
                name: "IX_MailQueue_CreatedBy",
                table: "MailQueue");

            migrationBuilder.DropIndex(
                name: "IX_Function_CreatedBy",
                table: "Function");

            migrationBuilder.DropIndex(
                name: "IX_Category_CreatedBy",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Area_CreatedBy",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "AllwayShow",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "IsPin",
                table: "Category");
        }
    }
}
