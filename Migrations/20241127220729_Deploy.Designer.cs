﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskTimePredicter.Data;

#nullable disable

namespace TaskTimePredicter.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241127220729_Deploy")]
    partial class Deploy
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskTimePredicter.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("categoryId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryDescription")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("categoryDescription");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("categoryName");

                    b.HasKey("CategoryId")
                        .HasName("PK__Categori__23CAF1D81A434EC4");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("projectId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"));

                    b.Property<string>("ProjectDescription")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("projectDescription");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("projectName");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.Quest", b =>
                {
                    b.Property<int>("QuestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("taskId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestId"));

                    b.Property<double?>("ActualTime")
                        .HasColumnType("float")
                        .HasColumnName("actualTime");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("categoryId");

                    b.Property<int?>("CategoryId1")
                        .HasColumnType("int");

                    b.Property<DateOnly>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("creationDate")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<double>("EstimatedTime")
                        .HasColumnType("float")
                        .HasColumnName("estimatedTime");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId1")
                        .HasColumnType("int");

                    b.Property<string>("QuestName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("questName");

                    b.Property<string>("QuestState")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("In Progress")
                        .HasColumnName("questState");

                    b.Property<int?>("SubcategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("SubcategoryId1")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.Property<int?>("UserId1")
                        .HasColumnType("int");

                    b.HasKey("QuestId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CategoryId1");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ProjectId1");

                    b.HasIndex("SubcategoryId");

                    b.HasIndex("SubcategoryId1");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.Subcategory", b =>
                {
                    b.Property<int>("SubcategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("subcategoryId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubcategoryId"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId1")
                        .HasColumnType("int");

                    b.Property<string>("SubcategoryDescription")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("subcategoryDescription");

                    b.Property<string>("SubcategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("subcategoryName");

                    b.HasKey("SubcategoryId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CategoryId1");

                    b.ToTable("Subcategories");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateOnly>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("createdAt")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("userEmail");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("userName");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("userPassword");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasDefaultValue("Developer")
                        .HasColumnName("userRole");

                    b.HasKey("UserId")
                        .HasName("PK__Users__CB9A1CFF1714BBE7");

                    b.HasIndex(new[] { "UserEmail" }, "UQ__Users__D54ADF555E66784B")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            CreatedAt = new DateOnly(2024, 11, 27),
                            UserEmail = "admin@gmail.com",
                            UserName = "Administrador Base",
                            UserPassword = "admin123",
                            UserRole = "Administrator"
                        });
                });

            modelBuilder.Entity("TaskTimePredicter.Models.Quest", b =>
                {
                    b.HasOne("TaskTimePredicter.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TaskTimePredicter.Models.Category", null)
                        .WithMany("Quests")
                        .HasForeignKey("CategoryId1");

                    b.HasOne("TaskTimePredicter.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TaskTimePredicter.Models.Project", null)
                        .WithMany("Quests")
                        .HasForeignKey("ProjectId1");

                    b.HasOne("TaskTimePredicter.Models.Subcategory", "Subcategory")
                        .WithMany()
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TaskTimePredicter.Models.Subcategory", null)
                        .WithMany("Quests")
                        .HasForeignKey("SubcategoryId1");

                    b.HasOne("TaskTimePredicter.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TaskTimePredicter.Models.User", null)
                        .WithMany("Quests")
                        .HasForeignKey("UserId1");

                    b.Navigation("Category");

                    b.Navigation("Project");

                    b.Navigation("Subcategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.Subcategory", b =>
                {
                    b.HasOne("TaskTimePredicter.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TaskTimePredicter.Models.Category", null)
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoryId1");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.Category", b =>
                {
                    b.Navigation("Quests");

                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.Project", b =>
                {
                    b.Navigation("Quests");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.Subcategory", b =>
                {
                    b.Navigation("Quests");
                });

            modelBuilder.Entity("TaskTimePredicter.Models.User", b =>
                {
                    b.Navigation("Quests");
                });
#pragma warning restore 612, 618
        }
    }
}
