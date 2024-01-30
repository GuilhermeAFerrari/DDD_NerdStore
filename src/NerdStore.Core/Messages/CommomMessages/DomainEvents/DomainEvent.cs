using MediatR;

namespace NerdStore.Core.Messages.CommomMessages.DomainEvents
{
    public abstract class DomainEvent : Message, INotification
    {
        public DateTime TimeStamp { get; private set; }

        protected DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            TimeStamp = DateTime.Now;
        }
    }
}
