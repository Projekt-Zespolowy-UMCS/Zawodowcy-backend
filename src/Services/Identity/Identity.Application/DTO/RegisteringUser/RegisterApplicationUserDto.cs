namespace Identity.Application.DTO.RegisteringUser;

public class RegisterApplicationUserDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public CountryInfoDto CountryInfo { get; set; }
    public string ZipCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
}
