using System;
using System.ComponentModel.Design;
using AutoMapper;
using Identity.Application.DTO;
using Identity.Application.DTO.Address;
using Identity.Application.DTO.RegisteringUser;
using Identity.Application.Mappers;
using Identity.Application.Mappers.UserMapper;
using Identity.Application.Mappers.UserMapper.AddressMapper;
using Identity.Application.Mappers.UserMapper.CountryInfoMapper;
using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Identity.UnitTests.Application.Mappers;

public class ApplicationUserMapperTests
{
    [Fact]
    public void User_is_correctly_mapped_to_aggregate()
    {
        //arrange
        var addressMapperStub = new Mock<IAddressMapper>();
        var addressDto = GetExampleAddAddressDto();
        var countryInfoAggregate = new CountryInfoAggregate("USA", "United States of America");
        var addressAggregate = new AddressAggregate("Random", "Warsaw", "Lubelskie", countryInfoAggregate, "08500");

        addressMapperStub.Setup(x => x.MapToEntity(addressDto))
            .Returns(addressAggregate);
        var userMapper = new ApplicationUserMapper(addressMapperStub.Object);
        
        //act
        var appUser = userMapper.MapToEntity(GetFakeRegisterUser());

        //assert
        Assert.NotNull(appUser);
        addressMapperStub.Verify(x => x.MapToEntity(It.IsAny<AddAddressDto>()));
    }

    public RegisterApplicationUserDto GetFakeRegisterUser()
    {
        return new RegisterApplicationUserDto()
        {
            Email = "test@test.com",
            FirstName = "Andrzej",
            LastName = "Daniluk",
            Password = "Password@1234",
            PhoneNumber = "123456789",
            Address = GetExampleAddAddressDto()
        };
    }

    private AddAddressDto GetExampleAddAddressDto()
    {
        return new AddAddressDto()
        {
            City = "Warsaw",
            Country = GetExampleCountryInfoDto(),
            State = "Lubelskie",
            Street = "Random",
            ZipCode = "08500"
        };
    }

    private CountryInfoDto GetExampleCountryInfoDto()
    {
        return new CountryInfoDto()
        {
            ISO = "USA",
            Name = "United States of America"
        };
    }

}
