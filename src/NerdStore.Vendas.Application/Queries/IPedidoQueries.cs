using NerdStore.Vendas.Application.Queries.ViewModels;

namespace NerdStore.Vendas.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<CarrinhoViewModel> ObterCarrinhoCliente(Guid clientId);
        Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clientId);
    }
}
