using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingRecommender.Data.Migrations
{
    public partial class TrainingAbout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Training",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "Training");
        }
    }
}
