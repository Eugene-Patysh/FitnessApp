using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Pablo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NutrientCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreatingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nutrients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DailyDose = table.Column<double>(type: "double precision", nullable: false),
                    NutrientCategoryId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nutrients_NutrientCategories_NutrientCategoryId",
                        column: x => x.NutrientCategoryId,
                        principalTable: "NutrientCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ProductCategoryId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSubCategories_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ProductSubCategoryId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductSubCategories_ProductSubCategoryId",
                        column: x => x.ProductSubCategoryId,
                        principalTable: "ProductSubCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductNutrients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quality = table.Column<double>(type: "double precision", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    NutrientId = table.Column<int>(type: "integer", nullable: true),
                    TreatingTypeId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductNutrients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductNutrients_Nutrients_NutrientId",
                        column: x => x.NutrientId,
                        principalTable: "Nutrients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductNutrients_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductNutrients_TreatingTypes_TreatingTypeId",
                        column: x => x.TreatingTypeId,
                        principalTable: "TreatingTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "NutrientCategories",
                columns: new[] { "Id", "Created", "Title", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4825), "Macronutrients", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4826) },
                    { 2, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4827), "Minerals", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4828) }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "Created", "Title", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4594), "Fruits", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4610) },
                    { 2, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4612), "Vegetables", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4613) }
                });

            migrationBuilder.InsertData(
                table: "TreatingTypes",
                columns: new[] { "Id", "Created", "Title", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4870), "Fresh", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4871) },
                    { 2, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4873), "Fried", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4874) }
                });

            migrationBuilder.InsertData(
                table: "Nutrients",
                columns: new[] { "Id", "Created", "DailyDose", "NutrientCategoryId", "Title", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4844), 0.75, 1, "Protein", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4845) },
                    { 2, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4848), 0.90000000000000002, 2, "Сalcium", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4849) }
                });

            migrationBuilder.InsertData(
                table: "ProductSubCategories",
                columns: new[] { "Id", "Created", "ProductCategoryId", "Title", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4781), 1, "Exotic", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4782) },
                    { 2, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4784), 2, "Tuberous", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4785) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Created", "ProductSubCategoryId", "Title", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4803), 1, "Banana", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4804) },
                    { 2, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4806), 2, "Potato", new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4806) }
                });

            migrationBuilder.InsertData(
                table: "ProductNutrients",
                columns: new[] { "Id", "Created", "NutrientId", "ProductId", "Quality", "TreatingTypeId", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4890), 1, 1, 0.80000000000000004, 1, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4891) },
                    { 2, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4894), 2, 2, 0.90000000000000002, 2, new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4895) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_NutrientCategoryId",
                table: "Nutrients",
                column: "NutrientCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductNutrients_NutrientId",
                table: "ProductNutrients",
                column: "NutrientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductNutrients_ProductId",
                table: "ProductNutrients",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductNutrients_TreatingTypeId",
                table: "ProductNutrients",
                column: "TreatingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSubCategoryId",
                table: "Products",
                column: "ProductSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_ProductCategoryId",
                table: "ProductSubCategories",
                column: "ProductCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductNutrients");

            migrationBuilder.DropTable(
                name: "Nutrients");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "TreatingTypes");

            migrationBuilder.DropTable(
                name: "NutrientCategories");

            migrationBuilder.DropTable(
                name: "ProductSubCategories");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
