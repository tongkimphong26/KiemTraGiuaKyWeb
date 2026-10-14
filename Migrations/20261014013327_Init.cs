using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KiemTraGiuaKy.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BIT247667_DishCategories",
                columns: table => new
                {
                    BIT247667_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BIT247667_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BIT247667_Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIT247667_DishCategories", x => x.BIT247667_Id);
                });

            migrationBuilder.CreateTable(
                name: "BIT247667_Dishes",
                columns: table => new
                {
                    BIT247667_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BIT247667_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BIT247667_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BIT247667_PreparationTime = table.Column<int>(type: "int", nullable: false),
                    BIT247667_IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    BIT247667_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BIT247667_DishCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIT247667_Dishes", x => x.BIT247667_Id);
                    table.ForeignKey(
                        name: "FK_BIT247667_Dishes_BIT247667_DishCategories_BIT247667_DishCategoryId",
                        column: x => x.BIT247667_DishCategoryId,
                        principalTable: "BIT247667_DishCategories",
                        principalColumn: "BIT247667_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BIT247667_DishImages",
                columns: table => new
                {
                    BIT247667_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BIT247667_ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BIT247667_IsThumbnail = table.Column<bool>(type: "bit", nullable: false),
                    BIT247667_DishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BIT247667_DishImages", x => x.BIT247667_Id);
                    table.ForeignKey(
                        name: "FK_BIT247667_DishImages_BIT247667_Dishes_BIT247667_DishId",
                        column: x => x.BIT247667_DishId,
                        principalTable: "BIT247667_Dishes",
                        principalColumn: "BIT247667_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BIT247667_DishCategories",
                columns: new[] { "BIT247667_Id", "BIT247667_Description", "BIT247667_Name" },
                values: new object[,]
                {
                    { 1, "Main dishes", "Main" },
                    { 2, "Drinks", "Drink" },
                    { 3, "Desserts", "Dessert" }
                });

            migrationBuilder.InsertData(
                table: "BIT247667_Dishes",
                columns: new[] { "BIT247667_Id", "BIT247667_Description", "BIT247667_DishCategoryId", "BIT247667_IsAvailable", "BIT247667_Name", "BIT247667_PreparationTime", "BIT247667_Price" },
                values: new object[,]
                {
                    { 1, null, 1, true, "Pho", 20, 50000m },
                    { 2, null, 1, true, "Bun cha", 25, 45000m },
                    { 3, null, 2, true, "Tra da", 2, 5000m },
                    { 4, null, 3, true, "Che", 10, 20000m },
                    { 5, null, 2, false, "Cafe sua", 5, 25000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BIT247667_Dishes_BIT247667_DishCategoryId_BIT247667_Name",
                table: "BIT247667_Dishes",
                columns: new[] { "BIT247667_DishCategoryId", "BIT247667_Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BIT247667_DishImages_BIT247667_DishId",
                table: "BIT247667_DishImages",
                column: "BIT247667_DishId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BIT247667_DishImages");

            migrationBuilder.DropTable(
                name: "BIT247667_Dishes");

            migrationBuilder.DropTable(
                name: "BIT247667_DishCategories");
        }
    }
}
