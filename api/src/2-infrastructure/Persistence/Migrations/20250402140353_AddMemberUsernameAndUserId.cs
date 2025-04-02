using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitTheBill.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberUsernameAndUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:case_insensitive_collation", "en-u-ks-primary,en-u-ks-primary,icu,False");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Members",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                collation: "case_insensitive_collation");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Members",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                collation: "case_insensitive_collation");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_Username",
                table: "Members",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_UserId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_Username",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Members");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:CollationDefinition:case_insensitive_collation", "en-u-ks-primary,en-u-ks-primary,icu,False");
        }
    }
}
