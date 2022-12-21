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
                    TimeStamp = table.Column<DateOnly>(type: "date", nullable: false, comment: "Time stamp"),
                    IsoCode = table.Column<string>(type: "text", nullable: false, comment: "ISO 3166-1 country code"),
                    Names = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized country names"),
                    OfficialLanguages = table.Column<ICollection<string>>(type: "jsonb", nullable: false, comment: "ISO-639-1 language codes"),
                    OfficialNames = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized official country names")
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
                    TimeStamp = table.Column<DateOnly>(type: "date", nullable: false, comment: "Time stamp"),
                    IsoCode = table.Column<string>(type: "text", nullable: false, comment: "ISO-639-1 language code"),
                    Names = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized language names")
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
                    TimeStamp = table.Column<DateOnly>(type: "date", nullable: false, comment: "Time stamp"),
                    Comments = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: true, comment: "Additional localized comments"),
                    Details = table.Column<int>(type: "integer", nullable: false, comment: "Additional detailed information"),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "End date of the holiday"),
                    Names = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized names of the holiday"),
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
                name: "OUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    TimeStamp = table.Column<DateOnly>(type: "date", nullable: false, comment: "Time stamp"),
                    Code = table.Column<string>(type: "text", nullable: false, comment: "Organizational unit code"),
                    Comments = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: true, comment: "Additional localized comments"),
                    Names = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized organizational unit names"),
                    ShortName = table.Column<string>(type: "text", nullable: false, comment: "Short name for display"),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to country"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Reference to parent organizational unit")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OUnits_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OUnits_OUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OUnits",
                        principalColumn: "Id");
                },
                comment: "Representation of an organizational unit (e.g. a holiday zone or a school type)");

            migrationBuilder.CreateTable(
                name: "Subdivisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique Id"),
                    TimeStamp = table.Column<DateOnly>(type: "date", nullable: false, comment: "Time stamp"),
                    Comments = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: true, comment: "Additional localized comments"),
                    IsoCode = table.Column<string>(type: "text", nullable: false, comment: "IsoCode subdivision code"),
                    Names = table.Column<ICollection<LocalizedText>>(type: "jsonb", nullable: false, comment: "Localized subdivision names"),
                    OfficialLanguages = table.Column<ICollection<string>>(type: "jsonb", nullable: false, comment: "Official languages as ISO-639-1 codes"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: false, comment: "Short name for display"),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Reference to country")
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
                name: "HolidayOUnit",
                columns: table => new
                {
                    HolidaysId = table.Column<Guid>(type: "uuid", nullable: false),
                    OUnitsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayOUnit", x => new { x.HolidaysId, x.OUnitsId });
                    table.ForeignKey(
                        name: "FK_HolidayOUnit_Holidays_HolidaysId",
                        column: x => x.HolidaysId,
                        principalTable: "Holidays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HolidayOUnit_OUnits_OUnitsId",
                        column: x => x.OUnitsId,
                        principalTable: "OUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "OUnitSubdivision",
                columns: table => new
                {
                    OUnitsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubdivisionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUnitSubdivision", x => new { x.OUnitsId, x.SubdivisionsId });
                    table.ForeignKey(
                        name: "FK_OUnitSubdivision_OUnits_OUnitsId",
                        column: x => x.OUnitsId,
                        principalTable: "OUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OUnitSubdivision_Subdivisions_SubdivisionsId",
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
                name: "IX_HolidayOUnit_OUnitsId",
                table: "HolidayOUnit",
                column: "OUnitsId");

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
                name: "IX_OUnits_Code",
                table: "OUnits",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OUnits_CountryId_ShortName",
                table: "OUnits",
                columns: new[] { "CountryId", "ShortName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OUnits_ParentId",
                table: "OUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OUnitSubdivision_SubdivisionsId",
                table: "OUnitSubdivision",
                column: "SubdivisionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_CountryId_ShortName",
                table: "Subdivisions",
                columns: new[] { "CountryId", "ShortName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_IsoCode",
                table: "Subdivisions",
                column: "IsoCode",
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
                name: "HolidayOUnit");

            migrationBuilder.DropTable(
                name: "HolidaySubdivision");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "OUnitSubdivision");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "OUnits");

            migrationBuilder.DropTable(
                name: "Subdivisions");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
