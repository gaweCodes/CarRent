using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.SearchHelper;
using CarRent.Source.CarManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CarManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarCategoryController : Controller
    {
        private readonly ICarCategoryService _carCategoryService;
        public CarCategoryController(ICarCategoryService carCategoryService)
        {
            _carCategoryService = carCategoryService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CarCategoryDto>>> Get()
        {
            try
            {
                return await _carCategoryService.GetAllAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CarCategoryDto>> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest("Id must be a valid Guid");
                return await _carCategoryService.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CarCategoryDto carCategoryDto)
        {
            try
            {
                if (carCategoryDto == null)  return BadRequest($"{nameof(carCategoryDto)} can not not be null!");
                await _carCategoryService.AddAsync(carCategoryDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] CarCategoryDto carCategoryDto)
        {
            try
            {
                await _carCategoryService.UpdateAsync(carCategoryDto);
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
                await _carCategoryService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<CarCategoryDto>>> Search([FromQuery] string name, [FromQuery] decimal? fee)
        {
            try
            {
                return await _carCategoryService.Search(new CarCategorySearch{Name = name, Fee = fee});
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}