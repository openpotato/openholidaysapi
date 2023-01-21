using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using OpenHolidaysApi.DataLayer;

#nullable disable

namespace OpenHolidaysApi.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    IsoCode = table.Column<string>(type: "text", nullable: false, comment: "ISO 3166-1 country code"),
                    Name = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized country names"),
                    OfficialLanguages = table.Column<ICollection<string>>(type: "jsonb", nullable: false, comment: "ISO-639-1 language codes")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                },
                comment: "Representation of a country");

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    IsoCode = table.Column<string>(type: "text", nullable: false, comment: "ISO-639-1 language code"),
                    Name = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized language names")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                },
                comment: "Representation of a language");

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Comment = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: true, comment: "Additional localized comments"),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "End date of the holiday"),
                    Name = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized names of the holiday"),
                    Nationwide = table.Column<bool>(type: "boolean", nullable: false, comment: "Is this a nationwide holiday?"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Start date of the holiday"),
                    Type = table.Column<int>(type: "integer", nullable: false, comment: "Type of holiday"),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to country")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holidays_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Representation of a holiday");

            migrationBuilder.CreateTable(
                name: "Subdivisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    Category = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized categories"),
                    Code = table.Column<string>(type: "text", nullable: false, comment: "Subdivision code"),
                    Comment = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: true, comment: "Additional localized comments"),
                    IsoCode = table.Column<string>(type: "text", nullable: true, comment: "Subdivision ISO 3166-2 code (if available)"),
                    Name = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized subdivision names"),
                    OfficialLanguages = table.Column<ICollection<string>>(type: "jsonb", nullable: false, comment: "Official languages as ISO-639-1 codes"),
                    ShortName = table.Column<string>(type: "text", nullable: false, comment: "Short name for display"),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to country"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Reference to parent subdivision")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subdivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subdivisions_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subdivisions_Subdivisions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id");
                },
                comment: "Representation of a subdivision (e.g. a federal state or a canton)");

            migrationBuilder.CreateTable(
                name: "HolidaySubdivision",
                columns: table => new
                {
                    HolidaysId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubdivisionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidaySubdivision", x => new { x.HolidaysId, x.SubdivisionsId });
                    table.ForeignKey(
                        name: "FK_HolidaySubdivision_Holidays_HolidaysId",
                        column: x => x.HolidaysId,
                        principalTable: "Holidays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HolidaySubdivision_Subdivisions_SubdivisionsId",
                        column: x => x.SubdivisionsId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsoCode",
                table: "Countries",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_CountryId",
                table: "Holidays",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidaySubdivision_SubdivisionsId",
                table: "HolidaySubdivision",
                column: "SubdivisionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_IsoCode",
                table: "Languages",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_Code",
                table: "Subdivisions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_CountryId_ShortName",
                table: "Subdivisions",
                columns: new[] { "CountryId", "ShortName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_ParentId",
                table: "Subdivisions",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidaySubdivision");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "Subdivisions");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
