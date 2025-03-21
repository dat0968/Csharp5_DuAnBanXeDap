﻿using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace MVCBanXeDap.Controllers
{
    public class CouponCodeController : Controller
    {
        Uri uri = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;
        public CouponCodeController(IjwtToken jwtToken)
        {
            this.jwtToken = jwtToken;
            this._client = new HttpClient();
            _client.BaseAddress = uri;
        }
        [NonAction]
        [HttpGet]
        public async Task<string?> SetAuthorizationHeader()
        {
            var validateAccessToken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                var accesstoken = validateAccessToken;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                return accesstoken;
            }
            return null;
        }
        public async Task<IActionResult> Index(string? keywords, string? status, string? sort ,int page = 1)
        {
            var accesstoken = await SetAuthorizationHeader();
            var listCouponCode = new List<MaCouponVM>();
            HttpResponseMessage responseGetAllResponse = _client.GetAsync(_client.BaseAddress + $"MaCoupons/GetAllCouponCodeByPage?keywords={keywords}&status={status}&page={page}").Result;
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
                    ViewBag.Token = accesstoken;
                    var infomation = jwtToken.GetInformationUserFromToken(accesstoken);
                    ViewBag.Role = infomation.VaiTro;
                }
            }
            else
            {
                return StatusCode((int)responseGetAllResponse.StatusCode);
            };
            return View(listCouponCode);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCouponCode(float SoPhanTramGiam, decimal SoTienGiam, DateTime NgayHetHan, decimal GiaToiThieu)
        {
            SetAuthorizationHeader();
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
            else
            {
                return StatusCode((int)response.StatusCode);
            };
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> EditCouponCode(MaCouponVM model)
        {
            SetAuthorizationHeader();
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
            else
            {
                return StatusCode((int)response.StatusCode);
            };
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CancelCouponCode(string id)
        {
            SetAuthorizationHeader();
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"MaCoupons/Cancel?id={id}", null);
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
            else
            {
                return StatusCode((int)response.StatusCode);
            };
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult GetPartialView([FromBody] MaCouponVM model)
        {
            return PartialView("~/Views/Shared/_CouponCodeEdit.cshtml", model);
        }


    }
}
