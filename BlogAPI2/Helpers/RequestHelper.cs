using Microsoft.Extensions.Configuration;

namespace BlogAPI2.Helpers
{
    public class RequestHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)); ;
        }
        public string GetIpAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Connection.RemoteIpAddress?.ToString();
        }
    }
}
