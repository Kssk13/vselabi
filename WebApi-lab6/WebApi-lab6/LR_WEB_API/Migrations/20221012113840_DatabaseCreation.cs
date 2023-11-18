using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LR_WEB_API.Migrations
{
    public partial class DatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    WarehiusesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoodName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Count = table.Column<int>(type: "int", maxLength: 60, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehiusesId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrdersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Goods = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<long>(type: "bigint", maxLength: 20, nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrdersId);
                    table.ForeignKey(
                        name: "FK_Orders_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "WarehiusesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "312 Forest Avenue, BF 923", "USA", "Admin_Solutions Ltd" },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "583 Wall Dr. Gwynn Oak, MD 21207", "USA", "IT_Solutions Ltd" }
                });

            migrationBuilder.InsertData(
                table: "Warehouse",
                columns: new[] { "WarehiusesId", "Count", "GoodName", "Price" },
                values: new object[,]
                {
                    { new Guid("713a847a-2875-469d-aefb-fd7bb283a8d4"), 10, "Diode PTFs Sal-Man 60w 5 strips on VAZ 2110)", 3790.0 },
                    { new Guid("8615e23f-2548-4ef7-a440-af6edc214fb0"), 4, "Tuning headlights for VAZ 2110", 13490.0 }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[,]
                {
                    { new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), 35, new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Kane Miller", "Administrator" },
                    { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), 26, new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Sam Raiden", "Software developer" },
                    { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), 30, new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Jana McLeaf", "Software developer" }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "OrdersId", "Cost", "Date", "Goods", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("86abbca8-664d-4b20-b5de-024705497d4a"), 20.100000000000001, 12102022L, "Каркасные шторки сетки на передние стекла ВАЗ (Лада)", new Guid("8615e23f-2548-4ef7-a440-af6edc214fb0") },
                    { new Guid("87abbca8-664d-4b20-b5de-024705497d4a"), 203.09999999999999, 12102022L, "Каркасные шторки сетки на передние стекла ВАЗ (Лада)", new Guid("8615e23f-2548-4ef7-a440-af6edc214fb0") },
                    { new Guid("88abbca8-664d-4b20-b5de-024705497d4a"), 210.09999999999999, 12102022L, "Каркасные шторки сетки на передние стекла ВАЗ (Лада)", new Guid("713a847a-2875-469d-aefb-fd7bb283a8d4") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WarehouseId",
                table: "Order",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Warehouse");
        }
    }
}
