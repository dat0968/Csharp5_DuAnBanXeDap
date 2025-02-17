using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace APIBanXeDap.Repository.YeuThich
{
    public interface IYeuThichRepository
    {
        Task<IEnumerable<WishlistVM>> GetAllYeuThichVMAsync(Expression<Func<Yeuthich, bool>>? filter = null, string? includeProperties = null);
        Task<WishlistVM> GetAsync(Expression<Func<Yeuthich, bool>> filter, string? includeProperties = null, bool tracked = false);
        Task<IActionResult> ChangeWishlist(int idProduct, string typeObject, int idUser);
        Task<bool> IsOneInWishlist(int idProduct, int idUser);
        Task<IEnumerable<bool>> IsManyInWishlist(int[] idProducts, int idUser);
    }
}
