using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pmapp.Infrastructures.Data.Migrations
{
    /// <inheritdoc />
    public partial class addprogresstask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "ProjectTasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "ProjectTasks");
        }
    }
}
