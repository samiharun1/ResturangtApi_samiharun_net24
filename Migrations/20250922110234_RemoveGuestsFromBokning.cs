using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResturangtApi_samiharun_net24.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGuestsFromBokning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guests",
                table: "Bokningar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Guests",
                table: "Bokningar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
