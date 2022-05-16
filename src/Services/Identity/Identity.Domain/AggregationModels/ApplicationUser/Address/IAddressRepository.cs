namespace Identity.Domain.AggregationModels.ApplicationUser.Address;

public interface IAddressRepository
{
    public Task<AddressAggregate?> GetUserAddressAsync(int id);
    public Task<AddressAggregate> AddUserAddressAsync(AddressAggregate address, string userId);
    public Task<AddressAggregate> UpdateUserAddress(AddressAggregate address);
    public Task<bool> RemoveUserAddressAsync(string userId);
    public Task<bool> RemoveAddressByIdAsync(int addressId);
}
