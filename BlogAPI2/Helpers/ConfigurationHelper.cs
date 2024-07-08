namespace BlogAPI2.Helpers
{
    public class ConfigurationHelper
    {
        private static IConfiguration _configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetApiUrl()
        {
            return _configuration["MySettings:ApiUrl"];
        }

        public static string GetImagesDirectory()
        {
            return _configuration["MySettings:ImagesDirectory"];
        }

        public static string GetJwtSecret()
        {
            return _configuration["JWT:Secret"];
        }

        public static string GetJwtAudience()
        {
            return _configuration["JWT:ValidAudience"];
        }

        public static string GetJwtIssuer()
        {
            return _configuration["JWT:ValidIssuer"];
        }
    }
}
