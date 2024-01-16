using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;

namespace NerdStore.Core.Bus
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        //public Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        //{
        //    throw new NotImplementedException();
        //}

        //public Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent
        //{
        //    throw new NotImplementedException();
        //}
    }
}
