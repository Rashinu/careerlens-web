namespace CareerLens.Shared.Constants;

public static class AppConstants
{
    public static class Cv
    {
        public const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
        public static readonly string[] AllowedMimeTypes = ["application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"];
        public static readonly string[] AllowedExtensions = [".pdf", ".docx"];
        public const string BlobContainerName = "cv-files";
    }

    public static class Jwt
    {
        public const int AccessTokenExpiryMinutes = 60;
        public const int RefreshTokenExpiryDays = 30;
    }

    public static class RateLimit
    {
        public const int PublicEndpointsPerMinute = 20;
        public const int AuthEndpointsPerMinute = 10;
        public const int AiEndpointsPerMinute = 5;
        public const int GeneralEndpointsPerMinute = 60;
    }

    public static class Cache
    {
        public const int SalaryBenchmarkCacheMinutes = 60;
        public const string SalaryBenchmarkKeyPrefix = "salary:benchmark:";
    }
}
