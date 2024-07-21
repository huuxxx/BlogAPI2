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
            return _configuration["MySettings:ApiUrl"] ?? throw new NullReferenceException();
        }

        public string GetImagesDirectory()
        {
            return _configuration["MySettings:ImagesDirectory"] ?? throw new NullReferenceException();
        }

        public string GetJwtSecret()
        {
            return _configuration["JWT:Secret"] ?? throw new NullReferenceException();
        }

        public string GetJwtAudience()
        {
            return _configuration["JWT:ValidAudience"] ?? throw new NullReferenceException();
        }

        public string GetJwtIssuer()
        {
            return _configuration["JWT:ValidIssuer"] ?? throw new NullReferenceException();
        }
    }
}
