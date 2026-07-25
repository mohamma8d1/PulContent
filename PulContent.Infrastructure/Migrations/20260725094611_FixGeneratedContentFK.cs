using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixGeneratedContentFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneratedContents_ProcessingJobs_ProcessingJobId",
                table: "GeneratedContents");

            migrationBuilder.DropIndex(
                name: "IX_GeneratedContents_ProcessingJobId",
                table: "GeneratedContents");

            migrationBuilder.DropColumn(
                name: "ProcessingJobId",
                table: "GeneratedContents");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedContents_JobId",
                table: "GeneratedContents",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneratedContents_ProcessingJobs_JobId",
                table: "GeneratedContents",
                column: "JobId",
                principalTable: "ProcessingJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneratedContents_ProcessingJobs_JobId",
                table: "GeneratedContents");

            migrationBuilder.DropIndex(
                name: "IX_GeneratedContents_JobId",
                table: "GeneratedContents");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessingJobId",
                table: "GeneratedContents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedContents_ProcessingJobId",
                table: "GeneratedContents",
                column: "ProcessingJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneratedContents_ProcessingJobs_ProcessingJobId",
                table: "GeneratedContents",
                column: "ProcessingJobId",
                principalTable: "ProcessingJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
