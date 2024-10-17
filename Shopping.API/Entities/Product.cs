using Shopping.API.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shopping.API.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage ="Product Name is required")]
        
        public string? Name { get; set; }
        [Required(ErrorMessage = "Product Description is required")]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Product Price is required")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Please  Category  is required")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime?  CreatedAt { get; set; }
        public DateTime?  UpdateAt { get; set; }
    }
}
