using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Services;
using MVCBanXeDap.Services.Email;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MVCBanXeDap.Controllers
{
    public class AccountsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public AccountsController(IConfiguration config)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _config = config;
        }
        [HttpGet]
        public IActionResult Login_Customer()
        {
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
                                var phoneNumber = responseData["phoneNumber"].ToString();
                                var fullName = responseData["fullName"].ToString();
                                HttpContext.Session.SetString("PhoneNumber", phoneNumber);
                                HttpContext.Session.SetString("FullName", fullName);
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
                        ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại hoặc đang bị tạm khóa!");
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
        public IActionResult Login_Staff()
        {
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
                        ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại hoặc đang bị tạm khóa!");
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
                        return RedirectToAction("LoginCustomer", "Accounts");
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
                    return RedirectToAction("LoginCustomer", "Accounts");
                }
                TempData["ErrorMessage"] = ResponseConvert["message"].Value<string>();
                return RedirectToAction("LoginCustomer", "Accounts");
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
                    return RedirectToAction("LoginStaff", "Accounts");
                }
                TempData["ErrorMessage"] = ResponseConvert["message"].Value<string>();
                return RedirectToAction("LoginStaff", "Accounts");
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
