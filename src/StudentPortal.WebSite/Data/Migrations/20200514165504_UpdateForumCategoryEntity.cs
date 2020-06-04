using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace StudentPortal.WebSite.Data.Migrations
{
    public partial class UpdateForumCategoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forum_ForumCategory_ForumCategoryName",
                table: "Forum");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessage_AspNetUsers_CreatorId",
                table: "ForumMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessage_ForumTopic_ForumTopicId",
                table: "ForumMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessageAttachment_ForumMessage_ForumMessageId",
                table: "ForumMessageAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopic_AspNetUsers_CreatorId",
                table: "ForumTopic");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopic_Forum_ForumId",
                table: "ForumTopic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumTopic",
                table: "ForumTopic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumMessageAttachment",
                table: "ForumMessageAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumMessage",
                table: "ForumMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumCategory",
                table: "ForumCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forum",
                table: "Forum");

            migrationBuilder.DropIndex(
                name: "IX_Forum_ForumCategoryName",
                table: "Forum");

            migrationBuilder.DropColumn(
                name: "ForumCategoryName",
                table: "Forum");

            migrationBuilder.RenameTable(
                name: "ForumTopic",
                newName: "ForumTopics");

            migrationBuilder.RenameTable(
                name: "ForumMessageAttachment",
                newName: "ForumMessageAttachments");

            migrationBuilder.RenameTable(
                name: "ForumMessage",
                newName: "forumMessages");

            migrationBuilder.RenameTable(
                name: "ForumCategory",
                newName: "ForumCategories");

            migrationBuilder.RenameTable(
                name: "Forum",
                newName: "Forums");

            migrationBuilder.RenameIndex(
                name: "IX_ForumTopic_ForumId",
                table: "ForumTopics",
                newName: "IX_ForumTopics_ForumId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumTopic_CreatorId",
                table: "ForumTopics",
                newName: "IX_ForumTopics_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessageAttachment_ForumMessageId",
                table: "ForumMessageAttachments",
                newName: "IX_ForumMessageAttachments_ForumMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessage_ForumTopicId",
                table: "forumMessages",
                newName: "IX_forumMessages_ForumTopicId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessage_CreatorId",
                table: "forumMessages",
                newName: "IX_forumMessages_CreatorId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ForumCategories",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ForumCategories",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "ForumCategoryId",
                table: "Forums",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumTopics",
                table: "ForumTopics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumMessageAttachments",
                table: "ForumMessageAttachments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_forumMessages",
                table: "forumMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumCategories",
                table: "ForumCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forums",
                table: "Forums",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_ForumCategoryId",
                table: "Forums",
                column: "ForumCategoryId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Forums_ForumCategories_ForumCategoryId",
                table: "Forums",
                column: "ForumCategoryId",
                principalTable: "ForumCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_Forums_ForumId",
                table: "ForumTopics",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Forums_ForumCategories_ForumCategoryId",
                table: "Forums");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_Forums_ForumId",
                table: "ForumTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumTopics",
                table: "ForumTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forums",
                table: "Forums");

            migrationBuilder.DropIndex(
                name: "IX_Forums_ForumCategoryId",
                table: "Forums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_forumMessages",
                table: "forumMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumMessageAttachments",
                table: "ForumMessageAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumCategories",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "ForumCategoryId",
                table: "Forums");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ForumCategories");

            migrationBuilder.RenameTable(
                name: "ForumTopics",
                newName: "ForumTopic");

            migrationBuilder.RenameTable(
                name: "Forums",
                newName: "Forum");

            migrationBuilder.RenameTable(
                name: "forumMessages",
                newName: "ForumMessage");

            migrationBuilder.RenameTable(
                name: "ForumMessageAttachments",
                newName: "ForumMessageAttachment");

            migrationBuilder.RenameTable(
                name: "ForumCategories",
                newName: "ForumCategory");

            migrationBuilder.RenameIndex(
                name: "IX_ForumTopics_ForumId",
                table: "ForumTopic",
                newName: "IX_ForumTopic_ForumId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumTopics_CreatorId",
                table: "ForumTopic",
                newName: "IX_ForumTopic_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_forumMessages_ForumTopicId",
                table: "ForumMessage",
                newName: "IX_ForumMessage_ForumTopicId");

            migrationBuilder.RenameIndex(
                name: "IX_forumMessages_CreatorId",
                table: "ForumMessage",
                newName: "IX_ForumMessage_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessageAttachments_ForumMessageId",
                table: "ForumMessageAttachment",
                newName: "IX_ForumMessageAttachment_ForumMessageId");

            migrationBuilder.AddColumn<string>(
                name: "ForumCategoryName",
                table: "Forum",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ForumCategory",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumTopic",
                table: "ForumTopic",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forum",
                table: "Forum",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumMessage",
                table: "ForumMessage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumMessageAttachment",
                table: "ForumMessageAttachment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumCategory",
                table: "ForumCategory",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Forum_ForumCategoryName",
                table: "Forum",
                column: "ForumCategoryName");

            migrationBuilder.AddForeignKey(
                name: "FK_Forum_ForumCategory_ForumCategoryName",
                table: "Forum",
                column: "ForumCategoryName",
                principalTable: "ForumCategory",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessage_AspNetUsers_CreatorId",
                table: "ForumMessage",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessage_ForumTopic_ForumTopicId",
                table: "ForumMessage",
                column: "ForumTopicId",
                principalTable: "ForumTopic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessageAttachment_ForumMessage_ForumMessageId",
                table: "ForumMessageAttachment",
                column: "ForumMessageId",
                principalTable: "ForumMessage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopic_AspNetUsers_CreatorId",
                table: "ForumTopic",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopic_Forum_ForumId",
                table: "ForumTopic",
                column: "ForumId",
                principalTable: "Forum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
