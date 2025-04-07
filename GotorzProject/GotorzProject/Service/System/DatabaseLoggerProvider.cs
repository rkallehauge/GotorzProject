using GotorzProject.Model.ObjectRelationMapping;

namespace GotorzProject.Service.System
{
    [ProviderAlias("GotorzLogger")]
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseLoggerProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(categoryName, _dbContext);
        }

        public void Dispose()
        {
            // DI handles this, don't bother :D
        }
    }
}
