using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitTheBill.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseAndPaymentTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(1, 1, 1, 0, 0, 0, 0, TimeSpan.Zero));

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Expenses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(1, 1, 1, 0, 0, 0, 0, TimeSpan.Zero));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Expenses");
        }
    }
}
