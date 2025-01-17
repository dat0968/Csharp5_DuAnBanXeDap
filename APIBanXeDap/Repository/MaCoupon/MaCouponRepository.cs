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

        public void Delete(string id)
        {
            var FindMaCoupon = db.MaCoupons.FirstOrDefault(p => p.Code == id);
            if (FindMaCoupon != null)
            {
                db.MaCoupons.Remove(FindMaCoupon);
                db.SaveChanges();
            }
        }

        public List<MaCouponVM> GetAll(string? keywords, bool? status, string? sort)
        {
            var listCouponCode = db.MaCoupons.AsQueryable();
            var expiredCoupons = listCouponCode.Where(c => c.NgayHetHan < DateTime.Now && c.TrangThai == true).ToList();
            foreach (var coupon in expiredCoupons)
            {
                coupon.TrangThai = false;
            }
            db.UpdateRange(expiredCoupons);
            db.SaveChanges();
            var CovertToListMaCouponVM = new List<MaCouponVM>();
            
            if (!string.IsNullOrEmpty(keywords))
            {
                listCouponCode = listCouponCode.Where(p => p.Code.Contains(keywords));
            }
            switch (status)
            {
                case true:
                    listCouponCode = listCouponCode.Where(p => p.TrangThai == true);
                    break;
                case false:
                    listCouponCode = listCouponCode.Where(p => p.TrangThai == false);
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
                //if (item.NgayHetHan < DateTime.Now)
                //{
                //    item.TrangThai = false;
                //    db.MaCoupons.Update(item);
                //    db.SaveChanges();
                //}
                CovertToListMaCouponVM.Add(new MaCouponVM
                {
                    Code = item.Code,
                    PhanTramGiam = item.PhanTramGiam,
                    SoTienGiam = item.SoTienGiam,
                    NgayHetHan = item.NgayHetHan,
                    TrangThai = item.TrangThai,
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
                PhanTramGiam = maCoupon.PhanTramGiam,
                SoTienGiam = maCoupon.SoTienGiam,
                NgayHetHan = maCoupon.NgayHetHan,
                TrangThai = maCoupon.TrangThai,
                MinimumOrderAmount = maCoupon.MinimumOrderAmount,
            };
            db.MaCoupons.Update(editCouponCode);
            db.SaveChanges();
        }
    }
}
