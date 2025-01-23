using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APIBanXeDap.Migrations
{
    /// <inheritdoc />
    public partial class DLSS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANHMUC",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CHUNGLOA__5A2B82D4FD9DA7E3", x => x.MaDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "KHACHHANG",
                columns: table => new
                {
                    MaKH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CCCD = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    SDT = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "char(40)", unicode: false, fixedLength: true, maxLength: 40, nullable: false),
                    TenTaiKhoan = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    MatKhau = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true, defaultValue: "Đang hoạt động"),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KHACHHAN__2725CF1E657F00C0", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "KICHTHUOC",
                columns: table => new
                {
                    MaKichThuoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKichThuoc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KICHTHUO__22BFD6648B50E9DC", x => x.MaKichThuoc);
                });

            migrationBuilder.CreateTable(
                name: "MaCoupons",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoTienGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhanTramGiam = table.Column<float>(type: "real", nullable: true),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaCoupons", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MAUSAC",
                columns: table => new
                {
                    MaMau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMau = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MAUSAC__3A5BBB7DF08FE194", x => x.MaMau);
                });

            migrationBuilder.CreateTable(
                name: "NHACUNGCAP",
                columns: table => new
                {
                    MaNhaCC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCC = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "char(40)", unicode: false, fixedLength: true, maxLength: 40, nullable: false),
                    SDT = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NHACUNGC__C87CD3713545E178", x => x.MaNhaCC);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIEN",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CCCD = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    SDT = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "char(40)", unicode: false, fixedLength: true, maxLength: 40, nullable: false),
                    NgayVaoLam = table.Column<DateOnly>(type: "date", nullable: false),
                    Luong = table.Column<int>(type: "int", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TenTaiKhoan = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, maxLength: 15, nullable: true),
                    MatKhau = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true, defaultValue: "Đang hoạt động"),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NHANVIEN__2725D70A418AABE4", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "THUONGHIEU",
                columns: table => new
                {
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__THUONGHI__A3733E2C4364572B", x => x.MaThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "HOADON",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaChiNhanHang = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgayTao = table.Column<DateOnly>(type: "date", nullable: false),
                    HTTT = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Chờ xác nhận"),
                    MaNV = table.Column<int>(type: "int", nullable: true),
                    MaKH = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hoten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SDT = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    ThoiGianGiao = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HOADON__835ED13B422391A8", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK__HOADON__MaKH__05D8E0BE",
                        column: x => x.MaKH,
                        principalTable: "KHACHHANG",
                        principalColumn: "MaKH");
                    table.ForeignKey(
                        name: "FK__HOADON__MaNV__04E4BC85",
                        column: x => x.MaNV,
                        principalTable: "NHANVIEN",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "SANPHAM",
                columns: table => new
                {
                    MaSP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSP = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false),
                    Hinh = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySanXuat = table.Column<DateOnly>(type: "date", nullable: false),
                    MaNhaCC = table.Column<int>(type: "int", nullable: false),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SANPHAM__2725081C65ECD104", x => x.MaSP);
                    table.ForeignKey(
                        name: "FK__SANPHAM__IsDelet__534D60F1",
                        column: x => x.MaNhaCC,
                        principalTable: "NHACUNGCAP",
                        principalColumn: "MaNhaCC");
                    table.ForeignKey(
                        name: "FK__SANPHAM__MaDanh__5629CD9C",
                        column: x => x.MaDanhMuc,
                        principalTable: "DANHMUC",
                        principalColumn: "MaDanhMuc");
                    table.ForeignKey(
                        name: "FK__SANPHAM__MaThuon__5441852A",
                        column: x => x.MaThuongHieu,
                        principalTable: "THUONGHIEU",
                        principalColumn: "MaThuongHieu");
                });

            migrationBuilder.CreateTable(
                name: "CHITIETHOADON",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false),
                    MaSP = table.Column<int>(type: "int", nullable: false),
                    MaMau = table.Column<int>(type: "int", nullable: false),
                    MaKichThuoc = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__CHITIETHOA__MaSP__0A9D95DB",
                        column: x => x.MaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                    table.ForeignKey(
                        name: "FK__CHITIETHO__MaHoa__09A971A2",
                        column: x => x.MaHoaDon,
                        principalTable: "HOADON",
                        principalColumn: "MaHoaDon");
                    table.ForeignKey(
                        name: "FK__CHITIETHO__MaKic__0D7A0286",
                        column: x => x.MaKichThuoc,
                        principalTable: "KICHTHUOC",
                        principalColumn: "MaKichThuoc");
                    table.ForeignKey(
                        name: "FK__CHITIETHO__MaMau__0B91BA14",
                        column: x => x.MaMau,
                        principalTable: "MAUSAC",
                        principalColumn: "MaMau");
                });

            migrationBuilder.CreateTable(
                name: "CHITIETSANPHAM",
                columns: table => new
                {
                    MaSP = table.Column<int>(type: "int", nullable: false),
                    MaMau = table.Column<int>(type: "int", nullable: false),
                    MaKichThuoc = table.Column<int>(type: "int", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SANPHAM___81A20C7D87F7213C", x => new { x.MaSP, x.MaMau, x.MaKichThuoc });
                    table.ForeignKey(
                        name: "FK__SANPHAM_TH__MaSP__70DDC3D8",
                        column: x => x.MaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                    table.ForeignKey(
                        name: "FK__SANPHAM_T__MaKic__72C60C4A",
                        column: x => x.MaKichThuoc,
                        principalTable: "KICHTHUOC",
                        principalColumn: "MaKichThuoc");
                    table.ForeignKey(
                        name: "FK__SANPHAM_T__MaMau__71D1E811",
                        column: x => x.MaMau,
                        principalTable: "MAUSAC",
                        principalColumn: "MaMau");
                });

            migrationBuilder.CreateTable(
                name: "HINHANH",
                columns: table => new
                {
                    MaHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HinhAnh = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    MaSP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HINHANH__A9C37A9BF334F41B", x => x.MaHinhAnh);
                    table.ForeignKey(
                        name: "FK__HINHANH__MaSP__6477ECF3",
                        column: x => x.MaSP,
                        principalTable: "SANPHAM",
                        principalColumn: "MaSP");
                });

            migrationBuilder.InsertData(
                table: "MaCoupons",
                columns: new[] { "Code", "MinimumOrderAmount", "NgayHetHan", "NgayTao", "PhanTramGiam", "SoTienGiam", "TrangThai" },
                values: new object[,]
                {
                    { "COUPON001", 200000m, new DateTime(2025, 2, 15, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5295), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5266), null, 50000m, true },
                    { "COUPON002", 300000m, new DateTime(2025, 1, 31, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5312), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5309), 10f, null, true },
                    { "COUPON003", 500000m, new DateTime(2025, 2, 5, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5317), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5316), null, 100000m, false },
                    { "COUPON004", 400000m, new DateTime(2025, 1, 26, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5323), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5322), 20f, null, true },
                    { "COUPON005", 600000m, new DateTime(2025, 2, 10, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5328), new DateTime(2025, 1, 16, 23, 22, 55, 63, DateTimeKind.Local).AddTicks(5327), null, 150000m, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETHOADON_MaHoaDon",
                table: "CHITIETHOADON",
                column: "MaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETHOADON_MaKichThuoc",
                table: "CHITIETHOADON",
                column: "MaKichThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETHOADON_MaMau",
                table: "CHITIETHOADON",
                column: "MaMau");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETHOADON_MaSP",
                table: "CHITIETHOADON",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETSANPHAM_MaKichThuoc",
                table: "CHITIETSANPHAM",
                column: "MaKichThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETSANPHAM_MaMau",
                table: "CHITIETSANPHAM",
                column: "MaMau");

            migrationBuilder.CreateIndex(
                name: "IX_HINHANH_MaSP",
                table: "HINHANH",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_MaKH",
                table: "HOADON",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_HOADON_MaNV",
                table: "HOADON",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_MaDanhMuc",
                table: "SANPHAM",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_MaNhaCC",
                table: "SANPHAM",
                column: "MaNhaCC");

            migrationBuilder.CreateIndex(
                name: "IX_SANPHAM_MaThuongHieu",
                table: "SANPHAM",
                column: "MaThuongHieu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIETHOADON");

            migrationBuilder.DropTable(
                name: "CHITIETSANPHAM");

            migrationBuilder.DropTable(
                name: "HINHANH");

            migrationBuilder.DropTable(
                name: "MaCoupons");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "HOADON");

            migrationBuilder.DropTable(
                name: "KICHTHUOC");

            migrationBuilder.DropTable(
                name: "MAUSAC");

            migrationBuilder.DropTable(
                name: "SANPHAM");

            migrationBuilder.DropTable(
                name: "KHACHHANG");

            migrationBuilder.DropTable(
                name: "NHANVIEN");

            migrationBuilder.DropTable(
                name: "NHACUNGCAP");

            migrationBuilder.DropTable(
                name: "DANHMUC");

            migrationBuilder.DropTable(
                name: "THUONGHIEU");
        }
    }
}
