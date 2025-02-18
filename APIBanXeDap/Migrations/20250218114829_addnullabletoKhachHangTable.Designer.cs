﻿// <auto-generated />
using System;
using APIBanXeDap.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIBanXeDap.Migrations
{
    [DbContext(typeof(Csharp5Context))]
    [Migration("20250218114829_addnullabletoKhachHangTable")]
    partial class addnullabletoKhachHangTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APIBanXeDap.Models.Chitiethoadon", b =>
                {
                    b.Property<int>("MaHoaDon")
                        .HasColumnType("int");

                    b.Property<int>("MaSp")
                        .HasColumnType("int")
                        .HasColumnName("MaSP");

                    b.Property<int>("MaMau")
                        .HasColumnType("int");

                    b.Property<int>("MaKichThuoc")
                        .HasColumnType("int");

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<decimal>("ThanhTien")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("MaHoaDon", "MaSp", "MaMau", "MaKichThuoc");

                    b.HasIndex("MaKichThuoc");

                    b.HasIndex("MaMau");

                    b.HasIndex("MaSp");

                    b.ToTable("CHITIETHOADON", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Chitietsanpham", b =>
                {
                    b.Property<int>("MaSp")
                        .HasColumnType("int")
                        .HasColumnName("MaSP");

                    b.Property<int>("MaMau")
                        .HasColumnType("int");

                    b.Property<int>("MaKichThuoc")
                        .HasColumnType("int");

                    b.Property<double>("DonGia")
                        .HasColumnType("float");

                    b.Property<int>("SoLuongTon")
                        .HasColumnType("int");

                    b.HasKey("MaSp", "MaMau", "MaKichThuoc")
                        .HasName("PK__SANPHAM___81A20C7D87F7213C");

                    b.HasIndex("MaKichThuoc");

                    b.HasIndex("MaMau");

                    b.ToTable("CHITIETSANPHAM", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Danhmuc", b =>
                {
                    b.Property<int>("MaDanhMuc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDanhMuc"));

                    b.Property<bool?>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("TenDanhMuc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaDanhMuc")
                        .HasName("PK__CHUNGLOA__5A2B82D4FD9DA7E3");

                    b.ToTable("DANHMUC", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Hinhanh", b =>
                {
                    b.Property<int>("MaHinhAnh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaHinhAnh"));

                    b.Property<string>("HinhAnh1")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("HinhAnh");

                    b.Property<int>("MaSp")
                        .HasColumnType("int")
                        .HasColumnName("MaSP");

                    b.HasKey("MaHinhAnh")
                        .HasName("PK__HINHANH__A9C37A9BF334F41B");

                    b.HasIndex("MaSp");

                    b.ToTable("HINHANH", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Hoadon", b =>
                {
                    b.Property<int>("MaHoaDon")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaHoaDon"));

                    b.Property<string>("DiaChiNhanHang")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<float>("GiamGiaMaCoupon")
                        .HasColumnType("real");

                    b.Property<string>("Hoten")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Httt")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)")
                        .HasColumnName("HTTT");

                    b.Property<int>("MaKh")
                        .HasColumnType("int")
                        .HasColumnName("MaKH");

                    b.Property<int?>("MaNv")
                        .HasColumnType("int")
                        .HasColumnName("MaNV");

                    b.Property<string>("MoTa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("NgayTao")
                        .HasColumnType("date");

                    b.Property<float>("PhiVanChuyen")
                        .HasColumnType("real");

                    b.Property<string>("Sdt")
                        .HasMaxLength(12)
                        .IsUnicode(false)
                        .HasColumnType("char(12)")
                        .HasColumnName("SDT")
                        .IsFixedLength();

                    b.Property<DateOnly>("ThoiGianGiao")
                        .HasColumnType("date");

                    b.Property<float>("TienGoc")
                        .HasColumnType("real");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasDefaultValue("Chờ xác nhận");

                    b.Property<float>("TongTien")
                        .HasColumnType("real");

                    b.HasKey("MaHoaDon")
                        .HasName("PK__HOADON__835ED13B422391A8");

                    b.HasIndex("MaKh");

                    b.HasIndex("MaNv");

                    b.ToTable("HOADON", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Khachhang", b =>
                {
                    b.Property<int>("MaKh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MaKH");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaKh"));

                    b.Property<string>("Cccd")
                        .HasMaxLength(12)
                        .IsUnicode(false)
                        .HasColumnType("char(12)")
                        .HasColumnName("CCCD")
                        .IsFixedLength();

                    b.Property<string>("DiaChi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("char(40)")
                        .IsFixedLength();

                    b.Property<string>("GioiTinh")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Hinh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool?>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("MatKhau")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("char(20)")
                        .IsFixedLength();

                    b.Property<DateOnly?>("NgaySinh")
                        .HasColumnType("date");

                    b.Property<string>("Sdt")
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("char(11)")
                        .HasColumnName("SDT")
                        .IsFixedLength();

                    b.Property<string>("TenTaiKhoan")
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("char(15)")
                        .IsFixedLength();

                    b.Property<string>("TinhTrang")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)")
                        .HasDefaultValue("Đang hoạt động");

                    b.HasKey("MaKh")
                        .HasName("PK__KHACHHAN__2725CF1E657F00C0");

                    b.ToTable("KHACHHANG", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.MaCoupon", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("DaSuDung")
                        .HasColumnType("bit");

                    b.Property<decimal?>("MinimumOrderAmount")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("NgayHetHan")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<float?>("PhanTramGiam")
                        .HasColumnType("real");

                    b.Property<decimal?>("SoTienGiam")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.HasKey("Code");

                    b.ToTable("MaCoupons");

                    b.HasData(
                        new
                        {
                            Code = "COUPON001",
                            DaSuDung = false,
                            MinimumOrderAmount = 200000m,
                            NgayHetHan = new DateTime(2025, 3, 20, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3337),
                            NgayTao = new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3306),
                            SoTienGiam = 50000m,
                            TrangThai = true
                        },
                        new
                        {
                            Code = "COUPON002",
                            DaSuDung = false,
                            MinimumOrderAmount = 300000m,
                            NgayHetHan = new DateTime(2025, 3, 5, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3360),
                            NgayTao = new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3356),
                            PhanTramGiam = 10f,
                            TrangThai = true
                        },
                        new
                        {
                            Code = "COUPON003",
                            DaSuDung = false,
                            MinimumOrderAmount = 500000m,
                            NgayHetHan = new DateTime(2025, 3, 10, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3372),
                            NgayTao = new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3368),
                            SoTienGiam = 100000m,
                            TrangThai = false
                        },
                        new
                        {
                            Code = "COUPON004",
                            DaSuDung = false,
                            MinimumOrderAmount = 400000m,
                            NgayHetHan = new DateTime(2025, 2, 28, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3382),
                            NgayTao = new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3379),
                            PhanTramGiam = 20f,
                            TrangThai = true
                        },
                        new
                        {
                            Code = "COUPON005",
                            DaSuDung = false,
                            MinimumOrderAmount = 600000m,
                            NgayHetHan = new DateTime(2025, 3, 15, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3392),
                            NgayTao = new DateTime(2025, 2, 18, 18, 48, 27, 3, DateTimeKind.Local).AddTicks(3389),
                            SoTienGiam = 150000m,
                            TrangThai = true
                        });
                });

            modelBuilder.Entity("APIBanXeDap.Models.Mausac", b =>
                {
                    b.Property<int>("MaMau")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaMau"));

                    b.Property<bool?>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("TenMau")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("MaMau")
                        .HasName("PK__MAUSAC__3A5BBB7DF08FE194");

                    b.ToTable("MAUSAC", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Nhacungcap", b =>
                {
                    b.Property<int>("MaNhaCc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MaNhaCC");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNhaCc"));

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("char(40)")
                        .IsFixedLength();

                    b.Property<bool?>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Sdt")
                        .IsRequired()
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("char(11)")
                        .HasColumnName("SDT")
                        .IsFixedLength();

                    b.Property<string>("TenNhaCc")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("TenNhaCC");

                    b.HasKey("MaNhaCc")
                        .HasName("PK__NHACUNGC__C87CD3713545E178");

                    b.ToTable("NHACUNGCAP", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Nhanvien", b =>
                {
                    b.Property<int>("MaNv")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MaNV");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNv"));

                    b.Property<string>("Cccd")
                        .HasMaxLength(12)
                        .IsUnicode(false)
                        .HasColumnType("char(12)")
                        .HasColumnName("CCCD")
                        .IsFixedLength();

                    b.Property<string>("DiaChi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("char(40)")
                        .IsFixedLength();

                    b.Property<string>("GioiTinh")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Hinh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool?>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Luong")
                        .HasColumnType("int");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("char(20)")
                        .IsFixedLength();

                    b.Property<DateOnly?>("NgaySinh")
                        .HasColumnType("date");

                    b.Property<DateOnly>("NgayVaoLam")
                        .HasColumnType("date");

                    b.Property<string>("Sdt")
                        .IsRequired()
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("char(11)")
                        .HasColumnName("SDT")
                        .IsFixedLength();

                    b.Property<string>("TenTaiKhoan")
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("char(15)")
                        .IsFixedLength();

                    b.Property<string>("TinhTrang")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)")
                        .HasDefaultValue("Đang hoạt động");

                    b.Property<string>("VaiTro")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("MaNv")
                        .HasName("PK__NHANVIEN__2725D70A418AABE4");

                    b.ToTable("NHANVIEN", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Sanpham", b =>
                {
                    b.Property<int>("MaSp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MaSP");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaSp"));

                    b.Property<string>("Hinh")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool?>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("MaDanhMuc")
                        .HasColumnType("int");

                    b.Property<int>("MaNhaCc")
                        .HasColumnType("int")
                        .HasColumnName("MaNhaCC");

                    b.Property<int>("MaThuongHieu")
                        .HasColumnType("int");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("NgaySanXuat")
                        .HasColumnType("date");

                    b.Property<string>("TenSp")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("TenSP");

                    b.HasKey("MaSp")
                        .HasName("PK__SANPHAM__2725081C65ECD104");

                    b.HasIndex("MaDanhMuc");

                    b.HasIndex("MaNhaCc");

                    b.HasIndex("MaThuongHieu");

                    b.ToTable("SANPHAM", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.SizeVM", b =>
                {
                    b.Property<int>("MaKichThuoc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaKichThuoc"));

                    b.Property<bool?>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("TenKichThuoc")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("MaKichThuoc")
                        .HasName("PK__KICHTHUO__22BFD6648B50E9DC");

                    b.ToTable("KICHTHUOC", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Thuonghieu", b =>
                {
                    b.Property<int>("MaThuongHieu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaThuongHieu"));

                    b.Property<bool?>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("TenThuongHieu")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("MaThuongHieu")
                        .HasName("PK__THUONGHI__A3733E2C4364572B");

                    b.ToTable("THUONGHIEU", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Vanchuyen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Gia")
                        .HasColumnType("real");

                    b.Property<string>("Phuong")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuanHuyen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThanhPho")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Vanchuyens");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Yeuthich", b =>
                {
                    b.Property<int>("Ma")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Ma"));

                    b.Property<string>("DoiTuongYeuThich")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MaDoiTuong")
                        .HasColumnType("int");

                    b.Property<int>("MaNguoiDung")
                        .HasColumnType("int");

                    b.HasKey("Ma");

                    b.HasIndex("MaDoiTuong");

                    b.HasIndex("MaNguoiDung");

                    b.ToTable("YEUTHICH", (string)null);
                });

            modelBuilder.Entity("APIBanXeDap.Models.Chitiethoadon", b =>
                {
                    b.HasOne("APIBanXeDap.Models.Hoadon", "MaHoaDonNavigation")
                        .WithMany()
                        .HasForeignKey("MaHoaDon")
                        .IsRequired()
                        .HasConstraintName("FK__CHITIETHO__MaHoa__09A971A2");

                    b.HasOne("APIBanXeDap.Models.SizeVM", "MaKichThuocNavigation")
                        .WithMany()
                        .HasForeignKey("MaKichThuoc")
                        .IsRequired()
                        .HasConstraintName("FK__CHITIETHO__MaKic__0D7A0286");

                    b.HasOne("APIBanXeDap.Models.Mausac", "MaMauNavigation")
                        .WithMany()
                        .HasForeignKey("MaMau")
                        .IsRequired()
                        .HasConstraintName("FK__CHITIETHO__MaMau__0B91BA14");

                    b.HasOne("APIBanXeDap.Models.Sanpham", "MaSpNavigation")
                        .WithMany()
                        .HasForeignKey("MaSp")
                        .IsRequired()
                        .HasConstraintName("FK__CHITIETHOA__MaSP__0A9D95DB");

                    b.Navigation("MaHoaDonNavigation");

                    b.Navigation("MaKichThuocNavigation");

                    b.Navigation("MaMauNavigation");

                    b.Navigation("MaSpNavigation");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Chitietsanpham", b =>
                {
                    b.HasOne("APIBanXeDap.Models.SizeVM", "MaKichThuocNavigation")
                        .WithMany("Chitietsanphams")
                        .HasForeignKey("MaKichThuoc")
                        .IsRequired()
                        .HasConstraintName("FK__SANPHAM_T__MaKic__72C60C4A");

                    b.HasOne("APIBanXeDap.Models.Mausac", "MaMauNavigation")
                        .WithMany("Chitietsanphams")
                        .HasForeignKey("MaMau")
                        .IsRequired()
                        .HasConstraintName("FK__SANPHAM_T__MaMau__71D1E811");

                    b.HasOne("APIBanXeDap.Models.Sanpham", "MaSpNavigation")
                        .WithMany("Chitietsanphams")
                        .HasForeignKey("MaSp")
                        .IsRequired()
                        .HasConstraintName("FK__SANPHAM_TH__MaSP__70DDC3D8");

                    b.Navigation("MaKichThuocNavigation");

                    b.Navigation("MaMauNavigation");

                    b.Navigation("MaSpNavigation");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Hinhanh", b =>
                {
                    b.HasOne("APIBanXeDap.Models.Sanpham", "MaSpNavigation")
                        .WithMany("Hinhanhs")
                        .HasForeignKey("MaSp")
                        .IsRequired()
                        .HasConstraintName("FK__HINHANH__MaSP__6477ECF3");

                    b.Navigation("MaSpNavigation");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Hoadon", b =>
                {
                    b.HasOne("APIBanXeDap.Models.Khachhang", "MaKhNavigation")
                        .WithMany("Hoadons")
                        .HasForeignKey("MaKh")
                        .IsRequired()
                        .HasConstraintName("FK__HOADON__MaKH__05D8E0BE");

                    b.HasOne("APIBanXeDap.Models.Nhanvien", "MaNvNavigation")
                        .WithMany("Hoadons")
                        .HasForeignKey("MaNv")
                        .HasConstraintName("FK__HOADON__MaNV__04E4BC85");

                    b.Navigation("MaKhNavigation");

                    b.Navigation("MaNvNavigation");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Sanpham", b =>
                {
                    b.HasOne("APIBanXeDap.Models.Danhmuc", "MaDanhMucNavigation")
                        .WithMany("Sanphams")
                        .HasForeignKey("MaDanhMuc")
                        .IsRequired()
                        .HasConstraintName("FK__SANPHAM__MaDanh__5629CD9C");

                    b.HasOne("APIBanXeDap.Models.Nhacungcap", "MaNhaCcNavigation")
                        .WithMany("Sanphams")
                        .HasForeignKey("MaNhaCc")
                        .IsRequired()
                        .HasConstraintName("FK__SANPHAM__IsDelet__534D60F1");

                    b.HasOne("APIBanXeDap.Models.Thuonghieu", "MaThuongHieuNavigation")
                        .WithMany("Sanphams")
                        .HasForeignKey("MaThuongHieu")
                        .IsRequired()
                        .HasConstraintName("FK__SANPHAM__MaThuon__5441852A");

                    b.Navigation("MaDanhMucNavigation");

                    b.Navigation("MaNhaCcNavigation");

                    b.Navigation("MaThuongHieuNavigation");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Yeuthich", b =>
                {
                    b.HasOne("APIBanXeDap.Models.Sanpham", "Sanpham")
                        .WithMany("YeuThichs")
                        .HasForeignKey("MaDoiTuong")
                        .IsRequired();

                    b.HasOne("APIBanXeDap.Models.Khachhang", "Khachhang")
                        .WithMany("YeuThichs")
                        .HasForeignKey("MaNguoiDung")
                        .IsRequired();

                    b.Navigation("Khachhang");

                    b.Navigation("Sanpham");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Danhmuc", b =>
                {
                    b.Navigation("Sanphams");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Khachhang", b =>
                {
                    b.Navigation("Hoadons");

                    b.Navigation("YeuThichs");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Mausac", b =>
                {
                    b.Navigation("Chitietsanphams");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Nhacungcap", b =>
                {
                    b.Navigation("Sanphams");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Nhanvien", b =>
                {
                    b.Navigation("Hoadons");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Sanpham", b =>
                {
                    b.Navigation("Chitietsanphams");

                    b.Navigation("Hinhanhs");

                    b.Navigation("YeuThichs");
                });

            modelBuilder.Entity("APIBanXeDap.Models.SizeVM", b =>
                {
                    b.Navigation("Chitietsanphams");
                });

            modelBuilder.Entity("APIBanXeDap.Models.Thuonghieu", b =>
                {
                    b.Navigation("Sanphams");
                });
#pragma warning restore 612, 618
        }
    }
}
