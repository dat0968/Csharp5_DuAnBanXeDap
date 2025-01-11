using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
namespace APIBanXeDap.Repository.MaCoupon
{
    public interface IMaCouponRepository
    {
        public List<MaCouponVM> GetAll(string? keywords, bool? status, string? sort);
        public MaCouponVM Create(MaCouponVM maCoupon);
        public void Update(MaCouponVM maCoupon);
        public void Delete(string id);
    }
}
