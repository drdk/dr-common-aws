using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace DR.Common.AWS.S3.Test
{
    internal static class Startup
    {
        internal static readonly S3ServiceOptions S3ServiceOptions;
        static Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            S3ServiceOptions = new S3ServiceOptions();

            configuration.GetSection("S3Client").Bind(S3ServiceOptions);
        }
    }
}
