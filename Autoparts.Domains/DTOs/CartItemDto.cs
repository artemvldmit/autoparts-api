namespace Autoparts.Domains.DTOs;

public class CartItemDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
}

public class CreateCartItemDto
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateCartItemDto
{
    public int Quantity { get; set; }
}
