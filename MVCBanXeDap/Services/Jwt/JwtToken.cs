using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MVCBanXeDap.Services.Jwt
{
    public class JwtToken : IjwtToken
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        public readonly HttpClient _client;
        public JwtToken() {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public async Task<string?> ValidateAccessToken(string accessToken, string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(accessToken))
            {
                var jwtToken = handler.ReadJwtToken(accessToken);
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
                        StringContent content = new StringContent(resreshtoken, Encoding.UTF8, "application/json");
                        var response = await _client.PostAsync(_client.BaseAddress + "Accounts/RenewAccessToken", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var data = await response.Content.ReadAsStringAsync();
                            var convertResponse = JsonConvert.DeserializeObject<JObject>(data);
                            var isSuccess = convertResponse["success"].Value<bool>();
                            if(isSuccess == true)
                            {
                                var TokenResponse = convertResponse["data"];
                                var NewAccessToken = TokenResponse["accessToken"]?.ToString();
                                return NewAccessToken;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        return accessToken;
                    }
                }
            }
            return null;
        }
        public string GetUserIdFromToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(accessToken))
            {
                var jwtToken = handler.ReadJwtToken(accessToken);
                var claims = jwtToken.Claims;


                var id = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub); 

                return id?.Value; // Trả về idStaff hoặc null
            }
            return null;
        }
    }
}
