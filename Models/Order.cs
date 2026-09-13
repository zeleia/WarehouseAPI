namespace WarehouseAPI.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = String.Empty;
    public double Subtotal { get; set; }
    public double TaxAmount { get; set; }
    public double TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}