using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CarManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Das ist ein test";
        }
    }
}