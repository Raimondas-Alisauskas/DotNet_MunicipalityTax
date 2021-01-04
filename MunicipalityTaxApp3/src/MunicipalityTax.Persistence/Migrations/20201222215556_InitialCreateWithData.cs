namespace MunicipalityTax.Persistence.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitialCreateWithData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Municipality",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MunicipalityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleType = table.Column<int>(type: "int", nullable: false),
                    TaxStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MunicipalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxSchedule_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Municipality",
                columns: new[] { "Id", "MunicipalityName" },
                values: new object[] { new Guid("7ebced2b-e2f9-45e0-bf75-111111111100"), "TestMunicipality" });

            migrationBuilder.InsertData(
                table: "TaxSchedule",
                columns: new[] { "Id", "MunicipalityId", "ScheduleType", "Tax", "TaxEndDate", "TaxStartDate" },
                values: new object[,]
                {
                    { new Guid("7ebced2b-e2f9-45e0-bf75-111111111113"), new Guid("7ebced2b-e2f9-45e0-bf75-111111111100"), 0, 0.1m, new DateTime(2016, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7ebced2b-e2f9-45e0-bf75-111111111111"), new Guid("7ebced2b-e2f9-45e0-bf75-111111111100"), 1, 0.2m, new DateTime(2016, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2015, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7ebced2b-e2f9-45e0-bf75-111111111112"), new Guid("7ebced2b-e2f9-45e0-bf75-111111111100"), 2, 0.3m, new DateTime(2016, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7ebced2b-e2f9-45e0-bf75-111111111114"), new Guid("7ebced2b-e2f9-45e0-bf75-111111111100"), 3, 0.4m, new DateTime(2016, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7ebced2b-e2f9-45e0-bf75-111111111115"), new Guid("7ebced2b-e2f9-45e0-bf75-111111111100"), 2, 0.5m, new DateTime(2016, 2, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxSchedule_MunicipalityId",
                table: "TaxSchedule",
                column: "MunicipalityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxSchedule");

            migrationBuilder.DropTable(
                name: "Municipality");
        }
    }
}
