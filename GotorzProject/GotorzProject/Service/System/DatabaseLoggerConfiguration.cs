using GotorzProject.Model.ObjectRelationMapping;

namespace GotorzProject.Service.System
{
    public sealed class DatabaseLoggerConfiguration
    {
        public int EventId { get; set; } // not sure why this is needed?

        public ApplicationDbContext Context { get; set; }

        //todo : figure out why this is needed, and perhaps customize it
    }
}
