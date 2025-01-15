using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraceLink.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagDetails",
                columns: table => new
                {
                    TaggingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChasisNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceIMEI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfPanicButton = table.Column<int>(type: "int", nullable: false),
                    TaggingValidity = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MobileNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagDetails", x => x.TaggingId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagDetails");
        }
    }
}
