using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Recycle.Data.Migrations
{
    public partial class AvailabilityEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailabilityEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvailabilityZoneId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailabilityEmployee_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailabilityEmployee_AvailabilityZones_AvailabilityZoneId",
                        column: x => x.AvailabilityZoneId,
                        principalTable: "AvailabilityZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityEmployee_AvailabilityZoneId",
                table: "AvailabilityEmployee",
                column: "AvailabilityZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityEmployee_EmployeeId",
                table: "AvailabilityEmployee",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailabilityEmployee");
        }
    }
}
