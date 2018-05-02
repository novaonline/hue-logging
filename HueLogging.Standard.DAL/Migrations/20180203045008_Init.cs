using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HueLogging.Standard.DAL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HueConfigStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddDate = table.Column<DateTime>(nullable: false),
                    IpAddress = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HueConfigStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Light",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HueType = table.Column<string>(nullable: true),
                    ModelId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SWVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Light", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Brightness = table.Column<short>(nullable: false),
                    Hue = table.Column<short>(nullable: false),
                    On = table.Column<bool>(nullable: false),
                    Reachable = table.Column<bool>(nullable: false),
                    Saturation = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LightEvent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddDate = table.Column<DateTime>(nullable: false),
                    LightId = table.Column<string>(nullable: true),
                    StateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LightEvent_Light_LightId",
                        column: x => x.LightId,
                        principalTable: "Light",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LightEvent_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HueConfigStates_AddDate",
                table: "HueConfigStates",
                column: "AddDate");

            migrationBuilder.CreateIndex(
                name: "IX_LightEvent_AddDate",
                table: "LightEvent",
                column: "AddDate");

            migrationBuilder.CreateIndex(
                name: "IX_LightEvent_LightId",
                table: "LightEvent",
                column: "LightId");

            migrationBuilder.CreateIndex(
                name: "IX_LightEvent_StateId",
                table: "LightEvent",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HueConfigStates");

            migrationBuilder.DropTable(
                name: "LightEvent");

            migrationBuilder.DropTable(
                name: "Light");

            migrationBuilder.DropTable(
                name: "State");
        }
    }
}
