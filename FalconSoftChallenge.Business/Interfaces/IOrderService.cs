using FalconSoftChallenge.Business.DTO;
using FalconSoftChallenge.Business.QueryObjects;
using FalconSoftChallenge.DAL.DTO;

namespace FalconSoftChallenge.Business.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResultDTO<OrderDTO>> GetPaginated(OrdersQueryModel filters);

        Task<OrderDTO> Update(UpdateOrderDTO updateOrderDTO);
    }
}
