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
    }
}
