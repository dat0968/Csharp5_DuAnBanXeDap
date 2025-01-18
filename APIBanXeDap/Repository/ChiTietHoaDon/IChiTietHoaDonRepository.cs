using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using System.Linq.Expressions;

namespace APIBanXeDap.Repository.ChiTietHoaDon
{
    public interface IChiTietHoaDonRepository
    {
        Task<List<ChiTietHoaDonVM>> GetAllDetailInvoiceAsync(Expression<Func<Chitiethoadon, bool>>? filter = null);
        Task<ChiTietHoaDonVM> GetDetailInvoiceByIdAsync(Expression<Func<Chitiethoadon, bool>> filter);
    }
}
