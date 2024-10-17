using Shopping.API.DTO;

namespace Shopping.API.Services
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryDto>> GetAllCategory();
        Task<CategoryDto> GetCategoryById(int id);
        Task<CategoryDto> CreateUpdateCategoryAsync(CategoryDto model);
        Task<bool> DeleteCategory(int id);
    }
}
