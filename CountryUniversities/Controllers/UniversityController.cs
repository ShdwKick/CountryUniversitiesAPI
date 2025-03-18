using CountryUniversities.Services;
using Microsoft.AspNetCore.Mvc;

namespace CountryUniversities.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UniversityController
    {
        private readonly IUniversityService _universityService;

        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
        }

        [HttpGet]
        public ActionResult<bool> ExtractData()
        {
            _universityService.ExtractDataForCountriesAsync();
            return true;
        }
        
        [HttpGet]
        public ActionResult<bool> GetData()
        {
            _universityService.ExtractDataForCountriesAsync();
            return true;
        }
        

    }
}
