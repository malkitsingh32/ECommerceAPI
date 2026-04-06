using Application.Handler.Product.Dtos;
using Common.Common;

namespace Application.Abstraction.Services
{
    public interface IProductService
    {
        Task<CommonResultResponseDto<ProductDto>> GetProducts(int pageNumber, int pageSize, string whereClause, string sortQuery, string SearchText = null);
        Task<CommonResultResponseDto<int>> CreateUpdateProduct(CreateUpdateDto createUpdateDto);
        Task<CommonResultResponseDto<ProductDto>> GetProductById(int productId);
        Task<CommonResultResponseDto<string>> DeleteProduct(int productId);
    }
}
