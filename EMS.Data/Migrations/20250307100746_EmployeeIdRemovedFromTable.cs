using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Data.Migrations
{
    public partial class EmployeeIdRemovedFromTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_EmployeeId",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "224047d7-9c97-4768-a0f6-50e7eec9fe29",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f6265b40-aa7f-44d7-b47e-d9a13838881e", "ADMIN@123.COM", "AQAAAAIAAYagAAAAEMljsd48oE17mP9IEECbkodM+2PRSWPOPTmlC05nd2SOzShkTWo7IpQYKJV5WlqnBg==", "7c62d4b7-c419-4056-9427-74bbd0731d01" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3f72a68b-1d4e-4c7b-9f0a-5e3d8c2e6b59",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d452441-ece7-46ea-be2f-9e2b0d38a039", "EMPLOYEE@123.COM", "AQAAAAIAAYagAAAAEH9/sJgG+bs+0V8/Wfa07mQwQoTuUl+zIjcFE4FIgm3j2+5oYQt1EURb74uEa8WWJg==", "3efee652-2ef8-4f67-8d24-fd0e3626d5b4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d8545447-30d8-4910-8931-b5c22e47f8ce",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5312beaf-4494-427b-9b00-24b92c9fd03a", "SUPERADMIN@123.COM", "AQAAAAIAAYagAAAAEMpCrn4xLx4QKdjIYDHP3LIrmmCZjOvPtelMw33efrCcplCYoWfOjSaEfhIn7RCxkw==", "974003fc-e138-4598-be4f-8320098d2ad8" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_EmployeeId",
                table: "Tasks",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_EmployeeId",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "224047d7-9c97-4768-a0f6-50e7eec9fe29",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "272c410f-4de1-48c4-b910-4cc3645ccc34", null, "AQAAAAIAAYagAAAAEAwmyi4Ea8kClHdLDX3gTDLQW7ApM/pKtU2uTdbO+Vy52HAdZHKktd19LjxCc+MEiA==", "cc12717d-4784-4331-96c6-fbed5e63f290" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3f72a68b-1d4e-4c7b-9f0a-5e3d8c2e6b59",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "647b61e9-0d68-4fd6-965a-589adbb8bf1f", null, "Test@123", "0ff268e8-5f61-423d-8eb5-cc94a0f80062" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d8545447-30d8-4910-8931-b5c22e47f8ce",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eab65971-4a48-4523-8845-557e4358ee99", null, "AQAAAAIAAYagAAAAECbfyW1zD81GW2I8UEzDxqv/6Mr1yKIHZVUK4NB3nrhvGJzZIikpjb+5r7aXu3ZjIw==", "66d86f45-acce-42ac-be89-c5c26ec4c8be" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_EmployeeId",
                table: "Tasks",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
