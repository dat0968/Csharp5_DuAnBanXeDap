using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing.Text;
using MVCBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MVCBanXeDap.ViewComponents
{
    public class CouponCodeViewComponent : ViewComponent
    {

        Uri uri = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        public CouponCodeViewComponent()
        {
            this._client = new HttpClient();
            _client.BaseAddress = uri;
        }
        public IViewComponentResult Invoke()
        {
            var listCouponCode = new List<MaCouponVM>();
            HttpResponseMessage responseGetAllResponse = _client.GetAsync(_client.BaseAddress + $"MaCoupons/GetAll").Result;
            if (responseGetAllResponse.IsSuccessStatusCode)
            {
                string data = responseGetAllResponse.Content.ReadAsStringAsync().Result;
                var convertResponse = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = convertResponse["success"].Value<bool>();
                if (isSuccess)
                {
                    var listCouponCodeResponse = convertResponse["data"].ToObject<List<MaCouponVM>>();
                    listCouponCode = listCouponCodeResponse;
                    return View(listCouponCode);
                }
            }
            return View(listCouponCode);
        }
    }
}
