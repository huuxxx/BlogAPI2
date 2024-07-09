namespace BlogAPI2.Helpers
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetApiUrl()
        {
            return _configuration["MySettings:ApiUrl"];
        }

        public string GetImagesDirectory()
        {
            return _configuration["MySettings:ImagesDirectory"];
        }

        public string GetJwtSecret()
        {
            return _configuration["JWT:Secret"];
        }

        public string GetJwtAudience()
        {
            return _configuration["JWT:ValidAudience"];
        }

        public string GetJwtIssuer()
        {
            return _configuration["JWT:ValidIssuer"];
        }
    }
}
