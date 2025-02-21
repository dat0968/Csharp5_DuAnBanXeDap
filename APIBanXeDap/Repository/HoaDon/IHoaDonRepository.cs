using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace APIBanXeDap.Repository.HoaDon
{
    public interface IHoaDonRepository
    {
        Task<IEnumerable<HoadonVM>> GetAllHoadonVMAsync(Expression<Func<Hoadon, bool>>? filter = null, string? includeProperties = null);
        Task<HoadonVM> GetAsync(Expression<Func<Hoadon, bool>> filter, string? includeProperties = null, bool tracked = false);
        Task<string?> ChangeStatusOrder(int idOrder, int idStaff, string statusOrder, string? reason);
        Task<InvoiceVM> GetInvoiceDataAsync(int maHoaDon);
        Task<string?> GetOrderStatusById(int maHoaDon);
        Task<IEnumerable<InvoiceVM>> GetAllInvoiceDataAsync();
        //Task<bool> SetReasonCancel(int maHoaDon, string reason);
    }
}
