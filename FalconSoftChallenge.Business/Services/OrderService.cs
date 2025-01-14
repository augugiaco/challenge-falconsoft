using FalconSoftChallenge.Business.DTO;
using FalconSoftChallenge.Business.Interfaces;
using FalconSoftChallenge.Business.QueryObjects;
using FalconSoftChallenge.DAL;
using FalconSoftChallenge.DAL.DTO;
using FalconSoftChallenge.DAL.Extensions;
using FalconSoftChallenge.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FalconSoftChallenge.Business.Services
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(FalconSoftDbContext context) : base(context){}

        public async Task<PagedResultDTO<OrderDTO>> GetPaginated(OrdersQueryModel filters)
        {
            var query = _context
                .Orders
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .AsNoTracking();

            query = Filter(query, filters);

            query = Sort(query, filters.SortingField, filters.SortingWay);

            var result = await query
                .Paginate(filters.Page, 
                          filters.PageSize, 
                          x => x.ToDTO());

            return result;
        }

        public async Task<OrderDTO> Update(UpdateOrderDTO updateOrderDTO)
        {
            if(updateOrderDTO == null) throw new ArgumentNullException(nameof(updateOrderDTO));

            var order = await _context
                .Orders
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync(x => x.Id == updateOrderDTO.OrderId);

            if (order == null) throw new Exception("Order not found");

            UpdateProductsInOrder(order, updateOrderDTO);

            RemoveProductsInOrder(order, updateOrderDTO);

            await AddProductsToOrder(order, updateOrderDTO);

            await _context.SaveChangesAsync();

            return order.ToDTO();
        }

        private void UpdateProductsInOrder(Order order, UpdateOrderDTO updateOrderDTO) 
        {
            var updatedProducts = updateOrderDTO
                .Products
                .Where(x => x.Quantity > 0)
                .ToList();

            if (updatedProducts.Any() == false) return;

            foreach (var product in updatedProducts)
            {
                var productInOrder = order
                    .Products
                    .FirstOrDefault(x => x.ProductId == product.Id);

                if (productInOrder == null) continue;

                productInOrder.SetQuantity(product.Quantity);
            }

            order.RefreshAmount();
        }

        private void RemoveProductsInOrder(Order order, UpdateOrderDTO updateOrderDTO) 
        {
            var actualProductsInOrderIds = order
                .Products
                .Select(x => x.ProductId)
                .ToList();

            var productsToRemoveIds = updateOrderDTO
                 .Products
                 .Where(x => x.Quantity < 1 && actualProductsInOrderIds.Contains(x.Id))
                 .Select(x => x.Id)
                 .ToList();

            if (productsToRemoveIds.Any())
                order.RemoveProducts(productsToRemoveIds);
        }

        private async Task AddProductsToOrder(Order order, UpdateOrderDTO updateOrderDTO) 
        {
            var actualProductsInOrderIds = order
                .Products
                .Select(x => x.ProductId)
                .ToList();

            var productsToAddIds = updateOrderDTO
                .Products
                .Where(p => p.Quantity > 0 && actualProductsInOrderIds.Contains(p.Id) == false)
                .Select(x => x.Id)
                .ToList();

            if (productsToAddIds.Any() == false) return; 

            var products = await _context
                .Products
                .Where(x => productsToAddIds.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync();

            if (products.Any() == false) return;

            var newProductsPerOrder = new List<ProductsPerOrder>();

            foreach (var product in products)
            {
                var quantity = updateOrderDTO
                    .Products
                    .FirstOrDefault(x => x.Id == product.Id)?.Quantity;

                if (quantity == null) continue;

                var productPerOrder = new ProductsPerOrder(order, product,quantity.Value);

                newProductsPerOrder.Add(productPerOrder);
            }

            if (newProductsPerOrder.Any()) 
                order.AddProducts(newProductsPerOrder);
            
        }

        private IQueryable<Order> Filter(IQueryable<Order> query, OrdersQueryModel filters)
        {
            query = query.Where(x => (filters.CreatedDate == null) ||
                (filters.CreatedDate >= x.CreatedDate && filters.CreatedDate <= x.CreatedDate));

            query = query.Where(x => (string.IsNullOrEmpty(filters.Description)) ||
                x.Description.ToLower().Contains(filters.Description.ToLower()));

            query = query.Where(x => filters.Amount == null ||
                x.Amount == filters.Amount);

            return query;
        }

        private IOrderedQueryable<Order> Sort(IQueryable<Order> query,OrderSortingField sortingField, OrderSortingWay sortingWay) 
        {
            Expression<Func<Order, object>> sortBy = sortingField switch
            {
                OrderSortingField.CreatedDate => x => x.CreatedDate,
                //OrderSortingField.Amount => x => x.Amount, -- COMENTADO DEBIDO A UNA LIMITACION DE SQLite, no se puede usar valores decimales en el order by.
                OrderSortingField.Description => x => x.Description,
                _ => throw new ArgumentException(),
            };

            if (sortingWay == OrderSortingWay.Asc)
                return query.OrderBy(sortBy);
            else
                return query.OrderByDescending(sortBy);
        }
    }
}
