using MediatR;
using NerdStore.Core.Data.EventSourcing;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommomMessages.DomainEvents;
using NerdStore.Core.Messages.CommomMessages.Notifications;

namespace NerdStore.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatorHandler(IMediator mediator, IEventSourcingRepository eventSourcing)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcing;
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
            await _eventSourcingRepository.SalvarEvento(evento);
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediator.Publish(notificacao);
        }

        public async Task PublicarDomainEvent<T>(T evento) where T : DomainEvent
        {
            await _mediator.Publish(evento);
        }
    }
}
