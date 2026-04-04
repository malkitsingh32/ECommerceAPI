using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Implementation.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static readonly List<Products> _products = new()
        {
            new Products { ProductId = 1, ProductName = "Sample Product 1", SKU = "SKU-001", Description = "Dummy product 1", CategoryId = 1, SellingPrice = 99.99m, StockQty = 10, IsActive = true, IsDeleted = false, CreatedDate = DateTime.UtcNow },
            new Products { ProductId = 2, ProductName = "Sample Product 2", SKU = "SKU-002", Description = "Dummy product 2", CategoryId = 2, SellingPrice = 149.50m, StockQty = 20, IsActive = true, IsDeleted = false, CreatedDate = DateTime.UtcNow },
            new Products { ProductId = 3, ProductName = "Sample Product 3", SKU = "SKU-003", Description = "Dummy product 3", CategoryId = 1, SellingPrice = 249.00m, StockQty = 5, IsActive = true, IsDeleted = false, CreatedDate = DateTime.UtcNow }
        };

        private static long _nextProductId = 4;

        public Task<int> CreateUpdateProduct(Products products)
        {
            if (!products.ProductId.HasValue || products.ProductId.Value <= 0)
            {
                products.ProductId = _nextProductId++;
                products.CreatedDate = DateTime.UtcNow;
                products.IsActive = true;
                products.IsDeleted = false;
                _products.Add(products);
                return Task.FromResult((int)products.ProductId.Value);
            }

            var existing = _products.FirstOrDefault(p => p.ProductId == products.ProductId);
            if (existing == null)
            {
                return Task.FromResult(0);
            }

            existing.ProductName = products.ProductName;
            existing.Description = products.Description;
            existing.SKU = products.SKU;
            existing.CategoryId = products.CategoryId;
            existing.SellingPrice = products.SellingPrice;
            existing.StockQty = products.StockQty;
            existing.UpdatedBy = products.UpdatedBy;
            existing.UpdatedDate = DateTime.UtcNow;

            return Task.FromResult((int)existing.ProductId!.Value);
        }

        public Task<Products> GetProductById(int productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId && p.IsDeleted != true);
            return Task.FromResult(product);
        }

        public Task<Tuple<int, List<Products>>> GetProducts(int pageNumber, int pageSize, string whereClause, string sortQuery, string searchText = null)
        {
            var query = _products.Where(p => p.IsDeleted != true);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p =>
                    (p.ProductName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.SKU?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Description?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            var total = query.Count();

            var page = pageNumber <= 0 ? 1 : pageNumber;
            var size = pageSize <= 0 ? 10 : pageSize;

            var items = query
                .OrderByDescending(p => p.ProductId)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return Task.FromResult(Tuple.Create(total, items));
        }
    }
}
