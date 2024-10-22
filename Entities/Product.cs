using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationJavitoDolgozat.Entities;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public DateTime ManufacturedDate { get; set; }

    public int CategoryId { get; set; }

    public string UserId { get; set; }
    public Category Category { get; set; }

}