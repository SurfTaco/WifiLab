using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class SeedSomeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Firstname", "Surname" },
                values: new object[,]
                {
                    { 1, "John Ronald Reuen", "Tolkien" },
                    { 2, "Joanne Kathleen", "Rowling" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Firstname", "IsAdmin", "RegistrationDate", "Surname" },
                values: new object[,]
                {
                    { 1, "my@testemail.at", "John", false, new DateTime(2022, 2, 6, 12, 24, 57, 290, DateTimeKind.Local).AddTicks(5590), "Trapper" },
                    { 2, "Johnboy@waltons.at", "Johnboy", false, new DateTime(2022, 2, 6, 12, 24, 57, 290, DateTimeKind.Local).AddTicks(5630), "Walton" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorID", "CoverPicture", "Name", "ReleaseDate" },
                values: new object[] { 1, 1, "https://images-na.ssl-images-amazon.com/images/I/71KG561kbYL.jpg", "The Hobbit or there and back again", new DateTime(1937, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorID", "CoverPicture", "Name", "ReleaseDate" },
                values: new object[] { 2, 2, "https://pictures.abebooks.com/isbn/9780545582926-us.jpg", "Harry Potter and the chamber of secrets", new DateTime(1998, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Rents",
                columns: new[] { "Id", "BookId", "CustomerId", "DateOfRent", "DateOfReturn" },
                values: new object[] { 1, 1, 1, new DateTime(2022, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Rents",
                columns: new[] { "Id", "BookId", "CustomerId", "DateOfRent", "DateOfReturn" },
                values: new object[] { 2, 2, 1, new DateTime(2022, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
