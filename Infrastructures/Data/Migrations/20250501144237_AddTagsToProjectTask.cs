using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pmapp.Infrastructures.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsToProjectTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "ProjectTasks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "ProjectTasks");
        }
    }
}
