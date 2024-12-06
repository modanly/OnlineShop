using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Cost", "Description", "ImagePath", "Name" },
                values: new object[] { new Guid("68355daf-8834-4fa9-a145-5ae34fe45af0"), 1940m, "- Двойная защита швов\n- 4-точечная система блокировки замка", "/images/image3.jpeg", "Ошейник Zee.Dog Sand, XS, бежевый" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("68355daf-8834-4fa9-a145-5ae34fe45af0"));
        }
    }
}
