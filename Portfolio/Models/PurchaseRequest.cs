namespace Portfolio.Models;

public class PurchaseRequest
{
    public Guid Id { get; set; }
    public Guid ArtworkId { get; set; }

    public string ArtworkTitle { get; set; } = "";
    public decimal ArtworkPrice { get; set; }

    public string BuyerName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }

    public string Country { get; set; } = "";
    public string City { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string Address { get; set; } = "";

    public string DeliveryPreference { get; set; } = "Courier";
    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}