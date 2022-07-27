using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibTest.Migrations
{
    public partial class refactoringDbDistribution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanOfBooks");

            migrationBuilder.CreateTable(
                name: "DistributionOfBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsReturned = table.Column<bool>(type: "bit", nullable: false),
                    DateExtradition = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateReturn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    PersonalDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionOfBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributionOfBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributionOfBooks_PersonalData_PersonalDataId",
                        column: x => x.PersonalDataId,
                        principalTable: "PersonalData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributionOfBooks_BookId",
                table: "DistributionOfBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributionOfBooks_PersonalDataId",
                table: "DistributionOfBooks",
                column: "PersonalDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributionOfBooks");

            migrationBuilder.CreateTable(
                name: "LoanOfBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsReturned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanOfBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanOfBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanOfBooks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfBooks_BookId",
                table: "LoanOfBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfBooks_UserId",
                table: "LoanOfBooks",
                column: "UserId");
        }
    }
}
