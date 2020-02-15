using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRent.Source.Migrations
{
    public partial class RelationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarCategories_CategoryId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_CategoryId",
                table: "CarModels");

            migrationBuilder.AddColumn<Guid>(
                name: "CarCategoryId",
                table: "CarModels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_CarCategoryId",
                table: "CarModels",
                column: "CarCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarCategories_CarCategoryId",
                table: "CarModels",
                column: "CarCategoryId",
                principalTable: "CarCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarCategories_CarCategoryId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_CarCategoryId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "CarCategoryId",
                table: "CarModels");

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_CategoryId",
                table: "CarModels",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarCategories_CategoryId",
                table: "CarModels",
                column: "CategoryId",
                principalTable: "CarCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
