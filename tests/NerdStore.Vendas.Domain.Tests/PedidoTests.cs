using NerdStore.Core.DomainObjects;
using System.Linq;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar item novo pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

            // Act
            pedido.AdicionarItem(pedidoItem);

            // Assert
            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar item pedido existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1, 100);

            // Act
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId)?.Quantidade);
        }

        [Fact(DisplayName = "Adicionar item pedido existente e acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistenteAcimaDoPermitido_DeveIncrementarUnidadesSomarValores()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste 2", 14, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar item pedido inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExistenteNaLista_DeveRetornarException()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var pedidoItemAtualizado = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Atualizar item pedido valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(novaQuantidade, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Atualizar item pedido validar total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 15);
            var totalPedido = pedidoItemExistente1.CalcularValor() +
                              pedidoItemAtualizado.CalcularValor();

            // Act
            pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar item pedido quantidade acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemUnidadesAcimdaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(produtoId, "Produto Xpto", 2, 100);
            pedido.AdicionarItem(pedidoItemExistente1);

            // Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 15));
        }

        [Fact(DisplayName = "Remover item pedido inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemRemover));
        }

        [Fact(DisplayName = "Remover item pedido deve calcular valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExisteNaLista_DeveAtualizarValorTotal()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var produtoId = Guid.NewGuid();
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 20);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Xpto 2", 8, 50);
            
            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var totalPedido = pedidoItem2.CalcularValor();

            // Act
            pedido.RemoverItem(pedidoItem1);

            // Assert
            Assert.Equal(totalPedido, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var voucher = new Voucher("PROMO-TESTE", 15, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = pedido.AplicarVoucher(voucher);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var voucher = new Voucher("", null, 10, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), true, false);

            // Act
            var result = pedido.AplicarVoucher(voucher);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar voucher tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 20);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Xpto 2", 8, 50);

            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-10-PORCENTO", 10, null, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(15), true, false);

            var valorDesconto = (pedido.ValorTotal * voucher.Percentual) / 100;
            var valorTotalComDesconto = pedido.ValorTotal - valorDesconto;

            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(pedido.ValorTotal, valorTotalComDesconto);
        }

        [Fact(DisplayName = "Aplicar voucher tipo valor desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedido = new Pedido(Guid.NewGuid(), false, 10.50m, 100);
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 20);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Xpto 2", 8, 50);

            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-10-REAIS", null, 10, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            var valorTotalComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            // Act
            pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(pedido.ValorTotal, valorTotalComDesconto);
        }
    }
}