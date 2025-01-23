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
        return _repository.GetAll(keyword, sort)
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
        // Validate các trường cần thiết
        var khachHang = _repository.Update(id, khachHangVM);
        return new KhachHangVM
        {
            HoTen = khachHang.HoTen,
            Email = khachHang.Email
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
    public KhachHangVM GetKhachHangById(int id)
    {
        var khachHang = _repository.GetKhachHangById(id);
        if (khachHang == null)
        {
            return null;
        }
        var t =
         new KhachHangVM
        {
            MaKh = khachHang.MaKh,
            HoTen = khachHang.HoTen,
            GioiTinh = khachHang.GioiTinh,
            NgaySinh = khachHang.NgaySinh,
            DiaChi = khachHang.DiaChi,
            Cccd = khachHang.Cccd,
            Sdt = khachHang.Sdt,
            Email = khachHang.Email,
            TenTaiKhoan = khachHang.TenTaiKhoan,
            TinhTrang = khachHang.TinhTrang,
            Hinh = khachHang.Hinh,
            MatKhau = khachHang.MatKhau,
        };
        return t;
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
    public PagedResult<KhachHangVM> GetPagedKhachHang(int pageNumber, int pageSize, string? keyword, string? sort)
    {
        var totalItems = _repository.GetTotalCount(keyword);
        var khachHangs = _repository.GetPaged(pageNumber, pageSize, keyword, sort)
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
