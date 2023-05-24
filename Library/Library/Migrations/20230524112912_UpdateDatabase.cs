using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookImages_Books_BookId",
                table: "bookImages");

            migrationBuilder.DropTable(
                name: "bookGenres");

            migrationBuilder.DropTable(
                name: "literaryGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookImages",
                table: "bookImages");

            migrationBuilder.RenameTable(
                name: "bookImages",
                newName: "BookImages");

            migrationBuilder.RenameIndex(
                name: "IX_bookImages_BookId",
                table: "BookImages",
                newName: "IX_BookImages_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookImages",
                table: "BookImages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LiteraryGenders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteraryGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookGenders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LiteraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookGenders_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookGenders_LiteraryGenders_LiteraryId",
                        column: x => x.LiteraryId,
                        principalTable: "LiteraryGenders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookGenders_BookId",
                table: "BookGenders",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenders_LiteraryId",
                table: "BookGenders",
                column: "LiteraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LiteraryGenders_Name",
                table: "LiteraryGenders",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookImages_Books_BookId",
                table: "BookImages",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookImages_Books_BookId",
                table: "BookImages");

            migrationBuilder.DropTable(
                name: "BookGenders");

            migrationBuilder.DropTable(
                name: "LiteraryGenders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookImages",
                table: "BookImages");

            migrationBuilder.RenameTable(
                name: "BookImages",
                newName: "bookImages");

            migrationBuilder.RenameIndex(
                name: "IX_BookImages_BookId",
                table: "bookImages",
                newName: "IX_bookImages_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookImages",
                table: "bookImages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "literaryGenres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_literaryGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bookGenres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LiteraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookGenres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookGenres_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_bookGenres_literaryGenres_LiteraryId",
                        column: x => x.LiteraryId,
                        principalTable: "literaryGenres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookGenres_BookId",
                table: "bookGenres",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_bookGenres_LiteraryId",
                table: "bookGenres",
                column: "LiteraryId");

            migrationBuilder.CreateIndex(
                name: "IX_literaryGenres_Name",
                table: "literaryGenres",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bookImages_Books_BookId",
                table: "bookImages",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
