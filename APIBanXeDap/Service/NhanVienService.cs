using APIBanXeDap.Models;
using APIBanXeDap.Models.ViewModels;

//using APIBanXeDap.Repository.NhanVien;
using APIBanXeDap.ViewModels;



public class NhanVienService : INhanVienService
{
    private readonly INhanVienRepository _repository;

    public NhanVienService(INhanVienRepository repository)
    {
        _repository = repository;
    }

    public NhanVienVM AddNhanVien(NhanVienVM nhanVienVM)
    {
        // Validate unique fields
        if (_repository.IsCccdExists(nhanVienVM.Cccd))
        {
            throw new Exception("CCCD đã tồn tại");
        }

        if (_repository.IsSdtExists(nhanVienVM.Sdt))
        {
            throw new Exception("Số điện thoại đã tồn tại");
        }

        if (_repository.IsEmailExists(nhanVienVM.Email))
        {
            throw new Exception("Email đã tồn tại");
        }

        if (_repository.IsTenTaiKhoanExists(nhanVienVM.TenTaiKhoan))
        {
            throw new Exception("Tên tài khoản đã tồn tại");
        }

        // Thêm nhân viên
        var nhanVien = new Nhanvien
        {
            HoTen = nhanVienVM.HoTen,
            GioiTinh = nhanVienVM.GioiTinh,
            NgaySinh = nhanVienVM.NgaySinh,
            DiaChi = nhanVienVM.DiaChi,
            Cccd = nhanVienVM.Cccd,
            Sdt = nhanVienVM.Sdt,
            Email = nhanVienVM.Email,
            VaiTro = nhanVienVM.VaiTro,
            Luong = nhanVienVM.Luong,
            TenTaiKhoan = nhanVienVM.TenTaiKhoan,
            MatKhau = nhanVienVM.MatKhau,
            NgayVaoLam = nhanVienVM.NgayVaoLam,
            TinhTrang = "Đang hoạt động",
            IsDelete = false
        };

        _repository.Add(nhanVien);



        return new NhanVienVM
        {
            HoTen = nhanVien.HoTen,
            Email = nhanVien.Email
        };
    }

    public List<NhanVienVM> GetAllNhanVien(string? keyword, string? sort, string? status, string? gender)
    {
        return _repository.GetAll(keyword, sort, status, gender)
            .Select(nv => new NhanVienVM
            {
                MaNv = nv.MaNv,
                HoTen = nv.HoTen,
                GioiTinh = nv.GioiTinh,
                NgaySinh = nv.NgaySinh,
                DiaChi = nv.DiaChi,
                Cccd = nv.Cccd,
                Sdt = nv.Sdt,
                Email = nv.Email,
                VaiTro = nv.VaiTro,
                Luong = nv.Luong,
                TenTaiKhoan = nv.TenTaiKhoan,
                TinhTrang = nv.TinhTrang,
                Hinh = nv.Hinh,
                NgayVaoLam = nv.NgayVaoLam,
            }).ToList();
    }

    public NhanVienVM UpdateNhanVien(int id, NhanVienVM nhanVienVM)
    {
        // Validate các trường cần thiết
        var nhanVien = _repository.Update(id, nhanVienVM);
        return new NhanVienVM
        {
            HoTen = nhanVien.HoTen,
            Email = nhanVien.Email
        };
    }

    public void ToggleIsDelete(int id)
    {
        _repository.ToggleIsDelete(id);
    }

    public void ToggleStatus(int id, string isActive)
    {
        _repository.ToggleStatus(id, isActive);
    }

    public NhanVienVM GetNhanVienById(int id)
    {
        var nhanVien = _repository.GetNhanVienById(id);
        if (nhanVien == null)
        {
            return null;
        }
        return new NhanVienVM
        {
            MaNv = nhanVien.MaNv,
            HoTen = nhanVien.HoTen,
            GioiTinh = nhanVien.GioiTinh,
            NgaySinh = nhanVien.NgaySinh,
            DiaChi = nhanVien.DiaChi,
            Cccd = nhanVien.Cccd,
            Sdt = nhanVien.Sdt,
            Email = nhanVien.Email,
            VaiTro = nhanVien.VaiTro,
            Luong = nhanVien.Luong,
            TenTaiKhoan = nhanVien.TenTaiKhoan,
            TinhTrang = nhanVien.TinhTrang,
            Hinh = nhanVien.Hinh,
            MatKhau = nhanVien.MatKhau,
            NgayVaoLam = nhanVien.NgayVaoLam
        };
    }

    public void ImportNhanViens(List<NhanVienVM> nhanVienList)
    {
        foreach (var nhanVienVM in nhanVienList)
        {
            if (_repository.IsCccdExists(nhanVienVM.Cccd))
            {
                throw new Exception($"CCCD '{nhanVienVM.Cccd}' đã tồn tại.");
            }

            if (_repository.IsSdtExists(nhanVienVM.Sdt))
            {
                throw new Exception($"Số điện thoại '{nhanVienVM.Sdt}' đã tồn tại.");
            }

            if (_repository.IsEmailExists(nhanVienVM.Email))
            {
                throw new Exception($"Email '{nhanVienVM.Email}' đã tồn tại.");
            }

            if (_repository.IsTenTaiKhoanExists(nhanVienVM.TenTaiKhoan))
            {
                throw new Exception($"Tên tài khoản '{nhanVienVM.TenTaiKhoan}' đã tồn tại.");
            }

            var nhanVien = new Nhanvien
            {
                HoTen = nhanVienVM.HoTen,
                GioiTinh = nhanVienVM.GioiTinh,
                NgaySinh = nhanVienVM.NgaySinh,
                DiaChi = nhanVienVM.DiaChi,
                Cccd = nhanVienVM.Cccd,
                Sdt = nhanVienVM.Sdt,
                Email = nhanVienVM.Email,
                VaiTro = nhanVienVM.VaiTro,
                Luong = nhanVienVM.Luong,
                TenTaiKhoan = nhanVienVM.TenTaiKhoan,
                MatKhau = nhanVienVM.MatKhau,
                TinhTrang = "Đang hoạt động",
                IsDelete = false
            };

            _repository.Add(nhanVien);
        }
    }



    public PagedResult<NhanVienVM> GetPagedNhanVien(int pageNumber, int pageSize, string? keyword, string? sort, string? status, string? gender)
    {
        var totalItems = _repository.GetAll(keyword, sort, status, gender).Count;
        var nhanViens = _repository.GetPaged(pageNumber, pageSize, keyword, sort, status, gender)
            .Select(nv => new NhanVienVM
            {
                MaNv = nv.MaNv,
                HoTen = nv.HoTen,
                GioiTinh = nv.GioiTinh,
                NgaySinh = nv.NgaySinh,
                DiaChi = nv.DiaChi,
                Cccd = nv.Cccd,
                Sdt = nv.Sdt,
                Email = nv.Email,
                VaiTro = nv.VaiTro,
                Luong = nv.Luong,
                TenTaiKhoan = nv.TenTaiKhoan,
                TinhTrang = nv.TinhTrang,
                Hinh = nv.Hinh
            }).ToList();

        return new PagedResult<NhanVienVM>
        {
            Items = nhanViens,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
