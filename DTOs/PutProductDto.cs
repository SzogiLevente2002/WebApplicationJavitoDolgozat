public class PutProductDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public DateTime ManufacturedDate { get; set; }

    public int CategoryId { get; set; }
}
