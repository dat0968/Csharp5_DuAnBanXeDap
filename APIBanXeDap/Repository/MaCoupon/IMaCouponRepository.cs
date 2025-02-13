using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
namespace APIBanXeDap.Repository.MaCoupon
{
    public interface IMaCouponRepository
    {
        public List<MaCouponVM> GetAll(string? keywords, string? status, string? sort);
        public MaCouponVM Create(MaCouponVM maCoupon);
        public void Update(MaCouponVM maCoupon);
        public void Cancel(string id);
        public void RevokeCouponCode(string id);
    }
}
