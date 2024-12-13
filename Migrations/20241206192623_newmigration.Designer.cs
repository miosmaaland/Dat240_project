﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmaHauJenHoaVij.Infrastructure.Data;

#nullable disable

namespace SmaHauJenHoaVij.Migrations
{
    [DbContext(typeof(ShopContext))]
    [Migration("20241206192623_newmigration")]
    partial class newmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Cart.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ShoppingCartId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Sku")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Cart.ShoppingCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Fulfillment.Offer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Reimbursement")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Tip")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Reimbursement")
                        .IsUnique();

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Fulfillment.Reimbursement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InvoiceId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Tip")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Reimbursements");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Invoicing.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Tip")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Ordering.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Ordering.OrderLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("FoodItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Products.FoodItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PasswordResetTokenExpiry")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("UserType").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Users.Admins.Admin", b =>
                {
                    b.HasBaseType("SmaHauJenHoaVij.Core.Domain.Users.User");

                    b.Property<bool>("PasswordNeedsReset")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Administrator");

                    b.HasData(
                        new
                        {
                            Id = new Guid("793b94d3-9758-4aa3-870e-c1632e3540d8"),
                            Email = "admin@example.com",
                            Name = "Admin",
                            PasswordHash = "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=",
                            PasswordNeedsReset = true
                        },
                        new
                        {
                            Id = new Guid("1101e106-f86c-42d2-8d17-02cfd78598ea"),
                            Email = "admin2@example.com",
                            Name = "Admin2",
                            PasswordHash = "SOW9F8dms32WojBoMOfAr1kEH13KZSmpZi1yDFYNEkw=",
                            PasswordNeedsReset = true
                        });
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Users.Couriers.Courier", b =>
                {
                    b.HasBaseType("SmaHauJenHoaVij.Core.Domain.Users.User");

                    b.Property<decimal>("Earnings")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Courier");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Users.Customers.Customer", b =>
                {
                    b.HasBaseType("SmaHauJenHoaVij.Core.Domain.Users.User");

                    b.Property<bool>("IsApplyingForCourier")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Cart.CartItem", b =>
                {
                    b.HasOne("SmaHauJenHoaVij.Core.Domain.Cart.ShoppingCart", null)
                        .WithMany("Items")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Fulfillment.Offer", b =>
                {
                    b.HasOne("SmaHauJenHoaVij.Core.Domain.Fulfillment.Reimbursement", null)
                        .WithOne()
                        .HasForeignKey("SmaHauJenHoaVij.Core.Domain.Fulfillment.Offer", "Reimbursement")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Ordering.Order", b =>
                {
                    b.OwnsOne("SmaHauJenHoaVij.Core.Domain.Ordering.DeliveryFee", "DeliveryFee", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("TEXT");

                            b1.Property<decimal>("Value")
                                .HasColumnType("TEXT")
                                .HasColumnName("DeliveryFee");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.OwnsOne("SmaHauJenHoaVij.Core.Domain.Ordering.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Building")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("Building");

                            b1.Property<string>("Notes")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("LocationNotes");

                            b1.Property<string>("RoomNumber")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("RoomNumber");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("DeliveryFee")
                        .IsRequired();

                    b.Navigation("Location")
                        .IsRequired();
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Ordering.OrderLine", b =>
                {
                    b.HasOne("SmaHauJenHoaVij.Core.Domain.Ordering.Order", null)
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Products.FoodItem", b =>
                {
                    b.OwnsOne("Picture", "Picture", b1 =>
                        {
                            b1.Property<Guid>("FoodItemId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Path")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("PicturePath");

                            b1.HasKey("FoodItemId");

                            b1.ToTable("FoodItems");

                            b1.WithOwner()
                                .HasForeignKey("FoodItemId");
                        });

                    b.Navigation("Picture")
                        .IsRequired();
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Cart.ShoppingCart", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("SmaHauJenHoaVij.Core.Domain.Ordering.Order", b =>
                {
                    b.Navigation("OrderLines");
                });
#pragma warning restore 612, 618
        }
    }
}
