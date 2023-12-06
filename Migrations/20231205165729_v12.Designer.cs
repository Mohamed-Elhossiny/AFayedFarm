﻿// <auto-generated />
using System;
using AFayedFarm.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AFayedFarm.Migrations
{
    [DbContext(typeof(FarmContext))]
    [Migration("20231205165729_v12")]
    partial class v12
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AFayedFarm.Model.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("AFayedFarm.Model.Client", b =>
                {
                    b.Property<int>("ClientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientID"));

                    b.Property<string>("ClientName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Export")
                        .HasColumnType("int");

                    b.HasKey("ClientID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("AFayedFarm.Model.Expense", b =>
                {
                    b.Property<int>("ExpenseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseID"));

                    b.Property<string>("ExpenseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExpenseTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("StoreID")
                        .HasColumnType("int");

                    b.HasKey("ExpenseID");

                    b.HasIndex("ExpenseTypeId");

                    b.HasIndex("StoreID");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("AFayedFarm.Model.ExpenseRecord", b =>
                {
                    b.Property<int>("ExpenseRecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseRecordId"));

                    b.Property<string>("AdditionalNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("AdditionalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("Created_Date")
                        .HasColumnType("Date");

                    b.Property<DateTime?>("ExpenseDate")
                        .HasColumnType("Date");

                    b.Property<int?>("ExpenseID")
                        .HasColumnType("int");

                    b.Property<string>("ExpenseNotes")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<int?>("FarmRecordID")
                        .HasColumnType("int");

                    b.Property<decimal?>("Paied")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Remaining")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("StoreID")
                        .HasColumnType("int");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ExpenseRecordId");

                    b.HasIndex("ExpenseID");

                    b.HasIndex("FarmRecordID");

                    b.HasIndex("StoreID");

                    b.ToTable("ExpenseRecords");
                });

            modelBuilder.Entity("AFayedFarm.Model.Farms", b =>
                {
                    b.Property<int>("FarmsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FarmsID"));

                    b.Property<string>("FarmsName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FarmsID");

                    b.ToTable("Farms");
                });

            modelBuilder.Entity("AFayedFarm.Model.FarmsProduct", b =>
                {
                    b.Property<int>("FarmProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FarmProductID"));

                    b.Property<string>("CarNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created_Date")
                        .HasColumnType("Date");

                    b.Property<decimal?>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("FarmsID")
                        .HasColumnType("int");

                    b.Property<string>("FarmsNotes")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<decimal?>("NetQuantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Paied")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<decimal?>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Remaining")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("SupplyDate")
                        .HasColumnType("Date");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("FarmProductID");

                    b.HasIndex("FarmsID");

                    b.HasIndex("ProductID");

                    b.ToTable("FarmsProducts");
                });

            modelBuilder.Entity("AFayedFarm.Model.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<DateTime?>("Created_Date")
                        .HasColumnType("Date");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductNote")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<decimal?>("ProductUnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProductID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AFayedFarm.Model.Store", b =>
                {
                    b.Property<int>("StoreID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StoreID"));

                    b.Property<DateTime?>("Created_Date")
                        .HasColumnType("Date");

                    b.HasKey("StoreID");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("AFayedFarm.Model.StoreProduct", b =>
                {
                    b.Property<int>("StoreProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StoreProductID"));

                    b.Property<DateTime?>("Created_Date")
                        .HasColumnType("Date");

                    b.Property<int?>("FarmsID")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<decimal?>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("StoreID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SupplyDate")
                        .HasColumnType("Date");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("StoreProductID");

                    b.HasIndex("FarmsID");

                    b.HasIndex("ProductID");

                    b.HasIndex("StoreID");

                    b.ToTable("StoreProducts");
                });

            modelBuilder.Entity("AFayedFarm.Model.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionID"));

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Created_Date")
                        .HasColumnType("Date");

                    b.Property<decimal?>("GetPaied")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<decimal?>("Remaining")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ShippingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StoreID")
                        .HasColumnType("int");

                    b.HasKey("TransactionID");

                    b.HasIndex("ClientID");

                    b.HasIndex("ProductID");

                    b.HasIndex("StoreID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("AFayedFarm.Model.TypeOfExpense", b =>
                {
                    b.Property<int>("ExpenseTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseTypeID"));

                    b.Property<string>("ExpenseTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExpenseTypeID");

                    b.ToTable("TypeOfExpenses");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AFayedFarm.Model.Expense", b =>
                {
                    b.HasOne("AFayedFarm.Model.TypeOfExpense", "ExpenseType")
                        .WithMany("Expense")
                        .HasForeignKey("ExpenseTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AFayedFarm.Model.Store", null)
                        .WithMany("Expenses")
                        .HasForeignKey("StoreID");

                    b.Navigation("ExpenseType");
                });

            modelBuilder.Entity("AFayedFarm.Model.ExpenseRecord", b =>
                {
                    b.HasOne("AFayedFarm.Model.Expense", "Expense")
                        .WithMany("ExpenseRecords")
                        .HasForeignKey("ExpenseID");

                    b.HasOne("AFayedFarm.Model.FarmsProduct", "FarmRecord")
                        .WithMany("ExpeneseRecordList")
                        .HasForeignKey("FarmRecordID");

                    b.HasOne("AFayedFarm.Model.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreID");

                    b.Navigation("Expense");

                    b.Navigation("FarmRecord");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("AFayedFarm.Model.FarmsProduct", b =>
                {
                    b.HasOne("AFayedFarm.Model.Farms", "Farms")
                        .WithMany("FarmsProducts")
                        .HasForeignKey("FarmsID");

                    b.HasOne("AFayedFarm.Model.Product", "Product")
                        .WithMany("FarmsProducts")
                        .HasForeignKey("ProductID");

                    b.Navigation("Farms");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("AFayedFarm.Model.StoreProduct", b =>
                {
                    b.HasOne("AFayedFarm.Model.Farms", "Farms")
                        .WithMany()
                        .HasForeignKey("FarmsID");

                    b.HasOne("AFayedFarm.Model.Product", "Product")
                        .WithMany("StoreProducts")
                        .HasForeignKey("ProductID");

                    b.HasOne("AFayedFarm.Model.Store", "Store")
                        .WithMany("StoreProducts")
                        .HasForeignKey("StoreID");

                    b.Navigation("Farms");

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("AFayedFarm.Model.Transaction", b =>
                {
                    b.HasOne("AFayedFarm.Model.Client", "Client")
                        .WithMany("Transactions")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AFayedFarm.Model.Product", "Product")
                        .WithMany("Transactions")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AFayedFarm.Model.Store", "Store")
                        .WithMany("Transactions")
                        .HasForeignKey("StoreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AFayedFarm.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AFayedFarm.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AFayedFarm.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AFayedFarm.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AFayedFarm.Model.Client", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("AFayedFarm.Model.Expense", b =>
                {
                    b.Navigation("ExpenseRecords");
                });

            modelBuilder.Entity("AFayedFarm.Model.Farms", b =>
                {
                    b.Navigation("FarmsProducts");
                });

            modelBuilder.Entity("AFayedFarm.Model.FarmsProduct", b =>
                {
                    b.Navigation("ExpeneseRecordList");
                });

            modelBuilder.Entity("AFayedFarm.Model.Product", b =>
                {
                    b.Navigation("FarmsProducts");

                    b.Navigation("StoreProducts");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("AFayedFarm.Model.Store", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("StoreProducts");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("AFayedFarm.Model.TypeOfExpense", b =>
                {
                    b.Navigation("Expense");
                });
#pragma warning restore 612, 618
        }
    }
}
