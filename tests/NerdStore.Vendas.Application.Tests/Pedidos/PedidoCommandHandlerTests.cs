using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommomMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly PedidoCommandHandler _pedidoCommandHandler;
        private readonly Guid _clientId;
        private readonly Guid _produtoId;
        private readonly Pedido _pedido;

        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _pedidoCommandHandler = _mocker.CreateInstance<PedidoCommandHandler>();
            _clientId = Guid.NewGuid();
            _produtoId = Guid.NewGuid();
            _pedido = new Pedido(_clientId, false, 10, 100);
        }

        [Fact(DisplayName = "Adicionar item novo pedido com sucesso")]
        [Trait("Categoria", "Vendas - Pedido command handler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once());
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);

            // Nesse caso de testes, não faz sentido validar o lançamento do evento, pois quem validará isso é um teste do UnitOfWork
            //_mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once());
        }

        [Fact(DisplayName = "Adicionar novo item pedido rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Pedido command handler")]
        public async Task AdicionarItem_NovoItemAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(Guid.NewGuid(), "Produto teste", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clientId, Guid.NewGuid(), "Produto teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clientId))
                .Returns(Task.FromResult(_pedido));

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once());
            //mocker.GetMock<IPedidoRepository>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once());
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Adicionar item existente ao pedido rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Pedido command handler")]
        public async Task AdicionarItem_ItemExistenteAoPedidoRascunho_DeveExecutarComSucesso()
        {
            // Arrange
            var pedidoItemExistente = new PedidoItem(_produtoId, "Produto xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(_clientId, _produtoId, "Produto teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.ObterPedidoRascunhoPorClienteId(_clientId))
                .Returns(Task.FromResult(_pedido));

            _mocker.GetMock<IPedidoRepository>()
                .Setup(r => r.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once());
            _mocker.GetMock<IPedidoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }


        [Fact(DisplayName = "Adicionar item command inválido")]
        [Trait("Categoria", "Vendas - Pedido command handler")]
        public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = await _pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediatorHandler>().Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.Exactly(5));
        }
    }
}
