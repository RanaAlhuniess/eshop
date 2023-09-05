using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eshop.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProductTranslationLanguageCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountryCode",
                table: "ProductTranslations",
                newName: "LanguageCode");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTranslations_ProductId_CountryCode",
                table: "ProductTranslations",
                newName: "IX_ProductTranslations_ProductId_LanguageCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LanguageCode",
                table: "ProductTranslations",
                newName: "CountryCode");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTranslations_ProductId_LanguageCode",
                table: "ProductTranslations",
                newName: "IX_ProductTranslations_ProductId_CountryCode");
        }
    }
}
