using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplicationJavitoDolgozat.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string UserId { get; set; }

        public ICollection<Product> Products { get; set; }

        public ApplicationUser User { get; set; }  // Kapcsolat a User entitással
    }
}
