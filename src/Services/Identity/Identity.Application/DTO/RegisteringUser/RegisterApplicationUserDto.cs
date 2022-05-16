using Identity.Application.DTO.Address;

namespace Identity.Application.DTO.RegisteringUser;

public class RegisterApplicationUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public AddAddressDto? Address { get; set; }
}
