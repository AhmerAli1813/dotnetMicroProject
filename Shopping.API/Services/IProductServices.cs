using Shopping.API.DTO;

namespace Shopping.API.Services
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDto>> GetAllProduct();
        Task<ProductDto> GetProductById(int id);
        Task<ProductDto> CreateUpdateProductAsync(ProductDto proudct);
        Task<bool> DeleteProduct(int id);
    }
}
