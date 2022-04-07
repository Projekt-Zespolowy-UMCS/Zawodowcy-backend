namespace Identity.Domain.AggregationModels.ApplicationUser.Child;

public interface ICountryRepository
{
    public Task<IEnumerable<CountryInfo>> GetAll();
    public Task<CountryInfo?> Get(int id);
    public Task<CountryInfo?> Get(string iso);
}
