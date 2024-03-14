using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar voucher tipo valor valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher("PROMO-TESTE", null, 15, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar voucher tipo valor inválido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(4, result.Errors.Count);
            Assert.Contains("Este voucher está expirado.", result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("Este voucher não é mais válido.", result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("Este voucher já foi utilizado.", result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("Este voucher não está mais disponível.", result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validar voucher tipo procentagem valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPorcentagem_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher("PROMO-TESTE", 15, null, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar voucher tipo porcentagem inválido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPorcentagem_DeveEstarInvalido()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(4, result.Errors.Count);
            Assert.Contains("Este voucher está expirado.", result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("Este voucher não é mais válido.", result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("Este voucher já foi utilizado.", result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains("Este voucher não está mais disponível.", result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
