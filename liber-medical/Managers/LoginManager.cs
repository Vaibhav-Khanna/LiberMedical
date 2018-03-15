using System.Threading.Tasks;
using libermedical.Request;
using libermedical.Responses;

namespace libermedical.Managers
{
    class LoginManager : ILoginManager
    {
        private readonly IRestService<LoginRequest> _restService;

        public LoginManager(IRestService<LoginRequest> service)
        {
            _restService = service;
        }

        public Task<TokenResponse> Login(LoginRequest login)
        {
            return _restService.GetLoginToken(login);
        }

        public async Task RequestNewPassword(ForgotPasswordRequest request)
        {
            await _restService.RequestNewPassword(request);
        }
    }
}
