using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace APIBanXeDap.Repository.MaCoupon
{
    public class MaCouponRepository : IMaCouponRepository
    {
        private readonly Csharp5Context db;

        public MaCouponRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public MaCouponVM Create(MaCouponVM maCoupon)
        {
            var newCouponCode = new APIBanXeDap.Models.MaCoupon
            {
                Code = maCoupon.Code,
                SoTienGiam = maCoupon.SoTienGiam > 0 ? maCoupon.SoTienGiam : null,
                PhanTramGiam = maCoupon.PhanTramGiam > 0 ? maCoupon.PhanTramGiam : null,
                NgayHetHan = maCoupon.NgayHetHan,
                TrangThai = true,
                MinimumOrderAmount = maCoupon.MinimumOrderAmount,
            };
            db.MaCoupons.Add(newCouponCode);
            db.SaveChanges();
            return maCoupon;
        }

        public void Cancel(string id)
        {
            var FindMaCoupon = db.MaCoupons.FirstOrDefault(p => p.Code == id);
            if (FindMaCoupon != null)
            {
                FindMaCoupon.TrangThai = false;
                db.SaveChanges();
            }
        }

        public List<MaCouponVM> GetAll(string? keywords, string? status, string? sort)
        {
            var listCouponCode = db.MaCoupons.AsQueryable();
            var CovertToListMaCouponVM = new List<MaCouponVM>();
            
            if (!string.IsNullOrEmpty(keywords))
            {
                listCouponCode = listCouponCode.Where(p => p.Code.Contains(keywords));
            }
            switch (status)
            {
                case "Còn hiệu lực":
                    listCouponCode = listCouponCode.Where(p => p.TrangThai == true && p.DaSuDung == false && p.NgayHetHan > DateTime.Now);
                    break;
                case "Đã hủy":
                    listCouponCode = listCouponCode.Where(p => p.TrangThai == false);
                    break;
                case "Đã sử dụng":
                    listCouponCode = listCouponCode.Where(p => p.DaSuDung == true);
                    break;
                case "Đã hết hạn":
                    listCouponCode = listCouponCode.Where(p => p.NgayHetHan < DateTime.Now);
                    break;   
                default:
                    listCouponCode = listCouponCode.OrderByDescending(p => p.NgayTao);
                    break;
            }
            switch (sort)
            {
                case "asc":
                    listCouponCode = listCouponCode.OrderBy(p => p.NgayTao);
                    break;
                default:
                    listCouponCode = listCouponCode.OrderByDescending(p => p.NgayTao);
                    break;
            }
            foreach (var item in listCouponCode)
            {
                CovertToListMaCouponVM.Add(new MaCouponVM
                {
                    Code = item.Code,
                    PhanTramGiam = item.PhanTramGiam,
                    SoTienGiam = item.SoTienGiam,
                    NgayHetHan = item.NgayHetHan,
                    TrangThai = item.TrangThai,
                    NgayTao = item.NgayTao,
                    DaSuDung = item.DaSuDung,
                    MinimumOrderAmount = item.MinimumOrderAmount,
                });
            }
            return CovertToListMaCouponVM;
        }

        public void Update(MaCouponVM maCoupon)
        {
            var editCouponCode = new APIBanXeDap.Models.MaCoupon
            {
                Code = maCoupon.Code,
                SoTienGiam = maCoupon.SoTienGiam > 0 ? maCoupon.SoTienGiam : null,
                PhanTramGiam = maCoupon.PhanTramGiam > 0 ? maCoupon.PhanTramGiam : null,
                NgayHetHan = maCoupon.NgayHetHan,
                TrangThai = maCoupon.TrangThai,
                NgayTao = maCoupon.NgayTao,
                MinimumOrderAmount = maCoupon.MinimumOrderAmount,
                DaSuDung = maCoupon.DaSuDung,
            };
            db.MaCoupons.Update(editCouponCode);
            db.SaveChanges();
        }
        public void RevokeCouponCode(string id)
        {
            var findCouponCode = db.MaCoupons.FirstOrDefault(p => p.Code == id);
            if(findCouponCode != null)
            {
                findCouponCode.DaSuDung = true;
                db.MaCoupons.Update(findCouponCode);
                db.SaveChanges();
            }
        }
    }
}
