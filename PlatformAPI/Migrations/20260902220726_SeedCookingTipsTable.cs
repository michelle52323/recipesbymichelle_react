using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCookingTipsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CookingTips",
                columns: new[] { "Id", "Category", "Description", "IsActive", "Season" },
                values: new object[,]
                {
                    { 1, null, "Salt your pasta water until it tastes like the sea; it’s your only chance to season the noodles.", true, null },
                    { 2, null, "Let meat rest after cooking so the juices redistribute and the texture stays tender.", true, null },
                    { 3, null, "Toast spices in a dry pan for 30 seconds to unlock deeper aroma and flavor.", true, null },
                    { 4, null, "Add a splash of acid (lemon or vinegar) when a dish tastes flat — it brightens everything.", true, null },
                    { 5, null, "Preheat your pan fully before adding oil to prevent sticking and ensure even browning.", true, null },
                    { 6, null, "Cut ingredients to similar sizes so they cook evenly and finish at the same time.", true, null },
                    { 7, null, "Reserve some pasta water to adjust sauce consistency and help it cling to noodles.", true, null },
                    { 8, null, "Use room‑temperature butter and eggs for smoother baking batters and better rise.", true, null },
                    { 9, null, "Sear meat without moving it for the first minute — that’s how you get a proper crust.", true, null },
                    { 10, null, "Taste as you cook; seasoning gradually creates better balance than adding salt at the end.", true, null },
                    { 11, null, "Add aromatics (garlic, ginger, onions) after your oil heats to avoid burning and bitterness.", true, null },
                    { 12, null, "Chill cookie dough before baking to prevent spreading and deepen flavor.", true, null },
                    { 13, null, "Use a thermometer for meat and baked goods — guessing leads to overcooking.", true, null },
                    { 14, null, "Bloom cocoa powder in hot water or coffee to intensify chocolate flavor in desserts.", true, null },
                    { 15, null, "Deglaze your pan with wine, broth, or vinegar to create instant sauce and add depth.", true, null },
                    { 16, null, "Dry proteins with a paper towel before cooking — moisture prevents browning.", true, null },
                    { 17, null, "Add herbs at the right time: woody herbs early, delicate herbs at the end.", true, null },
                    { 18, null, "Don’t overcrowd the pan; steam builds and prevents proper browning.", true, null },
                    { 19, null, "Use unsalted butter in baking so you control the salt level precisely.", true, null },
                    { 20, null, "Warm tortillas before serving — heat improves texture and prevents cracking.", true, null },
                    { 21, null, "Add a pinch of sugar to tomato sauces to balance acidity without making it sweet.", true, null },
                    { 22, null, "Let rice rest for 5–10 minutes after cooking to finish steaming and improve fluffiness.", true, null },
                    { 23, null, "Use a microplane for garlic to create smoother sauces and dressings.", true, null },
                    { 24, null, "Stir‑fry ingredients in batches so each cooks properly and stays crisp.", true, null },
                    { 25, null, "Add cold butter at the end of pan sauces for a glossy finish and richer flavor.", true, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CookingTips",
                keyColumn: "Id",
                keyValue: 25);
        }
    }
}
