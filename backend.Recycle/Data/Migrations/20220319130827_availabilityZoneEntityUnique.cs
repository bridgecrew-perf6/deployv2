using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Recycle.Data.Migrations
{
    public partial class availabilityZoneEntityUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZoneName",
                table: "AvailabilityZones",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityZones_PostCode_ZoneName",
                table: "AvailabilityZones",
                columns: new[] { "PostCode", "ZoneName" },
                unique: true,
                filter: "[ZoneName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AvailabilityZones_PostCode_ZoneName",
                table: "AvailabilityZones");

            migrationBuilder.AlterColumn<string>(
                name: "ZoneName",
                table: "AvailabilityZones",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
