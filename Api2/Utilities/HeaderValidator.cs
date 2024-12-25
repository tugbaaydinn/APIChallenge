namespace Api2.Utilities
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Http;

    public class HeaderValidator
    {
        private readonly IConfiguration _configuration;

        public HeaderValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool ValidateHeaders(IHeaderDictionary headers, out string errorMessage)
        {
            var username = _configuration["Credentials:Username"];
            var password = _configuration["Credentials:Password"];

            if (!headers.ContainsKey("username") || !headers.ContainsKey("password"))
            {
                errorMessage = "Missing username or password in headers.";
                return false;
            }

            if (headers["username"] != username || headers["password"] != password)
            {
                errorMessage = "Invalid username or password.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
