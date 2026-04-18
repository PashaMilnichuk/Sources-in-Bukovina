namespace CarpathianCrown.Web.Models;

public class RevenueHourlyResponse
{
    public List<RevenueHourlyDto> Items { get; set; } = new();
    public double MaxRevenue { get; set; }
}