using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Handler.Product.Dtos;
using Common.Common;
using Domain.Entities;
using Helper;
using Mapster;

namespace Infrastructure.Implementation.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<CommonResultResponseDto<int>> CreateUpdateProduct(CreateUpdateDto createUpdateDto)
        {
            var productId = await _productRepository.CreateUpdateProduct(createUpdateDto.Adapt<Products>());
            if (productId > 0)
            {
                var message = createUpdateDto.ProductId.HasValue ? "Product updated successfully." : "Product created successfully.";
                return CommonResultResponseDto<int>.Success(new string[] { message }, productId);
            }
            else
            {
                return CommonResultResponseDto<int>.Failure(new string[] { "Failed to create/update product." }, 0);
            }
        }

        public async Task<CommonResultResponseDto<string>> DeleteProduct(int productId)
        {
            var id = await _productRepository.DeleteProduct(productId);
            if (id > 0)
            {
                return CommonResultResponseDto<string>.Success(new string[] { ActionStatusHelper.Deleted }, "",id);
            }
            else
            {
                return CommonResultResponseDto<string>.Failure(new string[] { "Failed to delete product." }, "");
            }
        }

        public async Task<CommonResultResponseDto<ProductDto>> GetProductById(int productId)
        {
            var product =  await _productRepository.GetProductById(productId);
            if (product != null) 
            {
               return CommonResultResponseDto<ProductDto>.Success(new string[] { "Product retrieved successfully." }, product.Adapt<ProductDto>(), 0);
            }
            else 
            {
              return CommonResultResponseDto<ProductDto>.Failure(new string[] { "Product not found." }, null, false);
            }
        }

        public async Task<CommonResultResponseDto<ProductDto>> GetProducts(int pageNumber, int pageSize, string whereClause, string sortQuery, string SearchText = null)
        {
            var (total,products) = await _productRepository.GetProducts(pageNumber, pageSize, whereClause, sortQuery, SearchText);
            ProductDto productDto = new ProductDto
            {
                TotalRecords = total,
                ProductList = products.Adapt<List<ProductListDto>>()
            };
            return CommonResultResponseDto<ProductDto>.Success(new string[] { "Products retrieved successfully." }, productDto, 0);
        }
    }
}
