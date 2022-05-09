namespace Identity.Application.DTO.Address;

public class UpdateAddressDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public CountryInfoDto Country { get; set; }
    public string ZipCode { get; set; }
}
