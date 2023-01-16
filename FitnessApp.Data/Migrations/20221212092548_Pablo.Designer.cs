﻿// <auto-generated />
using System;
using FitnessApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20221212092548_Pablo")]
    partial class Pablo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FitnessApp.Data.Models.NutrientCategoryDb", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("NutrientCategories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4825),
                            Title = "Macronutrients",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4826)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4827),
                            Title = "Minerals",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4828)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.NutrientDb", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("DailyDose")
                        .HasColumnType("double precision");

                    b.Property<int?>("NutrientCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("NutrientCategoryId");

                    b.ToTable("Nutrients", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4844),
                            DailyDose = 0.75,
                            NutrientCategoryId = 1,
                            Title = "Protein",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4845)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4848),
                            DailyDose = 0.90000000000000002,
                            NutrientCategoryId = 2,
                            Title = "Сalcium",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4849)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductCategoryDb", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("ProductCategories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4594),
                            Title = "Fruits",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4610)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4612),
                            Title = "Vegetables",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4613)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductDb", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ProductSubCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductSubCategoryId");

                    b.ToTable("Products", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4803),
                            ProductSubCategoryId = 1,
                            Title = "Banana",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4804)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4806),
                            ProductSubCategoryId = 2,
                            Title = "Potato",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4806)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductNutrientDb", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("NutrientId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<double>("Quality")
                        .HasColumnType("double precision");

                    b.Property<int?>("TreatingTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("NutrientId");

                    b.HasIndex("ProductId");

                    b.HasIndex("TreatingTypeId");

                    b.ToTable("ProductNutrients", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4890),
                            NutrientId = 1,
                            ProductId = 1,
                            Quality = 0.80000000000000004,
                            TreatingTypeId = 1,
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4891)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4894),
                            NutrientId = 2,
                            ProductId = 2,
                            Quality = 0.90000000000000002,
                            TreatingTypeId = 2,
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4895)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductSubCategoryDb", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ProductCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("ProductSubCategories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4781),
                            ProductCategoryId = 1,
                            Title = "Exotic",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4782)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4784),
                            ProductCategoryId = 2,
                            Title = "Tuberous",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4785)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.TreatingTypeDb", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("TreatingTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4870),
                            Title = "Fresh",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4871)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4873),
                            Title = "Fried",
                            Updated = new DateTime(2022, 12, 12, 12, 25, 39, 243, DateTimeKind.Local).AddTicks(4874)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.NutrientDb", b =>
                {
                    b.HasOne("FitnessApp.Data.Models.NutrientCategoryDb", "NutrientCategory")
                        .WithMany("Nutrients")
                        .HasForeignKey("NutrientCategoryId");

                    b.Navigation("NutrientCategory");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductDb", b =>
                {
                    b.HasOne("FitnessApp.Data.Models.ProductSubCategoryDb", "ProductSubCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductSubCategoryId");

                    b.Navigation("ProductSubCategory");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductNutrientDb", b =>
                {
                    b.HasOne("FitnessApp.Data.Models.NutrientDb", "Nutrient")
                        .WithMany("ProductNutrients")
                        .HasForeignKey("NutrientId");

                    b.HasOne("FitnessApp.Data.Models.ProductDb", "Product")
                        .WithMany("ProductNutrients")
                        .HasForeignKey("ProductId");

                    b.HasOne("FitnessApp.Data.Models.TreatingTypeDb", "TreatingType")
                        .WithMany("ProductNutrients")
                        .HasForeignKey("TreatingTypeId");

                    b.Navigation("Nutrient");

                    b.Navigation("Product");

                    b.Navigation("TreatingType");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductSubCategoryDb", b =>
                {
                    b.HasOne("FitnessApp.Data.Models.ProductCategoryDb", "ProductCategory")
                        .WithMany("ProductSubCategories")
                        .HasForeignKey("ProductCategoryId");

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.NutrientCategoryDb", b =>
                {
                    b.Navigation("Nutrients");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.NutrientDb", b =>
                {
                    b.Navigation("ProductNutrients");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductCategoryDb", b =>
                {
                    b.Navigation("ProductSubCategories");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductDb", b =>
                {
                    b.Navigation("ProductNutrients");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductSubCategoryDb", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.TreatingTypeDb", b =>
                {
                    b.Navigation("ProductNutrients");
                });
#pragma warning restore 612, 618
        }
    }
}
