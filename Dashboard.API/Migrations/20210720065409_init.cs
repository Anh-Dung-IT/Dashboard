using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dashboard.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Project = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Layouts",
                columns: table => new
                {
                    LayoutsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Column = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.LayoutsId);
                });

            migrationBuilder.CreateTable(
                name: "WidgetTypes",
                columns: table => new
                {
                    WidgetTypesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WidgetTypesName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetTypes", x => x.WidgetTypesId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevorked = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Accounts_Username",
                        column: x => x.Username,
                        principalTable: "Accounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dashboards",
                columns: table => new
                {
                    DashboardsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    LayoutsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboards", x => x.DashboardsId);
                    table.ForeignKey(
                        name: "FK_Dashboards_Accounts_Username",
                        column: x => x.Username,
                        principalTable: "Accounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dashboards_Layouts_LayoutsId",
                        column: x => x.LayoutsId,
                        principalTable: "Layouts",
                        principalColumn: "LayoutsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Widgets",
                columns: table => new
                {
                    WidgetsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MinWidth = table.Column<int>(type: "int", nullable: false),
                    MinHeight = table.Column<int>(type: "int", nullable: false),
                    WidgetTypesId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DashboardsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Widgets", x => x.WidgetsId);
                    table.ForeignKey(
                        name: "FK_Widgets_Dashboards_DashboardsId",
                        column: x => x.DashboardsId,
                        principalTable: "Dashboards",
                        principalColumn: "DashboardsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Widgets_WidgetTypes_WidgetTypesId",
                        column: x => x.WidgetTypesId,
                        principalTable: "WidgetTypes",
                        principalColumn: "WidgetTypesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    WidgetsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TasksId);
                    table.ForeignKey(
                        name: "FK_Tasks_Accounts_Username",
                        column: x => x.Username,
                        principalTable: "Accounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Widgets_WidgetsId",
                        column: x => x.WidgetsId,
                        principalTable: "Widgets",
                        principalColumn: "WidgetsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Username", "Email", "Firstname", "Lastname", "Password" },
                values: new object[,]
                {
                    { "admin", "abc@gmail.com", "admin", "admin", "$2a$11$8NwgycyRH50lI3/gy/ncu.VxXdHFwmpvwCsnmrLjwa97lgUHqbvsG" },
                    { "admin1", "abc1@gmail.com", "admin1", "admin1", "$2a$11$8NwgycyRH50lI3/gy/ncu.VxXdHFwmpvwCsnmrLjwa97lgUHqbvsG" }
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "EmployeeId", "AvatarUrl", "Department", "Firstname", "Lastname", "Project", "Title" },
                values: new object[,]
                {
                    { 1, "avatar1", "D1", "First", "Person", "Project1", "DEV" },
                    { 2, "avatar2", "D2", "second", "Person", "Project2", "DEV" },
                    { 3, "avatar3", "D3", "third", "Person", "Project3", "DEV" },
                    { 4, "avatar4", "D4", "fourth", "Person", "Project4", "DEV" },
                    { 5, "avatar5", "D5", "fifth", "Person", "Project5", "DEV" }
                });

            migrationBuilder.InsertData(
                table: "Layouts",
                columns: new[] { "LayoutsId", "Column", "Row" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "WidgetTypes",
                columns: new[] { "WidgetTypesId", "WidgetTypesName" },
                values: new object[,]
                {
                    { 1, "Widget Task" },
                    { 2, "Widget Note" },
                    { 3, "Widget Contact" }
                });

            migrationBuilder.InsertData(
                table: "Dashboards",
                columns: new[] { "DashboardsId", "LayoutsId", "Title", "Username" },
                values: new object[] { 1, 1, "Dashboard1", "admin" });

            migrationBuilder.InsertData(
                table: "Widgets",
                columns: new[] { "WidgetsId", "DashboardsId", "Description", "MinHeight", "MinWidth", "Title", "WidgetTypesId" },
                values: new object[] { 1, 1, null, 10, 10, "Widget1", 1 });

            migrationBuilder.InsertData(
                table: "Widgets",
                columns: new[] { "WidgetsId", "DashboardsId", "Description", "MinHeight", "MinWidth", "Title", "WidgetTypesId" },
                values: new object[] { 2, 1, "for test", 20, 20, "Widget2", 2 });

            migrationBuilder.InsertData(
                table: "Widgets",
                columns: new[] { "WidgetsId", "DashboardsId", "Description", "MinHeight", "MinWidth", "Title", "WidgetTypesId" },
                values: new object[] { 3, 1, null, 30, 30, "Widget3", 3 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TasksId", "IsCompleted", "TaskTitle", "Username", "WidgetsId" },
                values: new object[,]
                {
                    { 1, false, "Task1", "admin", 1 },
                    { 2, false, "Task2", "admin", 1 },
                    { 3, true, "Task3", "admin", 1 },
                    { 4, false, "Task4", "admin", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_LayoutsId",
                table: "Dashboards",
                column: "LayoutsId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_Username",
                table: "Dashboards",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Username",
                table: "RefreshTokens",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Username",
                table: "Tasks",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WidgetsId",
                table: "Tasks",
                column: "WidgetsId");

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_DashboardsId",
                table: "Widgets",
                column: "DashboardsId");

            migrationBuilder.CreateIndex(
                name: "IX_Widgets_WidgetTypesId",
                table: "Widgets",
                column: "WidgetTypesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Widgets");

            migrationBuilder.DropTable(
                name: "Dashboards");

            migrationBuilder.DropTable(
                name: "WidgetTypes");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Layouts");
        }
    }
}
