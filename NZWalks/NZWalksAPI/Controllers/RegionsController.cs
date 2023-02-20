using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            // return DTO Regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach( domainRegion => {
            //    var regionDTO = new Models.DTO.Region() {   
            //       Id = domainRegion.Id,
            //        Code = domainRegion.Code,
            //        Name = domainRegion.Name,
            //        Area = domainRegion.Area,
            //        Lat = domainRegion.Lat,
            //        Long = domainRegion.Long,
            //        Population = domainRegion.Population,
            //  };

            //    regionsDTO.Add(regionDTO);
            //});
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]

        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]

        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Convert request (DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Population = addRegionRequest.Population
            };
            // Pass details to Repository
            region = await regionRepository.AddAsync(region);
            // Convert back to DTO
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Get region from database
            var region = await regionRepository.DeleteAsync(id);
            // If null return not found
            if (region == null)
            {
                return NotFound();
            }
            // convert back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            // return OK
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest region)
        {
            
            //var regionDomainModel = mapper.Map<Models.Domain.Region>(region);
            var regionDomainModel = new Models.Domain.Region()
            {
                Code = region.Code,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Area = region.Area,
                Population = region.Population
            };
            var updatedRegion = await regionRepository.UpdateAsync(id, regionDomainModel);
            // If null return not found
            if (updatedRegion == null)
            {
                return NotFound();
            }
            var updatedRegionAsDTO = mapper.Map<Models.DTO.Region>(updatedRegion);
            return Ok(updatedRegionAsDTO);
        }
    }
}
