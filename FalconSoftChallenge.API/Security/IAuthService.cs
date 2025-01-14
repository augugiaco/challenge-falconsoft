namespace FalconSoftChallenge.API.Security
{
    public interface IAuthService
    {
        Task<string> LoginAndGenerateAccessToken(UserLoginModel userLoginModel);
    }
}
