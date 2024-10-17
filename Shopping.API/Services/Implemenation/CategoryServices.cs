
using Microsoft.EntityFrameworkCore;
using Shopping.API.Data;
using Shopping.API.DTO;
using Shopping.API.Entities;
using Shopping.API.Help;
using System.Reflection;

namespace Shopping.API.Services.Implemenation
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ShoppingDbContext _context;

        public CategoryServices(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> CreateUpdateCategoryAsync(CategoryDto req)
        {
            var itemreq = ModelConverter.ConvertTo<CategoryDto, Category>(req);
            if (itemreq.Id > 0)
            {
                _context.Categories.Update(itemreq);
            }
            else
            {
                _context.Categories.Add(itemreq);
            }
            await _context.SaveChangesAsync();
            return req;
        }


        public async Task<bool> DeleteCategory(int id)
        {
            var item = await _context.Categories.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (item != null)
            {
                _context.Categories.Remove(item);
                _context.SaveChanges();
                return true;
            }
            return false;

        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategory()
        {
            return await _context.Categories.Select(item => ModelConverter.ConvertTo<Category, CategoryDto>(item)).ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            return await _context.Categories.Where(x => x.Id == id).Select(item => ModelConverter.ConvertTo<Category, CategoryDto>(item)).FirstOrDefaultAsync();
        }
    }
}
