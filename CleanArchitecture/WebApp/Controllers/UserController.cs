using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Client.Infrastructure.ApiClientManagers;
using Client.Infrastructure.Configuration;
using Client.Infrastructure.Authentication;
using Client.Infrastructure.Exceptions;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly UserApiClientManager _userApiClientManager;
        private readonly IOptions<AuthConfig> _authConfig;
        private readonly IOptions<ApiClientConfig> _apiClientConfig;

        private AuthHandler AuthHandler() => new AuthHandler(_authConfig.Value, _tokenAcquisition);

        public UserController(IOptions<ApiClientConfig> apiClientConfig, IOptions<AuthConfig> authConfig, ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
            _apiClientConfig = apiClientConfig;
            _authConfig = authConfig;
            _userApiClientManager = new UserApiClientManager(_apiClientConfig.Value, authConfig.Value.Authentication ? AuthHandler() : null);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _userApiClientManager.GetAllAsync();
                if (result?.Data == null)
                    ViewBag("No User found");

                return View(result?.Data.Data);
            }
            catch (Exception ex)
            {
                throw new GeneralApplicationException("Failed to get user data from API.", ex);
            }
        }
    }
}
