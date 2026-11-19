using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyVaccine.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class My_Vaccine_Migration_II : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyGroupUser_FamilyGroups_FamilyGroupsId",
                table: "FamilyGroupUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyGroupUser_Users_UsersId",
                table: "FamilyGroupUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyGroupUser",
                table: "FamilyGroupUser");

            migrationBuilder.RenameTable(
                name: "FamilyGroupUser",
                newName: "UserFamilyGroups");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyGroupUser_UsersId",
                table: "UserFamilyGroups",
                newName: "IX_UserFamilyGroups_UsersId");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFamilyGroups",
                table: "UserFamilyGroups",
                columns: new[] { "FamilyGroupsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFamilyGroups_FamilyGroups_FamilyGroupsId",
                table: "UserFamilyGroups",
                column: "FamilyGroupsId",
                principalTable: "FamilyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFamilyGroups_Users_UsersId",
                table: "UserFamilyGroups",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFamilyGroups_FamilyGroups_FamilyGroupsId",
                table: "UserFamilyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFamilyGroups_Users_UsersId",
                table: "UserFamilyGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFamilyGroups",
                table: "UserFamilyGroups");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UserFamilyGroups",
                newName: "FamilyGroupUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserFamilyGroups_UsersId",
                table: "FamilyGroupUser",
                newName: "IX_FamilyGroupUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyGroupUser",
                table: "FamilyGroupUser",
                columns: new[] { "FamilyGroupsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyGroupUser_FamilyGroups_FamilyGroupsId",
                table: "FamilyGroupUser",
                column: "FamilyGroupsId",
                principalTable: "FamilyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyGroupUser_Users_UsersId",
                table: "FamilyGroupUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
