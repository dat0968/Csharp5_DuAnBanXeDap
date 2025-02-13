using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIBanXeDap.Models;

public partial class Csharp5Context : DbContext
{
    public Csharp5Context()
    {
    }

    public Csharp5Context(DbContextOptions<Csharp5Context> options)
        : base(options)
    {
    }


    public virtual DbSet<Chitiethoadon> Chitiethoadons { get; set; }

    public virtual DbSet<Chitietsanpham> Chitietsanphams { get; set; }

    public virtual DbSet<Danhmuc> Danhmucs { get; set; }

    public virtual DbSet<Hinhanh> Hinhanhs { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<SizeVM> Kichthuocs { get; set; }

    public virtual DbSet<Mausac> Mausacs { get; set; }

    public virtual DbSet<Nhacungcap> Nhacungcaps { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Thuonghieu> Thuonghieus { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<MaCoupon> MaCoupons { get; set; }
    public virtual DbSet<Vanchuyen> Vanchuyens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       

        modelBuilder.Entity<Chitiethoadon>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDon, e.MaSp, e.MaMau, e.MaKichThuoc});
            entity.ToTable("CHITIETHOADON");

            entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany()
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETHO__MaHoa__09A971A2");

            entity.HasOne(d => d.MaKichThuocNavigation).WithMany()
                .HasForeignKey(d => d.MaKichThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETHO__MaKic__0D7A0286");

            entity.HasOne(d => d.MaMauNavigation).WithMany()
                .HasForeignKey(d => d.MaMau)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETHO__MaMau__0B91BA14");

            entity.HasOne(d => d.MaSpNavigation).WithMany()
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETHOA__MaSP__0A9D95DB");
        });

        modelBuilder.Entity<Chitietsanpham>(entity =>
        {
            entity.HasKey(e => new { e.MaSp, e.MaMau, e.MaKichThuoc }).HasName("PK__SANPHAM___81A20C7D87F7213C");

            entity.ToTable("CHITIETSANPHAM");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.HasOne(d => d.MaKichThuocNavigation).WithMany(p => p.Chitietsanphams)
                .HasForeignKey(d => d.MaKichThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM_T__MaKic__72C60C4A");

            entity.HasOne(d => d.MaMauNavigation).WithMany(p => p.Chitietsanphams)
                .HasForeignKey(d => d.MaMau)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM_T__MaMau__71D1E811");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Chitietsanphams)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM_TH__MaSP__70DDC3D8");
        });

        modelBuilder.Entity<Danhmuc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__CHUNGLOA__5A2B82D4FD9DA7E3");

            entity.ToTable("DANHMUC");

            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(50);
        });

        modelBuilder.Entity<Hinhanh>(entity =>
        {
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HINHANH__A9C37A9BF334F41B");

            entity.ToTable("HINHANH");

            entity.Property(e => e.HinhAnh1)
                .IsUnicode(false)
                .HasColumnName("HinhAnh");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Hinhanhs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HINHANH__MaSP__6477ECF3");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HOADON__835ED13B422391A8");

            entity.ToTable("HOADON");

            entity.Property(e => e.DiaChiNhanHang).HasMaxLength(200);
            entity.Property(e => e.Hoten).HasMaxLength(50);
            entity.Property(e => e.Httt)
                .HasMaxLength(25)
                .HasColumnName("HTTT");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.Sdt)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.TinhTrang)
                .HasMaxLength(30)
                .HasDefaultValue("Chờ xác nhận");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOADON__MaKH__05D8E0BE");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__HOADON__MaNV__04E4BC85");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KHACHHAN__2725CF1E657F00C0");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.Cccd)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CCCD");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.GioiTinh).HasMaxLength(5);
            entity.Property(e => e.HoTen).HasMaxLength(40);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Sdt)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.TenTaiKhoan)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TinhTrang)
                .HasMaxLength(25)
                .HasDefaultValue("Đang hoạt động");
        });

        modelBuilder.Entity<SizeVM>(entity =>
        {
            entity.HasKey(e => e.MaKichThuoc).HasName("PK__KICHTHUO__22BFD6648B50E9DC");

            entity.ToTable("KICHTHUOC");

            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.TenKichThuoc).HasMaxLength(20);
        });

        modelBuilder.Entity<Mausac>(entity =>
        {
            entity.HasKey(e => e.MaMau).HasName("PK__MAUSAC__3A5BBB7DF08FE194");

            entity.ToTable("MAUSAC");

            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.TenMau).HasMaxLength(20);
        });

        modelBuilder.Entity<Nhacungcap>(entity =>
        {
            entity.HasKey(e => e.MaNhaCc).HasName("PK__NHACUNGC__C87CD3713545E178");

            entity.ToTable("NHACUNGCAP");

            entity.Property(e => e.MaNhaCc).HasColumnName("MaNhaCC");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.TenNhaCc)
                .HasMaxLength(60)
                .HasColumnName("TenNhaCC");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NHANVIEN__2725D70A418AABE4");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.Cccd)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CCCD");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.GioiTinh).HasMaxLength(5);
            entity.Property(e => e.HoTen).HasMaxLength(40);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Sdt)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.TenTaiKhoan)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TinhTrang)
                .HasMaxLength(25)
                .HasDefaultValue("Đang hoạt động");
            entity.Property(e => e.VaiTro).HasMaxLength(25);
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SANPHAM__2725081C65ECD104");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.Hinh).HasMaxLength(200);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.MaNhaCc).HasColumnName("MaNhaCC");
            entity.Property(e => e.TenSp)
                .HasMaxLength(40)
                .HasColumnName("TenSP");

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.MaDanhMuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM__MaDanh__5629CD9C");

            entity.HasOne(d => d.MaNhaCcNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.MaNhaCc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM__IsDelet__534D60F1");

            entity.HasOne(d => d.MaThuongHieuNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.MaThuongHieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM__MaThuon__5441852A");
        });

        modelBuilder.Entity<Thuonghieu>(entity =>
        {
            entity.HasKey(e => e.MaThuongHieu).HasName("PK__THUONGHI__A3733E2C4364572B");

            entity.ToTable("THUONGHIEU");

            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.TenThuongHieu).HasMaxLength(40);
        });
        //Thêm data cho bảng MaCoupon
        modelBuilder.Entity<MaCoupon>().HasData(
        new MaCoupon
        {
            Code = "COUPON001",
            SoTienGiam = 50000,
            PhanTramGiam = null,
            NgayHetHan = DateTime.Now.AddDays(30),
            TrangThai = true,
            MinimumOrderAmount = 200000
        },
        new MaCoupon
        {
            Code = "COUPON002",
            SoTienGiam = null,
            PhanTramGiam = 10,
            NgayHetHan = DateTime.Now.AddDays(15),
            TrangThai = true,
            MinimumOrderAmount = 300000
        },
        new MaCoupon
        {
            Code = "COUPON003",
            SoTienGiam = 100000,
            PhanTramGiam = null,
            NgayHetHan = DateTime.Now.AddDays(20),
            TrangThai = false,
            MinimumOrderAmount = 500000
        },
        new MaCoupon
        {
            Code = "COUPON004",
            SoTienGiam = null,
            PhanTramGiam = 20,
            NgayHetHan = DateTime.Now.AddDays(10),
            TrangThai = true,
            MinimumOrderAmount = 400000
        },
        new MaCoupon
        {
            Code = "COUPON005",
            SoTienGiam = 150000,
            PhanTramGiam = null,
            NgayHetHan = DateTime.Now.AddDays(25),
            TrangThai = true,
            MinimumOrderAmount = 600000
        }
    );


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
