﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Light.Identity.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: Schemas.Identity);

            migrationBuilder.EnsureSchema(name: Schemas.Audit);

            migrationBuilder.EnsureSchema(name: Schemas.System);

            migrationBuilder.CreateTable(
                name: Tables.Roles,
                schema: Schemas.Identity,
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),

                    /* Custom */
                    Description = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{Tables.Roles}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: Tables.Users,
                schema: Schemas.Identity,
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),

                    /* Custom */
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    UseDomainPassword = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{Tables.Users}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: Tables.RoleClaims,
                schema: Schemas.Identity,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{Tables.RoleClaims}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{Tables.RoleClaims}_{Tables.Roles}_RoleId",
                        column: x => x.RoleId,
                        principalTable: Tables.Roles,
                        principalSchema: Schemas.Identity,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: Tables.UserClaims,
                schema: Schemas.Identity,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{Tables.UserClaims}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{Tables.UserClaims}_{Tables.Users}_UserId",
                        column: x => x.UserId,
                        principalTable: Tables.Users,
                        principalSchema: Schemas.Identity,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: Tables.UserLogins,
                schema: Schemas.Identity,
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{Tables.UserLogins}", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: $"FK_{Tables.UserLogins}_{Tables.Users}_UserId",
                        column: x => x.UserId,
                        principalTable: Tables.Users,
                        principalSchema: Schemas.Identity,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: Tables.UserRoles,
                schema: Schemas.Identity,
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{Tables.UserRoles}", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: $"FK_{Tables.UserRoles}_{Tables.Roles}_RoleId",
                        column: x => x.RoleId,
                        principalTable: Tables.Roles,
                        principalSchema: Schemas.Identity,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: $"FK_{Tables.UserRoles}_{Tables.Users}_UserId",
                        column: x => x.UserId,
                        principalTable: Tables.Users,
                        principalSchema: Schemas.Identity,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: Tables.UserTokens,
                schema: Schemas.Identity,
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{Tables.UserTokens}", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: $"FK_{Tables.UserTokens}_{Tables.Users}_UserId",
                        column: x => x.UserId,
                        principalTable: Tables.Users,
                        principalSchema: Schemas.Identity,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: $"IX_{Tables.RoleClaims}_RoleId",
                table: Tables.RoleClaims,
                schema: Schemas.Identity,
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: Tables.Roles,
                schema: Schemas.Identity,
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: $"IX_{Tables.UserClaims}_UserId",
                table: Tables.UserClaims,
                schema: Schemas.Identity,
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: $"IX_{Tables.UserLogins}_UserId",
                table: Tables.UserLogins,
                schema: Schemas.Identity,
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: $"IX_{Tables.UserRoles}_RoleId",
                table: Tables.UserRoles,
                schema: Schemas.Identity,
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: Tables.Users,
                schema: Schemas.Identity,
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: Tables.Users,
                schema: Schemas.Identity,
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: Tables.RoleClaims,
                schema: Schemas.Identity);

            migrationBuilder.DropTable(
                name: Tables.UserClaims,
                schema: Schemas.Identity);

            migrationBuilder.DropTable(
                name: Tables.UserLogins,
                schema: Schemas.Identity);

            migrationBuilder.DropTable(
                name: Tables.UserRoles,
                schema: Schemas.Identity);

            migrationBuilder.DropTable(
                name: Tables.UserTokens,
                schema: Schemas.Identity);

            migrationBuilder.DropTable(
                name: Tables.Roles,
                schema: Schemas.Identity);

            migrationBuilder.DropTable(
                name: Tables.Users,
                schema: Schemas.Identity);
        }
    }
}