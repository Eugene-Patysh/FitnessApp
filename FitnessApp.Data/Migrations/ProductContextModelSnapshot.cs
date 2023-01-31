﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FitnessApp.Data.Migrations
{
    [DbContext(typeof(ProductContext))]
    partial class ProductContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FitnessApp.Data.Models.NutrientCategoryDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

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
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4668),
                            Title = "Macronutrients",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4669)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4670),
                            Title = "Minerals",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4671)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.NutrientDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("DailyDose")
                        .HasColumnType("double precision");

                    b.Property<int>("NutrientCategoryId")
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
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4686),
                            DailyDose = 0.75,
                            NutrientCategoryId = 1,
                            Title = "Protein",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4687)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4689),
                            DailyDose = 0.90000000000000002,
                            NutrientCategoryId = 2,
                            Title = "Сalcium",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4689)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductCategoryDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

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
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4449),
                            Title = "Fruits",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4459)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4461),
                            Title = "Vegetables",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4462)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ProductSubCategoryId")
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
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4648),
                            ProductSubCategoryId = 1,
                            Title = "Banana",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4649)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4651),
                            ProductSubCategoryId = 2,
                            Title = "Potato",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4652)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductNutrientDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("NutrientId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<double>("Quality")
                        .HasColumnType("double precision");

                    b.Property<int>("TreatingTypeId")
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
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4780),
                            NutrientId = 1,
                            ProductId = 1,
                            Quality = 0.80000000000000004,
                            TreatingTypeId = 1,
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4781)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4783),
                            NutrientId = 2,
                            ProductId = 2,
                            Quality = 0.90000000000000002,
                            TreatingTypeId = 2,
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4784)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductSubCategoryDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ProductCategoryId")
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
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4626),
                            ProductCategoryId = 1,
                            Title = "Exotic",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4627)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4629),
                            ProductCategoryId = 2,
                            Title = "Tuberous",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4629)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.TreatingTypeDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

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
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4704),
                            Title = "Fresh",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4704)
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4707),
                            Title = "Fried",
                            Updated = new DateTime(2022, 11, 23, 15, 10, 36, 983, DateTimeKind.Local).AddTicks(4708)
                        });
                });

            modelBuilder.Entity("FitnessApp.Data.Models.NutrientDb", b =>
                {
                    b.HasOne("FitnessApp.Data.Models.NutrientCategoryDb", "NutrientCategory")
                        .WithMany("Nutrients")
                        .HasForeignKey("NutrientCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NutrientCategory");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductDb", b =>
                {
                    b.HasOne("FitnessApp.Data.Models.ProductSubCategoryDb", "ProductSubCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductSubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductSubCategory");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductNutrientDb", b =>
                {
                    b.HasOne("FitnessApp.Data.Models.NutrientDb", "Nutrient")
                        .WithMany("ProductNutrients")
                        .HasForeignKey("NutrientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FitnessApp.Data.Models.ProductDb", "Product")
                        .WithMany("ProductNutrients")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FitnessApp.Data.Models.TreatingTypeDb", "TreatingType")
                        .WithMany("ProductNutrients")
                        .HasForeignKey("TreatingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nutrient");

                    b.Navigation("Product");

                    b.Navigation("TreatingType");
                });

            modelBuilder.Entity("FitnessApp.Data.Models.ProductSubCategoryDb", b =>
                {
                    b.HasOne("FitnessApp.Data.Models.ProductCategoryDb", "ProductCategory")
                        .WithMany("ProductSubCategories")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
