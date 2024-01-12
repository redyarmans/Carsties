using MongoDB.Entities;

namespace SearchService.Models;
public class Item : Entity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? AuctionEnd { get; set; }
    public string? Seller { get; set; }
    public string? Winner { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public string? Color { get; set; }
    public int Mileage { get; set; }
    public string? ImageUrl { get; set; } = "";
    public string? Status { get; set; }  
}