namespace FalconSoftChallenge.Business.DTO
{
    public class UpdateOrderDTO
    {
        public Guid OrderId { get; set; }
        public List<ProductPerOrderDTO> Products { get; set; }

        public UpdateOrderDTO()
        {
            Products = new List<ProductPerOrderDTO>();
        }
    }
}
