using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eshop.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProductTranslationCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTranslations_Languages_LanguageId",
                table: "ProductTranslations");

            migrationBuilder.DropIndex(
                name: "IX_ProductTranslations_LanguageId",
                table: "ProductTranslations");

            migrationBuilder.DropIndex(
                name: "IX_ProductTranslations_ProductId",
                table: "ProductTranslations");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "ProductTranslations");

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "ProductTranslations",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_ProductId_CountryCode",
                table: "ProductTranslations",
                columns: new[] { "ProductId", "LanguageCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductTranslations_ProductId_CountryCode",
                table: "ProductTranslations");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "ProductTranslations");

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "ProductTranslations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_LanguageId",
                table: "ProductTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_ProductId",
                table: "ProductTranslations",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTranslations_Languages_LanguageId",
                table: "ProductTranslations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
