namespace GotorzProject.Service.System
{
    public static class DatabaseLoggerExtensions
    {
        public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder builder)
        {
            //builder.AddConfiguration();
            builder.Services.AddScoped<ILogger, DatabaseLogger>();

            return builder;
        }

        public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder builder, Action<DatabaseLoggerConfiguration> configure)
        {

        }
    }
}
