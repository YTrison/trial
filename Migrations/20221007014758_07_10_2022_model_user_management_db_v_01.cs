using Microsoft.EntityFrameworkCore.Migrations;

namespace web_api_managemen_user.Migrations
{
    public partial class _07_10_2022_model_user_management_db_v_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ref1",
                table: "m_user",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ref2",
                table: "m_user",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ref3",
                table: "m_user",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ref1",
                table: "m_user");

            migrationBuilder.DropColumn(
                name: "ref2",
                table: "m_user");

            migrationBuilder.DropColumn(
                name: "ref3",
                table: "m_user");
        }
    }
}
