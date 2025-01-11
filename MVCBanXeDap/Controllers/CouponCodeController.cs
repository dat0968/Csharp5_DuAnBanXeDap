using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

namespace MVCBanXeDap.Controllers
{
    public class CouponCodeController : Controller
    {
        Uri uri = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        public CouponCodeController()
        {
            this._client = new HttpClient();
            _client.BaseAddress = uri;
        }
        public IActionResult Index(string? keywords, bool? status, string? sort ,int page = 1)
        {
            var listCouponCode = new List<MaCouponVM>();
            HttpResponseMessage responseGetAllResponse = _client.GetAsync(_client.BaseAddress + $"MaCoupons/GetAllCouponCode?keywords={keywords}&status={status}&page={page}").Result;
            if(responseGetAllResponse.IsSuccessStatusCode)
            {
                string data = responseGetAllResponse.Content.ReadAsStringAsync().Result;
                var convertResponse = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = convertResponse["success"].Value<bool>();
                if (isSuccess)
                {
                    var listCouponCodeResponse = convertResponse["data"].ToObject<List<MaCouponVM>>();
                    listCouponCode = listCouponCodeResponse;
                    ViewBag.TotalPages = convertResponse["totalPages"].Value<int>();
                    ViewBag.Page = convertResponse["page"].Value<int>();
                    ViewBag.Keywords = keywords;
                    ViewBag.Status = status;
                    ViewBag.Sort = sort;
                }
            };
            return View(listCouponCode);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCouponCode(float SoPhanTramGiam, decimal SoTienGiam, DateTime NgayHetHan, decimal GiaToiThieu)
        {
            var model = new MaCouponVM
            {
                SoTienGiam = SoTienGiam,
                PhanTramGiam = SoPhanTramGiam,
                NgayHetHan = NgayHetHan,
                MinimumOrderAmount = GiaToiThieu,
            };
            var convertModel = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(convertModel, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "MaCoupons/Create", content);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var convertDataresponse = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = convertDataresponse["success"].Value<bool>();
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = convertDataresponse["message"].Value<string>();
                }
                else
                {
                    TempData["ErrorMessage"] = convertDataresponse["message"].Value<string>();
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> EditCouponCode(MaCouponVM model)
        {
            var ConvertModel = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(ConvertModel, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + "MaCoupons/Update", content);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ConvertResponse = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = ConvertResponse["success"].Value<bool>();
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = ConvertResponse["message"].Value<string>();
                }
                else
                {
                    TempData["ErrorMessage"] = ConvertResponse["message"].Value<string>();
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCouponCode(string id)
        {
            HttpResponseMessage response = await _client.DeleteAsync(_client.BaseAddress + $"MaCoupons/Delete?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ConvertResponse = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = ConvertResponse["success"].Value<bool>();
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = ConvertResponse["message"].Value<string>();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
