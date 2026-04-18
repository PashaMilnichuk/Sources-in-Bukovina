using System.Net.Http.Json;

namespace CarpathianCrown.Api.Services;

public class AnalyticsClient(HttpClient http)
{
    public async Task<Dictionary<string, object>> Summary()
    {
        var res = await http.GetFromJsonAsync<Dictionary<string, object>>("/api/analytics/summary");
        return res ?? new Dictionary<string, object>();
    }

    public async Task<Dictionary<string, object>> ManagementDashboard()
    {
        var res = await http.GetFromJsonAsync<Dictionary<string, object>>("/api/analytics/dashboard");
        return res ?? new Dictionary<string, object>();
    }
}