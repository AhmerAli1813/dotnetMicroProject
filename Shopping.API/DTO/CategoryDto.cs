using Shopping.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
