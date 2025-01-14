using FalconSoftChallenge.API.Controllers;
using FalconSoftChallenge.Business.DTO;
using FalconSoftChallenge.Business.Interfaces;
using FalconSoftChallenge.Business.QueryObjects;
using FalconSoftChallenge.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FalconSoftChallenge.API.Tests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrdersController(_mockOrderService.Object);
        }

        [Fact]
        public async Task Get_WhenThereArentOrders_ShouldReturnOKWithPagedResultWithoutData()
        {
            // Arrange
            var pagedResult = new PagedResultDTO<OrderDTO>(new List<OrderDTO>(), 0, 1, 10, 1);

            _mockOrderService
                .Setup(x => x.GetPaginated(It.IsAny<OrdersQueryModel>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Get(null, null, null, 1, 10, OrderSortingField.CreatedDate, OrderSortingWay.Desc);
            var contentResult = (result as OkObjectResult)!.Value as PagedResultDTO<OrderDTO>;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Empty(contentResult!.Data);
            Assert.Equal(0, pagedResult.TotalItems);
            _mockOrderService.Verify(x => x.GetPaginated(It.IsAny<OrdersQueryModel>()), Times.Once);
        }

        [Fact]
        public async Task Get_WhenThereAreOrders_ShouldReturnOkWithPagedResultWithData()
        {
            // Arrange
            var orderDTO = new OrderDTO(
                Guid.NewGuid(),
                "Test",
                DateTime.UtcNow,
                10m,
                Entities.OrderStatus.Created,
                new List<ProductDTO>
                {
                    new ProductDTO(Guid.NewGuid(), "Test", 10m, 1)
                });
;

            var pagedResult = new PagedResultDTO<OrderDTO>(new List<OrderDTO>() { orderDTO }, 1, 1, 10, 1);

            _mockOrderService
                .Setup(x => x.GetPaginated(It.IsAny<OrdersQueryModel>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Get(null, null, null, 1, 10, OrderSortingField.CreatedDate, OrderSortingWay.Desc);
            var contentResult = (result as OkObjectResult)!.Value as PagedResultDTO<OrderDTO>;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotEmpty(contentResult!.Data);
            Assert.Equal(1, pagedResult.TotalItems);
            _mockOrderService.Verify(x => x.GetPaginated(It.IsAny<OrdersQueryModel>()), Times.Once);
        }
    }
}