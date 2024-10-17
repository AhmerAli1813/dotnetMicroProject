using System.ComponentModel.DataAnnotations;

namespace Shopping.API.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required")]

        public string? Name { get; set; }
        [Required(ErrorMessage = "Product Description is required")]
    
    
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
