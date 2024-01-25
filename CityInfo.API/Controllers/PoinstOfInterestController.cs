using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    [Authorize(Policy = "MustBeFromRafaela")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PoinstOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestDto> _logger;
        private readonly IMailService _mailService;
        //private readonly CityDataStore _cityDataStore;

        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PoinstOfInterestController(ILogger<PointsOfInterestDto> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            //_cityDataStore = cityDataStore;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDto>>> GetPoinstOfInterest(int cityId)
        {
            try
            {

                //Para utilizar los claims que se reciben en un TOKEN vamos a hacer de cuenta que un usuario puede acceder a los puntos de interes de una ciudad si vive en esa ciudad.
                //var userCityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
                //if(!await _cityInfoRepository.CityNameMatchesCityId(userCityName, cityId)) return Forbid();


                if (!await _cityInfoRepository.CityExistAsync(cityId))
                {
                    _logger.LogInformation($"El id {cityId} de la ciudad no fue encontrado al intentar GetPointsOfInterest");
                    return NotFound();
                }
                var pointsOfInterest = await _cityInfoRepository.GetPointsOfInteresForCityAsync(cityId);
                var pointsOfInterestDto = _mapper.Map<IEnumerable<PointsOfInterestDto>>(pointsOfInterest);
                return Ok(pointsOfInterestDto);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting point of interest for city with id {cityId},", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointsOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            try
            {
                if (!await _cityInfoRepository.CityExistAsync(cityId))
                {
                    _logger.LogInformation($"El id {cityId} de la ciudad no fue encontrado al intentar GetPointsOfInterest");
                    return NotFound();
                };

                var pointOfInterest = await _cityInfoRepository.GetPointOfInteresForCityAsync(cityId, pointOfInterestId);

                if (pointOfInterest == null)
                {
                    _logger.LogInformation($"El id {cityId} del punto de interes no fue encontrado en la ciudad con id {cityId} al intentar GetPointsOfInterest");
                    return NotFound();
                }
                var pointOfInterestDto = _mapper.Map<PointsOfInterestDto>(pointOfInterest);
                return Ok(pointOfInterestDto);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while you are getting point of interest from de city with id {cityId} - Message: {ex.Message} - InnerException: {ex.InnerException}");
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PointsOfInterestDto>> CreatePointOfIterest(int cityId, PointOfInterestCreationDto pointOfInterest)
        {
            try
            {
                //ModelState hace referencia a los atributos que coloco en la clase de los objetos que recibo por parametros
                if (!ModelState.IsValid) return BadRequest();

                if(!await _cityInfoRepository.CityExistAsync(cityId))
                {
                    _logger.LogInformation($"El id {cityId} de la ciudad no fue encontrado al intentar GetPointsOfInterest");
                    return NotFound();
                }

                var newPointofInterest = _mapper.Map<PointOfInterest>(pointOfInterest);

                await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, newPointofInterest);

                await _cityInfoRepository.SaveChangesAsync();

                var createdPointOfInterestDto = _mapper.Map<PointsOfInterestDto>(newPointofInterest);

                return CreatedAtRoute(
                    "GetPointOfInterest",
                    new
                    {
                        CityId = cityId,
                        pointOfInterestId = createdPointOfInterestDto.Id
                    },

                    createdPointOfInterestDto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while you are creating a point of interest into the city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpPut("{pointofinterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestUpdateDto pointOfInterest)
        {
            try
            {
                if (!await _cityInfoRepository.CityExistAsync(cityId))
                {
                    _logger.LogInformation($"The city with id: {cityId} not found trying UpdatePointOfInterest");
                    return NotFound();
                }

                var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInteresForCityAsync(cityId, pointOfInterestId);
                if (pointOfInterestEntity == null)
                {
                    _logger.LogInformation($"The point of interest with id: {pointOfInterestId} not found in the city with id: {cityId} trying UpdatePointOfInterest");
                    return NotFound();
                }
                
                _mapper.Map(pointOfInterest, pointOfInterestEntity);

                await _cityInfoRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception while you are updating a point of interest into the city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }


        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {

            try
            {
                if (!await _cityInfoRepository.CityExistAsync(cityId))
                {
                    _logger.LogInformation($"The city with id: {cityId} not found trying UpdatePointOfInterest");
                    return NotFound();
                }

                var pointOfInterestEntity= await _cityInfoRepository.GetPointOfInteresForCityAsync(cityId, pointOfInterestId);
                if (pointOfInterestEntity == null)
                {
                    _logger.LogInformation($"The point of interest with id: {pointOfInterestId} not found in the city with id: {cityId} trying UpdatePointOfInterest");
                    return NotFound();
                }

                var pointOfInterestToPatch = _mapper.Map<PointOfInterestUpdateDto>(pointOfInterestEntity);

                patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

                if (!ModelState.IsValid) return BadRequest();

                if (!TryValidateModel(pointOfInterestToPatch)) return BadRequest();

                _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

                await _cityInfoRepository.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while you are partially updating a point of interest into the city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("{pointofinterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            try
            {
                if (!await _cityInfoRepository.CityExistAsync(cityId))
                {
                    _logger.LogInformation($"The city with id: {cityId} not found trying UpdatePointOfInterest");
                    return NotFound();
                }

                var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInteresForCityAsync(cityId, pointOfInterestId);
                if (pointOfInterestEntity == null)
                {
                    _logger.LogInformation($"The point of interest with id: {pointOfInterestId} not found in the city with id: {cityId} trying UpdatePointOfInterest");
                    return NotFound();
                }

                _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

                await _cityInfoRepository.SaveChangesAsync();

                _mailService.Send("Point of interes deleted", $"The Point of Interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while you are deleting a point of interest into the city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }
    }
}