using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class AddressRepository: IAddressRepository
{
    private readonly AppIdentityDbContext _context;
    
    public AddressRepository(AppIdentityDbContext context)
    {
        _context = context;
    }
    
    public async Task<AddressAggregate?> GetUserAddressAsync(int id)
    {
        return await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<AddressAggregate> AddUserAddressAsync(AddressAggregate address, string userId)
    {
        var country = await _context.CountryInfos.FirstOrDefaultAsync(x => x.Id == address.CountryId || x.ISO == address.Country.ISO);
        address.SetCountry(country);
        _context.Entry(address.Country).State = EntityState.Detached;
        _context.Entry(address).State = EntityState.Added;
    
        var addedEntity = _context.Addresses.Add(address);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            throw new ArgumentException($"There is no user with ID {userId}.");

        user.SetUserAddress(address);
        await _context.SaveChangesAsync();
        return addedEntity?.Entity;
    }

    public async Task<AddressAggregate> CreateAddressAsync(AddressAggregate address)
    {
        var country = await _context.CountryInfos.FirstOrDefaultAsync(x => x.Id == address.CountryId || x.ISO == address.Country.ISO);
        address.SetCountry(country);
        _context.Entry(address.Country).State = EntityState.Detached;
        _context.Entry(address).State = EntityState.Added;

        var addedEntity = _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        return addedEntity.Entity;
    }

    public async Task<AddressAggregate> UpdateUserAddress(AddressAggregate address)
    {
        var country = await _context.CountryInfos.FirstOrDefaultAsync(x => x.Id == address.CountryId || x.ISO == address.Country.ISO);
        address.SetCountry(country);
        // _context.Entry(address.Country).State = EntityState.Unchanged;
        _context.Entry(address.Country).State = EntityState.Detached;
        _context.Entry(address).State = EntityState.Added;

        var updatedEntity = _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
        return updatedEntity?.Entity;
    }

    public async Task<bool> RemoveUserAddressAsync(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        var address = await _context.Addresses
            .Where(x => x.Id == user.AddressId)
            .FirstOrDefaultAsync();

        if (address is not null)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> RemoveAddressByIdAsync(int addressId)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(entity => entity.Id == addressId);

        if (address is not null)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
