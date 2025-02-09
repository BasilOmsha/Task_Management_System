using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PMS_Project.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class schemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Firstname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Lastname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TokenRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Priority",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workspace_AppUser_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBoard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBoard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBoard_AppUser_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectBoard_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workspace_AppUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace_AppUser", x => new { x.UserId, x.WorkspaceId });
                    table.ForeignKey(
                        name: "FK_Workspace_AppUser_AppRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workspace_AppUser_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workspace_AppUser_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    ProjectBoardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppList_ProjectBoard_ProjectBoardId",
                        column: x => x.ProjectBoardId,
                        principalTable: "ProjectBoard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBoard_AppUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectBoardId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBoard_AppUser", x => new { x.UserId, x.ProjectBoardId });
                    table.ForeignKey(
                        name: "FK_ProjectBoard_AppUser_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectBoard_AppUser_ProjectBoard_ProjectBoardId",
                        column: x => x.ProjectBoardId,
                        principalTable: "ProjectBoard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBoardLabel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectBoardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBoardLabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBoardLabel_ProjectBoard_ProjectBoardId",
                        column: x => x.ProjectBoardId,
                        principalTable: "ProjectBoard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ListId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    PriorityId = table.Column<Guid>(type: "uuid", nullable: false),
                    DueDate = table.Column<string>(type: "text", nullable: true),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskCard_AppList_ListId",
                        column: x => x.ListId,
                        principalTable: "AppList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskCard_Priority_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskCard_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCard_AppUser",
                columns: table => new
                {
                    TaskCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCard_AppUser", x => new { x.UserId, x.TaskCardId });
                    table.ForeignKey(
                        name: "FK_TaskCard_AppUser_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskCard_AppUser_TaskCard_TaskCardId",
                        column: x => x.TaskCardId,
                        principalTable: "TaskCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCard_ProjectBoardLabel",
                columns: table => new
                {
                    ProjectBoardLabelId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskCardId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCard_ProjectBoardLabel", x => new { x.TaskCardId, x.ProjectBoardLabelId });
                    table.ForeignKey(
                        name: "FK_TaskCard_ProjectBoardLabel_ProjectBoardLabel_ProjectBoardLa~",
                        column: x => x.ProjectBoardLabelId,
                        principalTable: "ProjectBoardLabel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskCard_ProjectBoardLabel_TaskCard_TaskCardId",
                        column: x => x.TaskCardId,
                        principalTable: "TaskCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCardActivity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Activity = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCardActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskCardActivity_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskCardActivity_TaskCard_TaskCardId",
                        column: x => x.TaskCardId,
                        principalTable: "TaskCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCardChecklist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCardChecklist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskCardChecklist_TaskCard_TaskCardId",
                        column: x => x.TaskCardId,
                        principalTable: "TaskCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCardChecklistItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskCardChecklistId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    IsChecked = table.Column<bool>(type: "boolean", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCardChecklistItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskCardChecklistItem_TaskCardChecklist_TaskCardChecklistId",
                        column: x => x.TaskCardChecklistId,
                        principalTable: "TaskCardChecklist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppRole",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("3933570b-f51f-4bb3-a64e-308a0d5f7b91"), "Administrator with elevated permissions within the workspace.", "Admin" },
                    { new Guid("3e755c18-dad2-484f-adf1-de3cad63ff82"), "Contributor with limited permissions within the workspace.", "Contributor" },
                    { new Guid("b40abe8b-85d9-4873-854e-cdec15f152e4"), "Owner of the workspace with full permissions.", "Owner" }
                });

            migrationBuilder.InsertData(
                table: "AppUser",
                columns: new[] { "Id", "CreatedAt", "Email", "Firstname", "Lastname", "Password", "RefreshToken", "RefreshTokenExpiry", "TokenRevoked", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("05b4e808-21d8-49ff-976e-7b9d2cccef1e"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3235), "user3@email.com", "User3", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3236), "user3" },
                    { new Guid("083b5288-e588-4cd3-abe3-964ab00e8f89"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3223), "user1@email.com", "User1", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3223), "user1" },
                    { new Guid("185db6f4-ebb6-4489-b697-3524b3517a2f"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3210), "user0@email.com", "User0", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3216), "user0" },
                    { new Guid("1fabe7e6-38d9-4b2a-8985-49fbf47dace7"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3386), "user22@email.com", "User22", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3386), "user22" },
                    { new Guid("249aa40e-318a-4911-afb1-ef855822dfcb"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3260), "user7@email.com", "User7", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3260), "user7" },
                    { new Guid("2d021956-e3cd-4efa-a1ce-228c13c5e345"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3251), "user6@email.com", "User6", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3251), "user6" },
                    { new Guid("39bf1bdd-6f57-46f2-87d5-48666fb15c61"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3282), "user10@email.com", "User10", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3283), "user10" },
                    { new Guid("3a4a768e-095e-49ff-a93d-925845d8fe5f"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3381), "user21@email.com", "User21", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3381), "user21" },
                    { new Guid("3b33a18d-39c0-4b1d-a821-21ba69e1c423"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3366), "user18@email.com", "User18", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3366), "user18" },
                    { new Guid("63be1873-e720-4db6-8c32-54aea961fed3"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3230), "user2@email.com", "User2", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3230), "user2" },
                    { new Guid("68a1f39f-fdd9-4ebe-8f75-bd9886d2739e"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3412), "user26@email.com", "User26", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3412), "user26" },
                    { new Guid("6c179786-eef1-4b5a-9229-913babbca033"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3265), "user8@email.com", "User8", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3266), "user8" },
                    { new Guid("6ccc4b87-13a1-434a-ac9b-bce9ad5281b8"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3287), "user11@email.com", "User11", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3288), "user11" },
                    { new Guid("72111a63-d764-4f39-a619-4d8a404b4bc2"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3427), "user29@email.com", "User29", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3427), "user29" },
                    { new Guid("743f9562-e711-4d81-99bf-3f5ee71fe70c"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3292), "user12@email.com", "User12", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3293), "user12" },
                    { new Guid("89ffe460-bf00-46b9-8a89-03926bbf1b24"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3335), "user13@email.com", "User13", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3335), "user13" },
                    { new Guid("8d5a4341-dfef-4f2f-9b4e-7ba13107f80a"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3360), "user17@email.com", "User17", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3360), "user17" },
                    { new Guid("935d508a-1b09-4c14-b823-860c9956b568"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3422), "user28@email.com", "User28", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3422), "user28" },
                    { new Guid("9859f9b5-ef04-4ba7-bdfc-11f546db1b87"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3349), "user15@email.com", "User15", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3350), "user15" },
                    { new Guid("a1f75b81-d21b-492a-9b83-128ad68062c6"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3240), "user4@email.com", "User4", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3241), "user4" },
                    { new Guid("a8de143d-d5c3-4f4e-a711-7875490f429c"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3355), "user16@email.com", "User16", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3355), "user16" },
                    { new Guid("a987ff7a-424c-45fe-abfc-b938f7a49da1"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3400), "user24@email.com", "User24", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3401), "user24" },
                    { new Guid("aa8b2be6-f652-4de4-8a94-8f819f6a10ad"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3405), "user25@email.com", "User25", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3406), "user25" },
                    { new Guid("b1c02ad2-adac-4091-a145-ab705340e781"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3246), "user5@email.com", "User5", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3246), "user5" },
                    { new Guid("b5d2e3ae-f5f5-45a0-ab50-9b4fb9b152ff"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3341), "user14@email.com", "User14", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3341), "user14" },
                    { new Guid("bc3f2c28-7e98-43c7-aaea-c764d6bef8ee"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3394), "user23@email.com", "User23", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3394), "user23" },
                    { new Guid("cc96e8b7-fbed-42e3-a98c-995058a1712b"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3416), "user27@email.com", "User27", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3417), "user27" },
                    { new Guid("d3e5f9c2-9059-454d-9f59-040d2c3fca47"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3271), "user9@email.com", "User9", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3271), "user9" },
                    { new Guid("f09fec9c-6c1f-43e4-ad49-28e020085b74"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3371), "user19@email.com", "User19", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3371), "user19" },
                    { new Guid("f7ea96e2-4fdb-47bf-8ebe-fa1db48b2bb3"), new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3376), "user20@email.com", "User20", "Test", "Qwerty123!", null, null, false, new DateTime(2025, 2, 9, 14, 18, 28, 722, DateTimeKind.Utc).AddTicks(3376), "user20" }
                });

            migrationBuilder.InsertData(
                table: "Priority",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1b5da427-2aec-4689-9e3e-b833645fca6c"), "High" },
                    { new Guid("2fde42aa-233b-41ef-8619-ac8e8264d0ea"), "Low" },
                    { new Guid("6b6636ff-7a74-4f4a-831e-af070061ce71"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3f0a6711-0093-47eb-81d9-a3ce2dccfe48"), "New" },
                    { new Guid("d1ae8e58-f472-424a-afb2-28cb0845e5a5"), "Done" },
                    { new Guid("ef7f307c-06d3-432e-92ae-df22efc5ec32"), "In Progress" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppList_ProjectBoardId",
                table: "AppList",
                column: "ProjectBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoard_CreatorUserId",
                table: "ProjectBoard",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoard_WorkspaceId",
                table: "ProjectBoard",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoard_AppUser_ProjectBoardId",
                table: "ProjectBoard_AppUser",
                column: "ProjectBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoardLabel_ProjectBoardId",
                table: "ProjectBoardLabel",
                column: "ProjectBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCard_ListId",
                table: "TaskCard",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCard_PriorityId",
                table: "TaskCard",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCard_StatusId",
                table: "TaskCard",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCard_AppUser_TaskCardId",
                table: "TaskCard_AppUser",
                column: "TaskCardId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCard_ProjectBoardLabel_ProjectBoardLabelId",
                table: "TaskCard_ProjectBoardLabel",
                column: "ProjectBoardLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCardActivity_TaskCardId",
                table: "TaskCardActivity",
                column: "TaskCardId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCardActivity_UserId",
                table: "TaskCardActivity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCardChecklist_TaskCardId",
                table: "TaskCardChecklist",
                column: "TaskCardId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCardChecklistItem_TaskCardChecklistId",
                table: "TaskCardChecklistItem",
                column: "TaskCardChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Workspace_CreatorUserId",
                table: "Workspace",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Workspace_AppUser_RoleId",
                table: "Workspace_AppUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Workspace_AppUser_WorkspaceId",
                table: "Workspace_AppUser",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectBoard_AppUser");

            migrationBuilder.DropTable(
                name: "TaskCard_AppUser");

            migrationBuilder.DropTable(
                name: "TaskCard_ProjectBoardLabel");

            migrationBuilder.DropTable(
                name: "TaskCardActivity");

            migrationBuilder.DropTable(
                name: "TaskCardChecklistItem");

            migrationBuilder.DropTable(
                name: "Workspace_AppUser");

            migrationBuilder.DropTable(
                name: "ProjectBoardLabel");

            migrationBuilder.DropTable(
                name: "TaskCardChecklist");

            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "TaskCard");

            migrationBuilder.DropTable(
                name: "AppList");

            migrationBuilder.DropTable(
                name: "Priority");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "ProjectBoard");

            migrationBuilder.DropTable(
                name: "Workspace");

            migrationBuilder.DropTable(
                name: "AppUser");
        }
    }
}
