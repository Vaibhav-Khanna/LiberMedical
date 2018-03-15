using System.Threading.Tasks;
using libermedical.Request;
using libermedical.Responses;

namespace libermedical.Managers
{
    public interface ILoginManager
    {
        Task<TokenResponse> Login(LoginRequest login);

        Task RequestNewPassword(ForgotPasswordRequest request);
    }
}
