using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

public class KhachHangService : IKhachHangService
{
    private readonly IKhachHangRepository _repository;

    public KhachHangService(IKhachHangRepository repository)
    {
        _repository = repository;
    }


    public KhachHangVM AddKhachHang(KhachHangVM khachHangVM)
    {
        // Validate unique fields
        if (_repository.IsCccdExists(khachHangVM.Cccd))
        {
            throw new Exception("CCCD đã tồn tại");
        }

        if (_repository.IsSdtExists(khachHangVM.Sdt))
        {
            throw new Exception("Số điện thoại đã tồn tại");
        }

        if (_repository.IsEmailExists(khachHangVM.Email))
        {
            throw new Exception("Email đã tồn tại");
        }

        if (_repository.IsTenTaiKhoanExists(khachHangVM.TenTaiKhoan))
        {
            throw new Exception("Tên tài khoản đã tồn tại");
        }

        // Thêm khách hàng
        var khachHang = _repository.Add(khachHangVM);
        return new KhachHangVM
        {
            HoTen = khachHang.HoTen,
            Email = khachHang.Email
        };
    }

    public List<KhachHangVM> GetAllKhachHang(string? keyword, string? sort)
    {
        return _repository.GetAll(keyword, sort, null, null) // Truyền null cho status và gender
            .Select(kh => new KhachHangVM
            {
                MaKh = kh.MaKh,
                HoTen = kh.HoTen,
                GioiTinh = kh.GioiTinh,
                NgaySinh = kh.NgaySinh,
                DiaChi = kh.DiaChi,
                Cccd = kh.Cccd,
                Sdt = kh.Sdt,
                Email = kh.Email,
                TenTaiKhoan = kh.TenTaiKhoan,
                TinhTrang = kh.TinhTrang,
                Hinh = kh.Hinh
            })
            .ToList();
    }


    public KhachHangVM UpdateKhachHang(int id, KhachHangVM khachHangVM)
    {
        // Nếu IsDelete là true, đặt các trường cần thiết thành null
        if (khachHangVM.IsDelete == true)
        {
            khachHangVM.TenTaiKhoan = null;
            khachHangVM.MatKhau = null;
            khachHangVM.Email = null;
        }

        // Chuyển đổi từ string về DateOnly?
        var ngaySinh = khachHangVM.NgaySinh;

        var khachHang = _repository.Update(id, new KhachHangVM
        {
            HoTen = khachHangVM.HoTen,
            GioiTinh = khachHangVM.GioiTinh,
            NgaySinh = ngaySinh,
            DiaChi = khachHangVM.DiaChi,
            Cccd = khachHangVM.Cccd,
            Sdt = khachHangVM.Sdt,
            Email = khachHangVM.Email,
            TenTaiKhoan = khachHangVM.TenTaiKhoan,
            MatKhau = khachHangVM.MatKhau,
            TinhTrang = khachHangVM.TinhTrang,
            Hinh = khachHangVM.Hinh,
            IsDelete = khachHangVM.IsDelete
        });
        if (khachHangVM.IsDelete != null)
        {
            khachHang.IsDelete = khachHangVM.IsDelete;
        }
        return new KhachHangVM
        {
            HoTen = khachHang.HoTen,
            Email = khachHang.Email
        };
    }


   public void ToggleIsDelete(int id)
{
    var khachHang = _repository.GetKhachHangById(id);
    if (khachHang == null)
    {
        throw new Exception("Không tìm thấy khách hàng");
    }

    // Cập nhật IsDelete và xóa thông tin nhạy cảm
    var updatedKhachHang = new KhachHangVM
    {
        HoTen = khachHang.HoTen,
        GioiTinh = khachHang.GioiTinh,
        NgaySinh = khachHang.NgaySinh,
        DiaChi = khachHang.DiaChi,
        Cccd = khachHang.Cccd,
        Sdt = khachHang.Sdt,
        Email = null, // Xóa email
        TenTaiKhoan = null, // Xóa tài khoản
        MatKhau = null, // Xóa mật khẩu
        TinhTrang = khachHang.TinhTrang,
        Hinh = khachHang.Hinh,
        IsDelete = true // Đặt IsDelete là true
    };

    _repository.Update(id, updatedKhachHang);
}


