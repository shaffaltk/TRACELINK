using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraceLink.Migrations
{
    /// <inheritdoc />
    public partial class TAgNAmuber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TagNumber",
                table: "TaggingRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagNumber",
                table: "TaggingRequests");
        }
    }
}
