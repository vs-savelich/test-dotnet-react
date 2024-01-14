using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestDotnetReact.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name_Country",
                table: "Tenants",
                columns: new[] { "Name", "Country" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_Name_TenantId",
                table: "Portfolios",
                columns: new[] { "Name", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_Name_PortfolioId",
                table: "Plants",
                columns: new[] { "Name", "PortfolioId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_Name_Country",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Portfolios_Name_TenantId",
                table: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_Plants_Name_PortfolioId",
                table: "Plants");
        }
    }
}
