using MVCBanXeDap.ViewModels;

namespace MVCBanXeDap.Services.Jwt
{
    public interface IjwtToken
    {
        Task<string> ValidateAccessToken();
        PersonalInformation GetInformationUserFromToken(string accessToken);
    }
}
