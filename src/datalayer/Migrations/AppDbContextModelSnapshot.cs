﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OpenHolidaysApi.DataLayer;

#nullable disable

namespace OpenHolidaysApi.DataLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HolidaySubdivision", b =>
                {
                    b.Property<Guid>("HolidaysId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubdivisionsId")
                        .HasColumnType("uuid");

                    b.HasKey("HolidaysId", "SubdivisionsId");

                    b.HasIndex("SubdivisionsId");

                    b.ToTable("HolidaySubdivision");
                });

            modelBuilder.Entity("OpenHolidaysApi.DataLayer.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0)
                        .HasComment("Unique Id");

                    b.Property<string>("IsoCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasComment("ISO 3166-1 country code");

                    b.Property<ICollection<LocalizedText>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasComment("Localized country names");

                    b.Property<ICollection<string>>("OfficialLanguages")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasComment("ISO-639-1 language codes");

                    b.HasKey("Id");

                    b.HasIndex("IsoCode")
                        .IsUnique();

                    b.ToTable("Countries", t =>
                        {
                            t.HasComment("Representation of a country");
                        });
                });

            modelBuilder.Entity("OpenHolidaysApi.DataLayer.Holiday", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0)
                        .HasComment("Unique Id");

                    b.Property<ICollection<LocalizedText>>("Comment")
                        .HasColumnType("jsonb")
                        .HasComment("Additional localized comments");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid")
                        .HasComment("Reference to country");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date")
                        .HasComment("End date of the holiday");

                    b.Property<ICollection<LocalizedText>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasComment("Localized names of the holiday");

                    b.Property<bool>("Nationwide")
                        .HasColumnType("boolean")
                        .HasComment("Is this a nationwide holiday?");

                    b.Property<int>("Quality")
                        .HasColumnType("integer")
                        .HasComment("Quality of holiday");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasComment("Start date of the holiday");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasComment("Type of holiday");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Holidays", t =>
                        {
                            t.HasComment("Representation of a holiday");
                        });
                });

            modelBuilder.Entity("OpenHolidaysApi.DataLayer.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0)
                        .HasComment("Unique Id");

                    b.Property<string>("IsoCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasComment("ISO-639-1 language code");

                    b.Property<ICollection<LocalizedText>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasComment("Localized language names");

                    b.HasKey("Id");

                    b.HasIndex("IsoCode")
                        .IsUnique();

                    b.ToTable("Languages", t =>
                        {
                            t.HasComment("Representation of a language");
                        });
                });

            modelBuilder.Entity("OpenHolidaysApi.DataLayer.Subdivision", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0)
                        .HasComment("Unique Id");

                    b.Property<ICollection<LocalizedText>>("Category")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasComment("Localized categories");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasComment("Subdivision code");

                    b.Property<ICollection<LocalizedText>>("Comment")
                        .HasColumnType("jsonb")
                        .HasComment("Additional localized comments");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid")
                        .HasComment("Reference to country");

                    b.Property<string>("IsoCode")
                        .HasColumnType("text")
                        .HasComment("Subdivision ISO 3166-2 code (if available)");

                    b.Property<ICollection<LocalizedText>>("Name")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasComment("Localized subdivision names");

                    b.Property<ICollection<string>>("OfficialLanguages")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasComment("Official languages as ISO-639-1 codes");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasComment("Reference to parent subdivision");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasComment("Short name for display");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.HasIndex("CountryId", "ShortName")
                        .IsUnique();

                    b.ToTable("Subdivisions", t =>
                        {
                            t.HasComment("Representation of a subdivision (e.g. a federal state or a canton)");
                        });
                });

            modelBuilder.Entity("HolidaySubdivision", b =>
                {
                    b.HasOne("OpenHolidaysApi.DataLayer.Holiday", null)
                        .WithMany()
                        .HasForeignKey("HolidaysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenHolidaysApi.DataLayer.Subdivision", null)
                        .WithMany()
                        .HasForeignKey("SubdivisionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OpenHolidaysApi.DataLayer.Holiday", b =>
                {
                    b.HasOne("OpenHolidaysApi.DataLayer.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("OpenHolidaysApi.DataLayer.Subdivision", b =>
                {
                    b.HasOne("OpenHolidaysApi.DataLayer.Country", "Country")
                        .WithMany("Subdivisions")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenHolidaysApi.DataLayer.Subdivision", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Country");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("OpenHolidaysApi.DataLayer.Country", b =>
                {
                    b.Navigation("Subdivisions");
                });

            modelBuilder.Entity("OpenHolidaysApi.DataLayer.Subdivision", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
