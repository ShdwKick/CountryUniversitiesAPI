using Microsoft.AspNetCore.Mvc;
using CountryUniversities.Repositories;
using CountryUniversities.Services;


namespace CountryUniversities.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IUniversityService _universityService;

        public UniversityController(IUniversityRepository universityRepository, IUniversityService universityService)
        {
            _universityRepository = universityRepository;
            _universityService = universityService;
        }

        [HttpPost("load")]
        public async Task<IActionResult> LoadUniversities([FromServices] IUniversityService universityService)
        {
            await universityService.ExtractDataForCountriesAsync();
            return Ok("Data loaded successfully");
        }
        
        [HttpGet("universities")]
        public async Task<IActionResult> GetUniversities([FromQuery] string? country, [FromQuery] string? name)
        {
            var universities = await _universityService.GetUniversitiesAsync(country, name);
            return Ok(universities);
        }
    }
}
