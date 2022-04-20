using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Recycle.Data.Migrations
{
    public partial class modifyAvailabilityEmployeeUniqueKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AvailabilityEmployee_EmployeeId",
                table: "AvailabilityEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityEmployee_EmployeeId_AvailabilityZoneId",
                table: "AvailabilityEmployee",
                columns: new[] { "EmployeeId", "AvailabilityZoneId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AvailabilityEmployee_EmployeeId_AvailabilityZoneId",
                table: "AvailabilityEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityEmployee_EmployeeId",
                table: "AvailabilityEmployee",
                column: "EmployeeId");
        }
    }
}
