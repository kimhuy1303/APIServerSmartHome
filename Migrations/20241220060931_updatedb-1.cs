using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServerSmartHome.Migrations
{
    /// <inheritdoc />
    public partial class updatedb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperatingTime",
                table: "Device");

            migrationBuilder.AddColumn<int>(
                name: "AccessLevel",
                table: "RFIDCard",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OperateTimeWorking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    OperatingTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperateTimeWorking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperateTimeWorking_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperateTimeWorking_DeviceId",
                table: "OperateTimeWorking",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperateTimeWorking");

            migrationBuilder.DropColumn(
                name: "AccessLevel",
                table: "RFIDCard");

            migrationBuilder.AddColumn<DateTime>(
                name: "OperatingTime",
                table: "Device",
                type: "datetime2",
                nullable: true);
        }
    }
}
