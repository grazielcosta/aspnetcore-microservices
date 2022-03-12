using System;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvents
    {
        public IntegrationBaseEvents() : this(new Guid(), DateTime.Now)
        {

        }
        public IntegrationBaseEvents(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}
