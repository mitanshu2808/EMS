using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Data.Migrations
{
    public partial class AddEventsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "224047d7-9c97-4768-a0f6-50e7eec9fe29",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c09aa32-38b0-4b52-94f4-cd24821dbf00", "AQAAAAIAAYagAAAAEM0pJS4AnjMR9kcTtwLX8NzeVqZBbA7rGnRC7zMeA7Wk5Jn+FQL9HxAZPzLeVbvwWg==", "655cf28a-70de-4e6e-ad7d-614440948671" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3f72a68b-1d4e-4c7b-9f0a-5e3d8c2e6b59",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8df8d82c-e6d1-4733-82b0-4aceb7086e9a", "AQAAAAIAAYagAAAAEIEiD6MEnF2KcM5zunloqrsFyllw1iWQQKOVKHyx0F1zKRvNB085pe5W6fg/vruLlw==", "b5fd6756-f6a7-4625-91bb-30773a5a85d2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d8545447-30d8-4910-8931-b5c22e47f8ce",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "de998d7e-9062-47ff-89f4-853940983f93", "AQAAAAIAAYagAAAAED07BtHLlVuSzd0RUJMc78g2SkQkiUmjdTINxdKHIXqgqCg2zyqjIdOr2khQnEhNZQ==", "6ed8885d-e4a6-45c1-82a2-723fb28f4af9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "224047d7-9c97-4768-a0f6-50e7eec9fe29",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f6265b40-aa7f-44d7-b47e-d9a13838881e", "AQAAAAIAAYagAAAAEMljsd48oE17mP9IEECbkodM+2PRSWPOPTmlC05nd2SOzShkTWo7IpQYKJV5WlqnBg==", "7c62d4b7-c419-4056-9427-74bbd0731d01" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3f72a68b-1d4e-4c7b-9f0a-5e3d8c2e6b59",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d452441-ece7-46ea-be2f-9e2b0d38a039", "AQAAAAIAAYagAAAAEH9/sJgG+bs+0V8/Wfa07mQwQoTuUl+zIjcFE4FIgm3j2+5oYQt1EURb74uEa8WWJg==", "3efee652-2ef8-4f67-8d24-fd0e3626d5b4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d8545447-30d8-4910-8931-b5c22e47f8ce",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5312beaf-4494-427b-9b00-24b92c9fd03a", "AQAAAAIAAYagAAAAEMpCrn4xLx4QKdjIYDHP3LIrmmCZjOvPtelMw33efrCcplCYoWfOjSaEfhIn7RCxkw==", "974003fc-e138-4598-be4f-8320098d2ad8" });
        }
    }
}
