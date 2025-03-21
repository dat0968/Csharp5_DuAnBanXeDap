﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MVCBanXeDap.Services;
using MVCBanXeDap.Services.Email;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MVCBanXeDap.Controllers
{
    public class AccountsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly IjwtToken jwtToken;

        public AccountsController(IConfiguration config, IjwtToken jwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _config = config;
            this.jwtToken = jwtToken;
        }
        [HttpGet]
        public async Task<IActionResult> Login_Customer()
        {
            var validationtoken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validationtoken))
            {
                var information = jwtToken.GetInformationUserFromToken(validationtoken);
                var role = information.VaiTro;
                if(role == "Admin" || role == "Nhân viên")
                {
                    return RedirectToAction("Index", "Product");
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login_Customer(Login model)
        {
            try
            {
                var ConvertModeltoJson = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(ConvertModeltoJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "Accounts/LoginCustomer", content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<JObject>(data);
                    var isSuccess = responseData["success"].Value<bool>();
                    if (isSuccess)
                    {
                        var token = responseData["data"];
                        if (token != null)
                        {
                            string accessToken = token["accessToken"]?.ToString();
                            string refreshToken = token["refreshToken"]?.ToString();
                            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
                            {
                                HttpContext.Session.SetString("AccessToken", token["accessToken"]?.ToString());
                                HttpContext.Session.SetString("RefreshToken", token["refreshToken"]?.ToString());                               
                                HttpContext.Session.SetString("Role", "Customer");
                                TempData["SuccessMessage"] = "Đăng nhập thành công";
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "AccessToken hoặc RefreshToken không có trong phản hồi");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Data trong phản hồi bị null");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, responseData["message"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Login_Staff()
        {
            var validationtoken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validationtoken))
            {
                var information = jwtToken.GetInformationUserFromToken(validationtoken);
                var role = information.VaiTro;
                if (role == "Admin" || role == "Nhân viên")
                {
                    return RedirectToAction("Index", "Product");
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login_Staff(Login model)
        {
            try
            {
                var ConvertModeltoJson = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(ConvertModeltoJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "Accounts/LoginStaff", content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<JObject>(data);
                    var isSuccess = responseData["success"].Value<bool>();
                    if (isSuccess)
                    {
                        var token = responseData["data"];
                        if (token != null)
                        {
                            string accessToken = token["accessToken"]?.ToString();
                            string refreshToken = token["refreshToken"]?.ToString();
                            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
                            {
                                HttpContext.Session.SetString("AccessToken", token["accessToken"]?.ToString());
                                HttpContext.Session.SetString("RefreshToken", token["refreshToken"]?.ToString());
                                //HttpContext.Session.SetString("Role", "Staff");
                                var validateAccessToken = await jwtToken.ValidateAccessToken();
                                if (!string.IsNullOrEmpty(validateAccessToken))
                                {
                                    var accesstoken = validateAccessToken;
                                    var information = jwtToken.GetInformationUserFromToken(accesstoken);
                                    HttpContext.Session.SetString("HoTen", information.HoTen);
                                    HttpContext.Session.SetString("Hinh", information.Hinh ?? "");
                                    HttpContext.Session.SetString("Role", information.VaiTro);
                                }
                                TempData["SuccessMessage"] = "Đăng nhập thành công";
                                return RedirectToAction("Index", "Product");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "AccessToken hoặc RefreshToken không có trong phản hồi");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Data trong phản hồi bị null");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, responseData["message"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult RegisterAccounts()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAccounts(Register model)
        {
            if (ModelState.IsValid)
            {
                var ConvertmodeltoJson = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(ConvertmodeltoJson, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_client.BaseAddress + "Accounts/Register", content);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<JObject>(data);
                    var isSuccess = responseData["success"].Value<bool>();
                    if (isSuccess)
                    {
                        TempData["SuccessMessage"] = "Đăng ký thành công";
                        return RedirectToAction("Login_Customer", "Accounts");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thực hiện thao tác");             
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> LogoutAccount()
        {
            string refreshToken = HttpContext.Session.GetString("RefreshToken");
            HttpResponseMessage response = await _client.DeleteAsync(_client.BaseAddress + $"Accounts/Logout?RefreshToken={refreshToken}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = responseData["success"].Value<bool>();
                if (isSuccess)
                {
                    HttpContext.Session.Remove("RefreshToken");
                    HttpContext.Session.Remove("AccessToken");
                    //HttpContext.Session.Remove("Role");
                    TempData["SuccessMessage"] = "Đăng xuất thành công";
                }
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPassword_Customer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword_Customer(string? YourEmail)
        {
            if (string.IsNullOrEmpty(YourEmail))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập Email của bạn";
                return RedirectToAction("ForgotPasswordCustomer", "Accounts");
            }
            var response = await _client.GetAsync(_client.BaseAddress + $"Accounts/ForgotPasswordCustomer?email={YourEmail}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ResponseConvert = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = ResponseConvert["success"].Value<bool>();
                if (isSuccess)
                {
                    // Nội dung email
                    var subject = "Yêu cầu đặt lại mật khẩu";
                    var newPassword = ResponseConvert["data"].Value<string>();
                    var body = new StringBuilder();
                    body.Append($"<p><strong>Mật khẩu mới của bạn là:</strong> {newPassword}</p>");
                    var emailService = new EmailService(_config);
                    await emailService.SendEmailAsync(YourEmail, subject, body.ToString());
                    var message = ResponseConvert["message"].Value<string>();
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Login_Customer", "Accounts");
                }
                TempData["ErrorMessage"] = ResponseConvert["message"].Value<string>();
                return RedirectToAction("Login_Customer", "Accounts");
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPassword_Staff()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword_Staff(string? YourEmail)
        {
            if (string.IsNullOrEmpty(YourEmail))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập Email của bạn";
                return RedirectToAction("ForgotPasswordStaff", "Accounts");
            }
            var response = await _client.GetAsync(_client.BaseAddress + $"Accounts/ForgotPasswordStaff?email={YourEmail}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ResponseConvert = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = ResponseConvert["success"].Value<bool>();
                if (isSuccess)
                {
                    // Nội dung email
                    var subject = "Yêu cầu đặt lại mật khẩu";
                    var newPassword = ResponseConvert["data"].Value<string>();
                    var body = new StringBuilder();
                    body.Append($"<p><strong>Mật khẩu mới của bạn là:</strong> {newPassword}</p>");
                    var emailService = new EmailService(_config);
                    await emailService.SendEmailAsync(YourEmail, subject, body.ToString());
                    var message = ResponseConvert["message"].Value<string>();
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Login_Staff", "Accounts");
                }
                TempData["ErrorMessage"] = ResponseConvert["message"].Value<string>();
                return RedirectToAction("Login_Staff", "Accounts");
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Google_Login()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "Accounts/LoginGoogle").Result;
            if (response.IsSuccessStatusCode)
            {
                return Redirect("https://localhost:7137/api/Accounts/LoginGoogle");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GoogleLoginSuccess(string accessToken, string refreshToken)
        {
            HttpContext.Session.SetString("AccessToken", accessToken);
            HttpContext.Session.SetString("RefreshToken", refreshToken);
            TempData["SuccessMessage"] = "Đăng nhập thành công";
            return RedirectToAction("Index", "Home");
        }
    }
}
