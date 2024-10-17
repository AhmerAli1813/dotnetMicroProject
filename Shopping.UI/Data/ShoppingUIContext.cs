using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shopping.UI.Models;

namespace Shopping.UI.Data
{
    public class ShoppingUIContext : DbContext
    {
        public ShoppingUIContext (DbContextOptions<ShoppingUIContext> options)
            : base(options)
        {
        }

        public DbSet<Shopping.UI.Models.ProductDto> ProductDto { get; set; } = default!;
    }
}
