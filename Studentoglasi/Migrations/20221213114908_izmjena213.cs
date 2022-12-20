﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentOglasi.Migrations
{
    /// <inheritdoc />
    public partial class izmjena213 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Trajanje",
                table: "Praksa",
                newName: "PocetakPrakse");

            migrationBuilder.AddColumn<DateTime>(
                name: "KrajPrakse",
                table: "Praksa",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KrajPrakse",
                table: "Praksa");

            migrationBuilder.RenameColumn(
                name: "PocetakPrakse",
                table: "Praksa",
                newName: "Trajanje");
        }
    }
}
