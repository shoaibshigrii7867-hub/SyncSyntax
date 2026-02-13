using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SyncSyntax.Migrations
{
    /// <inheritdoc />
    public partial class seedingInitaldata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Technology" },
                    { 2, null, "Health" },
                    { 3, null, "Lifestyle" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Author", "CategoryId", "Content", "FutureImageURL", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Jane Doe", 1, "Artificial Intelligence (AI) continues to evolve at a rapid pace, influencing various sectors from healthcare to finance. In 2024, we can expect significant advancements in machine learning algorithms, natural language processing, and AI ethics. Businesses are increasingly adopting AI-driven solutions to enhance customer experiences and streamline operations. Additionally, the integration of AI with other emerging technologies like blockchain and IoT will open new avenues for innovation.", "https://example.com/images/ai-future.jpg", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Future of AI: Trends to Watch in 2024" },
                    { 2, "John Smith", 2, "In today's fast-paced digital world, maintaining mental health is more important than ever. Here are ten tips to help you stay balanced: 1) Set boundaries for screen time, 2) Practice mindfulness and meditation, 3) Stay connected with loved ones, 4) Engage in regular physical activity, 5) Prioritize sleep, 6) Limit exposure to negative news, 7) Seek professional help when needed, 8) Cultivate hobbies outside of technology, 9) Practice gratitude daily, and 10) Take regular breaks from social media.", "https://example.com/images/mental-health-tips.jpg", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10 Tips for Maintaining Mental Health in a Digital World" },
                    { 3, "Emily Green", 3, "Sustainable living is essential for preserving our planet for future generations. To reduce your carbon footprint, consider the following steps: 1) Use energy-efficient appliances, 2) Reduce, reuse, and recycle materials, 3) Opt for public transportation or carpooling, 4) Support local and organic food sources, 5) Minimize water usage, 6) Plant trees and maintain green spaces, 7) Choose renewable energy options when possible, and 8) Educate others about the importance of sustainability.", "https://example.com/images/sustainable-living.jpg", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sustainable Living: How to Reduce Your Carbon Footprint" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
