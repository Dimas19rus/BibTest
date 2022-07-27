using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibTest.Migrations
{
    public partial class addItemDistribution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DistributionOfBooks",
                columns: new[] { "Id", "BookId", "DateExtradition", "DateReturn", "IsReturned", "PersonalDataId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 7, 18, 18, 58, 15, 571, DateTimeKind.Local).AddTicks(1609), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1 },
                    { 2, 2, new DateTime(2022, 7, 18, 18, 58, 15, 571, DateTimeKind.Local).AddTicks(1623), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1 },
                    { 3, 3, new DateTime(2022, 7, 18, 18, 58, 15, 571, DateTimeKind.Local).AddTicks(1625), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2 },
                    { 4, 4, new DateTime(2015, 7, 20, 18, 30, 25, 0, DateTimeKind.Unspecified), new DateTime(2016, 7, 20, 18, 30, 25, 0, DateTimeKind.Unspecified), true, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DistributionOfBooks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DistributionOfBooks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DistributionOfBooks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DistributionOfBooks",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
