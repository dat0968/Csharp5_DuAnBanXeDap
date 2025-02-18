using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.Token
{
    public interface ITokenServices
    {
        public string GenerateAccessToken(PersonalInformation model);
        public string GenerateRefreshToken();

    }
}
