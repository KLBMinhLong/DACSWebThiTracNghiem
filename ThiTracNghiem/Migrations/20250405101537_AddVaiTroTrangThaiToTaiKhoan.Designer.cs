﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThiTracNghiem.Data;

#nullable disable

namespace ThiTracNghiem.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250405101537_AddVaiTroTrangThaiToTaiKhoan")]
    partial class AddVaiTroTrangThaiToTaiKhoan
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ThiTracNghiem.Models.TaiKhoan", b =>
                {
                    b.Property<string>("TenTaiKhoan")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AnhDaiDienUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GioiTinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgaySinh")
                        .HasColumnType("datetime2");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ThoiGianTao")
                        .HasColumnType("datetime2");

                    b.Property<bool>("TrangThaiKhoa")
                        .HasColumnType("bit");

                    b.Property<string>("VaiTro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TenTaiKhoan");

                    b.ToTable("TaiKhoans");
                });
#pragma warning restore 612, 618
        }
    }
}
