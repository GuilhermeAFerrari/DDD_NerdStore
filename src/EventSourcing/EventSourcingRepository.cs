using NerdStore.Core.Data.EventSourcing;
using NerdStore.Core.Messages;

namespace EventSourcing
{
    public class EventSourcingRepository : IEventSourcingRepository
    {
        private readonly IEventStoreService _eventStoreService;

        public EventSourcingRepository(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task<IEnumerable<StoredEvent>> ObterEventos(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public async Task SalvarEvento<TEvent>(TEvent evento) where TEvent : Event
        {
            throw new NotImplementedException();
        }
    }
}
