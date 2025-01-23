using APIBanXeDap.Models;
using APIBanXeDap.Models.ViewModels;
using System.Linq.Expressions;

namespace APIBanXeDap.Repository.ThongKe
{
    public interface IThongKeRepository
    {
        Task<IEnumerable<NhanVienVM>> GetEmployeeOrderStatsAsync(Expression<Func<Nhanvien, bool>>? filter = null);
        Task<IEnumerable<(int, int)>> GetStatUser();
    }
}
