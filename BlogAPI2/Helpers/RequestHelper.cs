namespace BlogAPI2.Helpers
{
    public class RequestHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static string GetIpAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Connection.RemoteIpAddress?.ToString();
        }
    }
}
