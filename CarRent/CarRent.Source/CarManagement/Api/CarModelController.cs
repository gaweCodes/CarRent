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
    public class CarModelController : Controller
    {
        private readonly ICrudService<CarModel> _carModelCrudService;
        private readonly IMapper _mapper;
        public CarModelController(ICrudService<CarModel> carModelCrudService, IMapper mapper)
        {
            _carModelCrudService = carModelCrudService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<CarModelDto>>> Get()
        {
            try
            {
                var modelList = await _carModelCrudService.GetAllAsync();
                var listToReturn = _mapper.Map<List<CarModel>, List<CarModelDto>>(modelList).OrderBy(cm => cm.Title).ToList();
                return listToReturn;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CarModelDto>> GetById(Guid id)
        {
            try
            {
                var carModelDto = _mapper.Map<CarModel, CarModelDto>(await _carModelCrudService.GetByIdAsync(id));
                if (carModelDto == null) return NotFound(null);
                return carModelDto;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CarModelDto carModelDto)
        {
            try
            {
                if (carModelDto == null)  return BadRequest($"{nameof(carModelDto)} can not not be null!");
                await _carModelCrudService.AddAsync(_mapper.Map<CarModelDto, CarModel>(carModelDto));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] CarModelDto carModelDto)
        {
            try
            {
                if (id == Guid.Empty) return NotFound(null);
                var carModel = await _carModelCrudService.GetByIdAsync(id);
                carModel.Title = carModelDto.Title;
                carModel.BrandId = carModelDto.BrandId;
                carModel.CategoryId = carModelDto.CategoryId;
                await _carModelCrudService.UpdateAsync(carModel);
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
                if (await _carModelCrudService.CheckIfExistAsync(id) == false) return NotFound(null);
                await _carModelCrudService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}