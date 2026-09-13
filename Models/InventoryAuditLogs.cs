namespace WarehouseAPI.Models;

public class InventoryAuditLogs
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int QuantityChange { get; set; }
    public int PreviousStock { get; set; }
    public int NewStock { get; set; }
    public string Reason { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
}