    public void ToggleStatus(int id, string isActive)
    {
        _repository.ToggleStatus(id, isActive);
    }
        public KhachHangVM GetKhachHangById(int id)
        {
            var khachHang = _repository.GetKhachHangById(id);
            if (khachHang == null)
            {
                return null;
            }

            return new KhachHangVM
            {
                MaKh = khachHang.MaKh,
                HoTen = khachHang.HoTen,
                GioiTinh = khachHang.GioiTinh,
                NgaySinh = khachHang.NgaySinh, // Convert DateOnly? to string
                DiaChi = khachHang.DiaChi,  
                Cccd = khachHang.Cccd,
                Sdt = khachHang.Sdt,
                Email = khachHang.Email,
                TenTaiKhoan = khachHang.TenTaiKhoan,
                TinhTrang = khachHang.TinhTrang,
                Hinh = khachHang.Hinh,
                MatKhau = khachHang.MatKhau,
            };
        }

    public void ImportKhachHangs(List<KhachHangVM> khachHangList)
    {
        foreach (var khachHangVM in khachHangList)
        {
            if (_repository.IsCccdExists(khachHangVM.Cccd))
            {
                throw new Exception($"CCCD '{khachHangVM.Cccd}' đã tồn tại.");
            }

            if (_repository.IsSdtExists(khachHangVM.Sdt))
            {
                throw new Exception($"Số điện thoại '{khachHangVM.Sdt}' đã tồn tại.");
            }

            if (_repository.IsEmailExists(khachHangVM.Email))
            {
                throw new Exception($"Email '{khachHangVM.Email}' đã tồn tại.");
            }

            if (_repository.IsTenTaiKhoanExists(khachHangVM.TenTaiKhoan))
            {
                throw new Exception($"Tên tài khoản '{khachHangVM.TenTaiKhoan}' đã tồn tại.");
            }

            var khachHang = new Khachhang
            {
                HoTen = khachHangVM.HoTen,
                GioiTinh = khachHangVM.GioiTinh,
                NgaySinh = khachHangVM.NgaySinh,
                DiaChi = khachHangVM.DiaChi,
                Cccd = khachHangVM.Cccd,
                Sdt = khachHangVM.Sdt,
                Email = khachHangVM.Email,
                TenTaiKhoan = khachHangVM.TenTaiKhoan,
                MatKhau = khachHangVM.MatKhau,
                TinhTrang = "Đang hoạt động",
                IsDelete = false
            };

            _repository.Add(khachHang);
        }
    }
    public PagedResult<KhachHangVM> GetPagedKhachHang(int pageNumber, int pageSize, string? keyword, string? sort, string? status, string? gender)
    {
        // Lấy tổng số lượng khách hàng
        var totalItems = _repository.GetTotalCount(keyword, status, gender);

        // Lấy danh sách khách hàng
        var khachHangs = _repository.GetPaged(pageNumber, pageSize, keyword, sort, status, gender)
            .Select(kh => new KhachHangVM
            {
                MaKh = kh.MaKh,
                HoTen = kh.HoTen,
                GioiTinh = kh.GioiTinh,
                NgaySinh = kh.NgaySinh,
                DiaChi = kh.DiaChi,
                Cccd = kh.Cccd,
                Sdt = kh.Sdt,
                Email = kh.Email,
                TenTaiKhoan = kh.TenTaiKhoan,
                TinhTrang = kh.TinhTrang,
                Hinh = kh.Hinh
            })
            .ToList();

        return new PagedResult<KhachHangVM>
        {
            Items = khachHangs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }





}
