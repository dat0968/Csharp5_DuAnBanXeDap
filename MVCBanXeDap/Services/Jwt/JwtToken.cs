using APIBanXeDap.Models;
using Azure.Core;
using DocumentFormat.OpenXml.InkML;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace MVCBanXeDap.Services.Jwt
{
    public class JwtToken : IjwtToken
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        public readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JwtToken(IHttpContextAccessor _httpContextAccessor) {
            this._httpContextAccessor = _httpContextAccessor;
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public async Task<string?> ValidateAccessToken()
        {
            var accesstoken = _httpContextAccessor.HttpContext.Session.GetString("AccessToken")?.Trim('"');
            var refreshToken = _httpContextAccessor.HttpContext.Session.GetString("RefreshToken")?.Trim('"');
            if (accesstoken != null && refreshToken != null)
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(accesstoken))
                {
                    var jwtToken = handler.ReadJwtToken(accesstoken);
                    var expirationTime = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                    if (expirationTime != null && long.TryParse(expirationTime, out long exp))
                    {
                        var expireDate = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                        if (expireDate < DateTime.UtcNow)
                        {
                            var resreshtoken = JsonConvert.SerializeObject(new
                            {
                                RefreshToken = refreshToken
                            });
                            StringContent content = new StringContent(refreshToken, Encoding.UTF8, "application/json");
                            var response = await _client.PostAsync(_client.BaseAddress + "Accounts/RenewAccessToken", content);
                            if (response.IsSuccessStatusCode)
                            {
                                var data = await response.Content.ReadAsStringAsync();
                                var convertResponse = JsonConvert.DeserializeObject<JObject>(data);
                                var isSuccess = convertResponse["success"].Value<bool>();
                                if (isSuccess == true)
                                {
                                    var TokenResponse = convertResponse["data"];
                                    var NewAccessToken = TokenResponse["accessToken"]?.ToString();
                                    _httpContextAccessor.HttpContext.Session.SetString("AccessToken", NewAccessToken);
;                                    return NewAccessToken;
                                }
                                else
                                {
                                    _httpContextAccessor.HttpContext.Session.Remove("RefreshToken");
                                    return null;
                                }
                            }
                        }
                        else
                        {
                            return accesstoken;
                        }
                    }
                }
            }
            return null;
        }
        public PersonalInformation GetInformationUserFromToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(accessToken))
            {
                var jwtToken = handler.ReadJwtToken(accessToken);
                var claims = jwtToken.Claims;


                var id = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub); 
                var hoten = claims.FirstOrDefault(c => c.Type == "FullName");
                var sdt = claims.FirstOrDefault(c => c.Type == "PhoneNumber");
                var role = claims.FirstOrDefault(c => c.Type == "role");
                var data = new PersonalInformation
                {
                    Id = int.Parse(id.Value),
                    HoTen = hoten.Value,
                    SDT = sdt.Value,
                    VaiTro = role.Value,
                };
                return data;
            }
            return new PersonalInformation();
        }
        
    }
}
