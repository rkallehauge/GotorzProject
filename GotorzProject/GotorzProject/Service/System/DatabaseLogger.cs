using GotorzProject.Model;
using GotorzProject.Model.ObjectRelationMapping;

namespace GotorzProject.Service.System
{
    
    public class DatabaseLogger(string name, Func<DatabaseLoggerConfiguration> getConfig) : ILogger 
    {
        private readonly string _categoryName = default!;
        private ApplicationDbContext _dbContext;

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None; // for now log all non-none events

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);

            var log = new LoggedEvent
            {
                Timestamp = DateTime.UtcNow,
                LogLevel = logLevel,
                Category = _categoryName,
                Message = message,
                Exception = exception?.ToString()
            };
            _dbContext = getConfig().Context; // ????

            _dbContext.LoggedEvents.Add(log);
            _dbContext.SaveChanges(); 
        }
    }
}
