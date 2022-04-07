namespace Identity.Domain.AggregationModels.ApplicationUser.Child;

public interface ICountryRepository
{
    public Task<IEnumerable<CountryInfo>> GetAllAsync();
    public Task<CountryInfo?> GetAsync(int id);
    public Task<CountryInfo?> GetAsync(string iso);
}
