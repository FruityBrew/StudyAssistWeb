﻿// <auto-generated />
using System;
using KnowledgeDataAccessApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KnowledgeDataAccessApi.Migrations
{
    [DbContext(typeof(KnowledgeContext))]
    [Migration("20220418102329_TryNullableId")]
    partial class TryNullableId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StudyAssistModel.DataModel.Catalog", b =>
                {
                    b.Property<int?>("CatalogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CatalogId");

                    b.ToTable("Catalogs");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.Issue", b =>
                {
                    b.Property<int>("IssueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.HasKey("IssueId");

                    b.HasIndex("ThemeId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.IssueUnderStudy", b =>
                {
                    b.Property<int>("IssueUnderStudyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IssueId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RepeateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StudyLevel")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("IssueUnderStudyId");

                    b.HasIndex("IssueId");

                    b.ToTable("IssuesUnderStudy");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.Theme", b =>
                {
                    b.Property<int>("ThemeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CatalogId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ThemeId");

                    b.HasIndex("CatalogId");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.Issue", b =>
                {
                    b.HasOne("StudyAssistModel.DataModel.Theme", "Theme")
                        .WithMany("Issues")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.IssueUnderStudy", b =>
                {
                    b.HasOne("StudyAssistModel.DataModel.Issue", "Issue")
                        .WithMany()
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.Theme", b =>
                {
                    b.HasOne("StudyAssistModel.DataModel.Catalog", "Catalog")
                        .WithMany("Themes")
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Catalog");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.Catalog", b =>
                {
                    b.Navigation("Themes");
                });

            modelBuilder.Entity("StudyAssistModel.DataModel.Theme", b =>
                {
                    b.Navigation("Issues");
                });
#pragma warning restore 612, 618
        }
    }
}
