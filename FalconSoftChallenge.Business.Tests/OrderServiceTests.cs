using FalconSoftChallenge.Business.Interfaces;
using FalconSoftChallenge.Business.QueryObjects;
using FalconSoftChallenge.Business.Services;
using FalconSoftChallenge.DAL;
using FalconSoftChallenge.Entities;
using Moq;
using Moq.EntityFrameworkCore;

namespace FalconSoftChallenge.Business.Tests
{
    public class OrderServiceTests
    {
        private readonly IOrderService _orderServiceMock;
        private readonly Mock<FalconSoftDbContext> _falconDbContextMock;

        public OrderServiceTests()
        {
            _falconDbContextMock = new Mock<FalconSoftDbContext>();
            _orderServiceMock = new OrderService(_falconDbContextMock.Object);
        }

        [Fact]
        public async Task GetPaginated_WhenThereArentOrders_ShouldReturnsEmptyPagedResult()
        {
            // Arrange
            var data = new List<Order>().AsQueryable();

            _falconDbContextMock.Setup(x => x.Orders).ReturnsDbSet(data);
            var filters = new OrdersQueryModel(null, null, null,1,10);

            // Act
            var result = await _orderServiceMock.GetPaginated(filters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalItems);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetPaginated_WhenThereAreOrders_ShouldReturnsPagedResultCorrectly()
        {
            // Arrange
            var data = BuildFakeOrders();

            _falconDbContextMock.Setup(x => x.Orders).ReturnsDbSet(data);
            var filters = new OrdersQueryModel(null, null, null, 1, 1);

            // Act
            var result = await _orderServiceMock.GetPaginated(filters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalItems);
            Assert.Equal(3, result.TotalPagesCount);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetPaginated_WhenFilteringByField_ShouldReturnsPagedResultCorrectlyFiltered()
        {
            // Arrange
            var data = BuildFakeOrders();

            _falconDbContextMock.Setup(x => x.Orders).ReturnsDbSet(data);
            var filters = new OrdersQueryModel("Racing", null, null, 1, 10);

            // Act
            var result = await _orderServiceMock.GetPaginated(filters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalItems);
            Assert.Equal(1, result.TotalPagesCount);
            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Count == 1);
        }

        [Fact]
        public async Task GetPaginated_WhenSortingByFieldDesc_ShouldReturnsPagedResultCorrectlySorted()
        {
            // Arrange
            var data = BuildFakeOrders();

            _falconDbContextMock.Setup(x => x.Orders).ReturnsDbSet(data);
            var filters = new OrdersQueryModel(null, null, null, 1, 10, OrderSortingField.Description, OrderSortingWay.Desc);

            // Act
            var result = await _orderServiceMock.GetPaginated(filters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalItems);
            Assert.Equal(1, result.TotalPagesCount);
            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Count == 3);
            Assert.True(result.Data.First().Description == "Test");
        }

        [Fact]
        public async Task GetPaginated_WhenSortingByFieldAsc_ShouldReturnsPagedResultCorrectlySorted()
        {
            // Arrange
            var data = BuildFakeOrders();

            _falconDbContextMock.Setup(x => x.Orders).ReturnsDbSet(data);
            var filters = new OrdersQueryModel(null, null, null, 1, 10, OrderSortingField.Description, OrderSortingWay.Asc);

            // Act
            var result = await _orderServiceMock.GetPaginated(filters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalItems);
            Assert.Equal(1, result.TotalPagesCount);
            Assert.NotEmpty(result.Data);
            Assert.True(result.Data.Count == 3);
            Assert.True(result.Data.First().Description == "Hola");
        }

        private IQueryable<Order> BuildFakeOrders()
        {
            var user = new User();

            return new List<Order>
            {
               new Order(user, "Test"),
               new Order(user, "Hola"),
               new Order(user, "Racing")
            }
            .AsQueryable();
        }
    }
}