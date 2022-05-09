namespace Identity.Domain.AggregationModels.ApplicationUser.Address;

public interface IAddressRepository
{
    public Task<AddressAggregate?> GetUserAddressAsync(int id);
    public Task<int> AddUserAddressAsync(AddressAggregate address);
    public Task RemoveUserAddressAsync(int userId);
    public Task RemoveAddressByIdAsync(int addressId);
}
