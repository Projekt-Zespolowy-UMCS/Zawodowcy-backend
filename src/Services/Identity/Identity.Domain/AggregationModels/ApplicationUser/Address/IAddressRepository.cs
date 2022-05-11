namespace Identity.Domain.AggregationModels.ApplicationUser.Address;

public interface IAddressRepository
{
    public Task<AddressAggregate?> GetUserAddressAsync(int id);
    public Task<AddressAggregate> AddUserAddressAsync(AddressAggregate address);
    public Task<bool> RemoveUserAddressAsync(string userId);
    public Task<bool> RemoveAddressByIdAsync(int addressId);
}
