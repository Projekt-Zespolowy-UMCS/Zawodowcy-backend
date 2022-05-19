namespace Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

public interface ICountryRepository
{
    public Task<IEnumerable<CountryInfoAggregate>> GetAllAsync();
    public Task<CountryInfoAggregate?> GetAsync(int id);
    public Task<CountryInfoAggregate?> GetAsync(string iso);
}
