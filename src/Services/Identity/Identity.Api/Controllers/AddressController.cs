using Identity.Application.DTO.Address;
using Identity.Application.Mappers.UserMapper.AddressMapper;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace idsserver;

[Route("api/[controller]")]
public class AddressController: ControllerBase
{
    private readonly IAddressRepository _addressRepository;
    private readonly IAddressMapper _addressMapper;

    public AddressController(IAddressRepository addressRepository,
        IAddressMapper addressMapper)
    {
        _addressRepository = addressRepository;
        _addressMapper = addressMapper;
    }
    
    [Route("add/{userId}")]
    [HttpPost]
    public async Task<IActionResult> AddUserAddress(AddAddressDto addressDto, string userId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var address = _addressMapper.MapToEntity(addressDto);
        var addressAdded = await _addressRepository.AddUserAddressAsync(address, userId);

        return Ok(addressAdded);
    }

    [Route("remove/byUserId/{userId}")]
    [HttpDelete]
    public async Task<IActionResult> RemoveUserAddress(string userId)
    {
        var isRemoved = await _addressRepository.RemoveUserAddressAsync(userId);
        
        if (isRemoved)
            return Ok();
        return NotFound();
    }
    
    [Route("remove/{addressId}")]
    [HttpDelete]
    public async Task<IActionResult> RemoveUserAddress(int addressId)
    {
        var isRemoved = await _addressRepository.RemoveAddressByIdAsync(addressId);
        
        if (isRemoved)
            return Ok();
        return NotFound();
    }

    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> UpdateUserAddress(UpdateAddressDto addressDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var address = _addressMapper.MapToEntity(addressDto);
        var addressUpdated = await _addressRepository.UpdateUserAddress(address);
        
        return Ok(addressUpdated);
    }
}
