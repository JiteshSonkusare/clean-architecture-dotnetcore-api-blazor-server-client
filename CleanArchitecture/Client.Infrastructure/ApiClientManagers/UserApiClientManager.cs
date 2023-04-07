using System.Net;
using Client.Infrastructure.Routes;
using Client.Infrastructure.Client;
using Client.Infrastructure.Extensions;
using Client.Infrastructure.ViewModels;
using Client.Infrastructure.Exceptions;
using Client.Infrastructure.Configuration;
using Client.Infrastructure.Security.Interfaces;

namespace Client.Infrastructure.ApiClientManagers
{
    public class UserApiClientManager : ApiClientBase
    {
        public UserApiClientManager(IClientConfig config, IAuthHandler authHandler) : base(config, authHandler)
        {
            if (string.IsNullOrWhiteSpace(config?.BaseUrl))
            {
                throw new ArgumentNullException(nameof(config.BaseUrl));
            }
        }

        public async Task<Response<UserViewModel>?> GetAllAsync()
        {
            ResponseData result = await Send(
                    new Uri(UserEndpoints.GetAll, UriKind.Relative),
                    HttpMethod.Get,
                    null,
                     CancellationToken.None,
                    Array.Empty<HeaderData>()
                ).ConfigureAwait(false);

            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return new Response<UserViewModel>(new ResponseData(HttpStatusCode.NotFound, Array.Empty<UserViewModel>().ToJson(), result.ResponseHeaders), HttpStatusCode.NotFound);
            if (result.StatusCode == (int)HttpStatusCode.OK)
                return new Response<UserViewModel>(result, result.StatusCode);

            throw new GeneralApplicationException(result.Content);

        }

        protected override DefaultRequestHeaders GetDefaultRequestHeaders()
        {
            return new DefaultRequestHeaders(new string[] { System.Net.Mime.MediaTypeNames.Application.Json }, Array.Empty<HeaderData>());
        }
    }
}
