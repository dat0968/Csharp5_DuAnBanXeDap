using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APIBanXeDap.Repository.VanChuyen
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly Csharp5Context db;

        public ShippingRepository(Csharp5Context db)
        {
            this.db = db;
        }

        public ShippingVM Create(ShippingVM ship)
        {
            var checkInfoShipping = db.Vanchuyens.FirstOrDefault(p => p.QuanHuyen == ship.QuanHuyen && p.Phuong == ship.Phuong && p.ThanhPho == ship.ThanhPho);
            if(checkInfoShipping != null)
            {
                return null;
            }
            var createShipping = new Vanchuyen
            {
                QuanHuyen = ship.QuanHuyen,
                Phuong = ship.Phuong,
                ThanhPho = ship.ThanhPho,
                Gia = ship.Gia,
            };
            db.Vanchuyens.Add(createShipping);
            db.SaveChanges();
            return ship;
        }

        public void Delete(int id)
        {
            var findShipping = db.Vanchuyens.FirstOrDefault(p => p.Id == id);
            if(findShipping != null)
            {
                db.Vanchuyens.Remove(findShipping);
                db.SaveChanges();
            }
        }

        public void Edit(ShippingVM ship)
        {
            var model = new Vanchuyen
            {
                Id = ship.Id,
                QuanHuyen = ship.QuanHuyen,
                Phuong = ship.Phuong,
                ThanhPho = ship.ThanhPho,
                Gia = ship.Gia, 
            };
            db.Vanchuyens.Update(model);
            db.SaveChanges();
        }

        public List<ShippingVM> GetAll(string? keywords, string? priceFilter, string? SortByPrice)
        {
            var getshipping = db.Vanchuyens.AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                getshipping = getshipping.Where(p => p.QuanHuyen.Contains(keywords) || p.ThanhPho.Contains(keywords) || p.Phuong.Contains(keywords));
            }
            switch (priceFilter)
            {
                case "under20000":
                    getshipping = getshipping.Where(p => p.Gia < 20000);
                    break;
                case "over20000":
                    getshipping = getshipping.Where(p => p.Gia >= 20000);
                    break;
                default:
                    getshipping = getshipping;
                    break;
            }
            switch (SortByPrice)
            {
                case "asc":
                    getshipping = getshipping.OrderBy(p => p.Gia);
                    break;
                case "desc":
                    getshipping = getshipping.OrderByDescending(p => p.Gia);
                    break;
                default:
                    getshipping = getshipping;
                    break;
            }
            var listShip = new List<ShippingVM>();
            if(getshipping != null)
            {
                foreach (var ship in getshipping)
                {
                    listShip.Add(new ShippingVM
                    {
                        Id = ship.Id,
                        Gia = ship.Gia,
                        QuanHuyen = ship.QuanHuyen,
                        Phuong = ship.Phuong,
                        ThanhPho = ship.ThanhPho,
                    });
                }
            }
            
            return listShip;
        }

        public float? GetShippingFee(string pho, string quan, string phuong)
        {
            var findShippingfee = db.Vanchuyens.FirstOrDefault(p => p.ThanhPho == pho && p.QuanHuyen == quan && p.Phuong == phuong);
            if(findShippingfee != null)
            {
                return findShippingfee.Gia;
            }
            return 0;
        }
    }
}
