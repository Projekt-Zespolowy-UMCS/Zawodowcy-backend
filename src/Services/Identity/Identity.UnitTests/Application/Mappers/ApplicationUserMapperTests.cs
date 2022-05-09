using System;
using System.ComponentModel.Design;
using AutoMapper;
using Identity.Application.DTO;
using Identity.Application.DTO.RegisteringUser;
using Identity.Application.Mappers;
using Identity.Application.Mappers.UserMapper;
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
        var mapperStub = new Mock<IMapper<CountryInfoAggregate, CountryInfoDto>>();
        var countryInfoDto = new CountryInfoDto()
        {
            ISO = "USA",
            Name = "United States of America"
        };
        mapperStub.Setup(x => x.MapToEntity(countryInfoDto))
            .Returns(new CountryInfoAggregate("USA", "United States of America"));
        var userMapper = new ApplicationUserMapper(mapperStub.Object);
        
        //act
        var appUser = userMapper.MapToEntity(GetFakeRegisterUser(countryInfoDto));

        //assert
        Assert.NotNull(appUser);
    }

    public RegisterApplicationUserDto GetFakeRegisterUser(CountryInfoDto countryInfoDto)
    {
        return new RegisterApplicationUserDto()
        {
            City = "Warsaw",
            CountryInfo = countryInfoDto,
            Email = "test@test.com",
            FirstName = "Andrzej",
            LastName = "Daniluk",
            Password = "Password@1234",
            PhoneNumber = "123456789",
            State = "State",
            Street = "Street",
            ZipCode = "12345",
            UserName = "Danilukson",
        };
    }

}
