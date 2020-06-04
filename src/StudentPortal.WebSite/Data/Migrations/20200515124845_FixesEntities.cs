using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StudentPortal.WebSite.Data.Migrations
{
    public partial class FixesEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessageAttachments_forumMessages_ForumMessageId",
                table: "ForumMessageAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_forumMessages_AspNetUsers_CreatorId",
                table: "forumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_forumMessages_ForumTopics_ForumTopicId",
                table: "forumMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_forumMessages",
                table: "forumMessages");

            migrationBuilder.RenameTable(
                name: "forumMessages",
                newName: "ForumMessages");

            migrationBuilder.RenameIndex(
                name: "IX_forumMessages_ForumTopicId",
                table: "ForumMessages",
                newName: "IX_ForumMessages_ForumTopicId");

            migrationBuilder.RenameIndex(
                name: "IX_forumMessages_CreatorId",
                table: "ForumMessages",
                newName: "IX_ForumMessages_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumMessages",
                table: "ForumMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessageAttachments_ForumMessages_ForumMessageId",
                table: "ForumMessageAttachments",
                column: "ForumMessageId",
                principalTable: "ForumMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicId",
                table: "ForumMessages",
                column: "ForumTopicId",
                principalTable: "ForumTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessageAttachments_ForumMessages_ForumMessageId",
                table: "ForumMessageAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_AspNetUsers_CreatorId",
                table: "ForumMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessages_ForumTopics_ForumTopicId",
                table: "ForumMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumMessages",
                table: "ForumMessages");

            migrationBuilder.RenameTable(
                name: "ForumMessages",
                newName: "forumMessages");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessages_ForumTopicId",
                table: "forumMessages",
                newName: "IX_forumMessages_ForumTopicId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessages_CreatorId",
                table: "forumMessages",
                newName: "IX_forumMessages_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_forumMessages",
                table: "forumMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessageAttachments_forumMessages_ForumMessageId",
                table: "ForumMessageAttachments",
                column: "ForumMessageId",
                principalTable: "forumMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_forumMessages_AspNetUsers_CreatorId",
                table: "forumMessages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_forumMessages_ForumTopics_ForumTopicId",
                table: "forumMessages",
                column: "ForumTopicId",
                principalTable: "ForumTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
