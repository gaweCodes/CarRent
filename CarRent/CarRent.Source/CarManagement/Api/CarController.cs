using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.SearchHelper;
using CarRent.Source.CarManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CarManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CarDto>>> Get()
        {
            try
            {
                return await _carService.GetAllAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest("Id must be a valid Guid");
                return await _carService.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CarDto carDto)
        {
            try
            {
                if (carDto == null) return BadRequest($"{nameof(carDto)} can not not be null!");
                await _carService.AddAsync(carDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] CarDto carDto)
        {
            try
            {
                await _carService.UpdateAsync(carDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _carService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<CarDto>>> Search([FromQuery] string carNumber, [FromQuery] Guid? modelId)
        {
            try
            {
                return await _carService.Search(new CarSearch { CarNumber = carNumber, CarModelid = modelId });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}