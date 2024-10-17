
using Microsoft.EntityFrameworkCore;
using Shopping.API.Data;
using Shopping.API.DTO;
using Shopping.API.Entities;
using Shopping.API.Help;
using System.Reflection;

namespace Shopping.API.Services.Implemenation
{
    public class ProductServices : IProductServices
    {
        private readonly ShoppingDbContext _context;

        public ProductServices(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> CreateUpdateProductAsync(ProductDto req)
        {
            var itemreq = ModelConverter.ConvertTo<ProductDto, Product>(req); 
            if (itemreq.Id > 0)
            {
                itemreq.UpdateAt = DateTime.Now;
                
                _context.Products.Update(itemreq);
            }
            else
            {
                itemreq.CreatedAt = DateTime.Now;
                _context.Products.Add(itemreq);
            }
         await   _context.SaveChangesAsync();
            return req;
        }


        public async Task<bool> DeleteProduct(int id)
        {
            var item = await _context.Products.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (item != null) { 
            _context.Products.Remove(item);
                _context.SaveChanges();
                return true;
            }
            return false;
                
        }

        public async Task<IEnumerable<ProductDto>> GetAllProduct()
        {
          return await _context.Products.Select(item=> ModelConverter.ConvertTo<Product,ProductDto>(item)).ToListAsync();
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            return await _context.Products.Where(x => x.Id == id).Select(item => ModelConverter.ConvertTo<Product, ProductDto>(item)).FirstOrDefaultAsync();
        }
    }
}
