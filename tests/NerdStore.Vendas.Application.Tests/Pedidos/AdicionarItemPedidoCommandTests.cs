using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar item command válido")]
        [Trait("Categoria", "Vendas - Pedido commands")]
        public void AdicionarItemPedido_CommandEstaValido_DevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto teste", 2, 100);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Adicionar item command inválido")]
        [Trait("Categoria", "Vendas - Pedido commands")]
        public void AdicionarItemPedido_CommandEstaInvalido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains("Id do cliente inválido", pedidoCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains("Id do produto inválido", pedidoCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains("O nome do produto não foi informado", pedidoCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains("A quantidade miníma de um item é 1", pedidoCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains("O valor do item precisa ser maior que 0", pedidoCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
        }

        [Fact(DisplayName = "Adicionar item command unidades superior ao permitido")]
        [Trait("Categoria", "Vendas - Pedido commands")]
        public void AdicionarItemPedido_CommandComUnidadesSuperioresAoPermitido_NaoDevePassarNaValidacao()
        {
            // Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", Pedido.MAX_UNIDADES_ITEM + 1, 0);

            // Act
            var result = pedidoCommand.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains("A quantidade máxima de um item é 15", pedidoCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
        }
    }
}
