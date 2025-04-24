using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CleanSetupWithSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Sales");
        }
    }
}
