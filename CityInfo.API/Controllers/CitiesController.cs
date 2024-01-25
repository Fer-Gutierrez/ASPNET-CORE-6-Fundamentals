using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase
    {
        //private readonly CityDataStore _cityDataStore;

        //public CitiesController(CityDataStore cityDataStore)
        //{
        //    _cityDataStore = cityDataStore;
        //} 

        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int MaxCitiesPageSize = 20;
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper) { 
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities(string? name,string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if(pageSize  > MaxCitiesPageSize) pageSize = MaxCitiesPageSize;

            var (cities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery,pageNumber, pageSize);
            var citiesDto = _mapper.Map<List<CityWithoutPointOfInterestDto>>(cities);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(citiesDto);
        }

        /// <summary>
        /// Get city by id
        /// </summary>
        /// <param name="id">City id to returns</param>
        /// <param name="includePointsOfInterest">If the city to return includes the point of interests</param>
        /// <returns>an IActionResult</returns>
        /// <response code="200">Returns the requested City</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if (city == null) return NotFound();
            if(includePointsOfInterest) return Ok(_mapper.Map<CityDto>(city));
            return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
        }
    }
}
