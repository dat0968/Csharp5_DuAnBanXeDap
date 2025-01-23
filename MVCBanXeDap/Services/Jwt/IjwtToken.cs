namespace MVCBanXeDap.Services.Jwt
{
    public interface IjwtToken
    {
        Task<string?> ValidateAccessToken(string accessToken, string refreshToken);
    }
}
