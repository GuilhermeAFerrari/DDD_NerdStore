using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Novo item pedido abaixo do permitido")]
        [Trait("Categoria", "Item pedido Tests")]
        public void NovoItemPedido_UnidadesItemAbaixoDoPermitido_DeveRetornarException()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MIN_UNIDADES_ITEM - 1, 100));
        }


        [Fact(DisplayName = "Novo item pedido acima do permitido")]
        [Trait("Categoria", "Item pedido Tests")]
        public void NovoItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100));
        }
    }
}
