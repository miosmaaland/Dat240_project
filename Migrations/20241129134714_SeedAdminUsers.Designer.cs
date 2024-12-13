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
    [Migration("20241129134714_SeedAdminUsers")]
    partial class SeedAdminUsers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

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
                            Id = new Guid("9207cd38-e1ea-4e6f-aa34-d6be04dc4697"),
                            Email = "admin@example.com",
                            Name = "Admin",
                            PasswordHash = "O2Esdae1BIpDX7bsgeUv+S1teVqLWpwXBw9qY8l6U7I=",
                            PasswordNeedsReset = true
                        },
                        new
                        {
                            Id = new Guid("3b02936c-3b78-4f72-aa30-5517bcc68a36"),
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
#pragma warning restore 612, 618
        }
    }
}