using AutoMapper;
using Identity.Application.DTO;
using Identity.Application.Mappers;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Microsoft.AspNetCore.Mvc;

namespace idsserver;

[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountriesController(ICountryRepository countryRepository,
        IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }
    
    [Route("all")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var countries = await _countryRepository.GetAllAsync();
        var countriesDto = countries
            .Select(x => _mapper.Map<CountryInfoDto>(x))
            .OrderBy(x => x.Name);
        return Ok(countriesDto);
    }

    [Route("{iso}")]
    [HttpGet]
    public async Task<IActionResult> Get(string iso)
    {
        var country = await _countryRepository.GetAsync(iso);
        if (country != null)
            return Ok(country);
        return NotFound("There is no country with that ISO code.");
    }
}
