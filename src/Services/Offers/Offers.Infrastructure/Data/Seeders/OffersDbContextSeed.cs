namespace Offers.Infrastructure.Data.Seeders;

public class OffersDbContextSeed
{
    public async Task SeedAsync(IServiceProvider serviceProvider, int? retry = 0)
    {
        int retryCounter = retry.Value;

        try
        {
            SeedOffersAsync();
        }
        catch (Exception ex)
        {
            if (retryCounter < 10)
            {
                retryCounter++;
                await SeedAsync(serviceProvider, retryCounter);
            }
        }
    }

    private async Task SeedOffersAsync()
    {
        
    }
}
