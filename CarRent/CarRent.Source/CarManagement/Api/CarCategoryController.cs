using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.Common;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CarManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarCategoryController : Controller
    {
        private readonly ICrudService<CarCategory> _carCategoryCrudService;
        private readonly IMapper _mapper;
        public CarCategoryController(ICrudService<CarCategory> carCategoryCrudService, IMapper mapper)
        {
            _carCategoryCrudService = carCategoryCrudService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<CarCategoryDto>>> Get()
        {
            try
            {
                var categoryList = await _carCategoryCrudService.GetAllAsync();
                var listToReturn = _mapper.Map<List<CarCategory>, List<CarCategoryDto>>(categoryList).OrderBy(cc => cc.DailyFee).ToList();
                return listToReturn;
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
                var carCategoryDto = _mapper.Map<CarCategory, CarCategoryDto>(await _carCategoryCrudService.GetByIdAsync(id));
                if (carCategoryDto == null) return NotFound(null);
                return carCategoryDto;
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
                await _carCategoryCrudService.AddAsync(_mapper.Map<CarCategoryDto, CarCategory>(carCategoryDto));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] CarCategoryDto carCategoryDto)
        {
            try
            {
                if (id == Guid.Empty) return NotFound(null);
                var carCategory = await _carCategoryCrudService.GetByIdAsync(id);
                carCategory.Name = carCategoryDto.Name;
                carCategory.DailyFee = carCategoryDto.DailyFee;
                await _carCategoryCrudService.UpdateAsync(carCategory);
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
                if (await _carCategoryCrudService.CheckIfExistAsync(id) == false) return NotFound(null);
                await _carCategoryCrudService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}