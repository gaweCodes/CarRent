using CarRent.Source.Database;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CarManagement.Api
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