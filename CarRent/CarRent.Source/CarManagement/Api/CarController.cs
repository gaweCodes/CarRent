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
    public class CarController : Controller
    {
        private readonly ICrudService<Car> _carCrudService;
        private readonly IMapper _mapper;
        public CarController(ICrudService<Car> carCrudService, IMapper mapper)
        {
            _carCrudService = carCrudService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<CarDto>>> Get()
        {
            try
            {
                var carList = await _carCrudService.GetAllAsync();
                var listToReturn = _mapper.Map<List<Car>, List<CarDto>>(carList).OrderBy(cm => cm.CarNumber).ToList();
                return listToReturn;
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
                var carDto = _mapper.Map<Car, CarDto>(await _carCrudService.GetByIdAsync(id));
                if (carDto == null) return NotFound(null);
                return carDto;
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
                await _carCrudService.AddAsync(_mapper.Map<CarDto, Car>(carDto));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] CarDto carDto)
        {
            try
            {
                if (id == Guid.Empty) return NotFound(null);
                var car = await _carCrudService.GetByIdAsync(id);
                car.CarNumber = carDto.CarNumber;
                car.CarModelid = carDto.CarModelid;
                await _carCrudService.UpdateAsync(car);
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
                if (await _carCrudService.CheckIfExistAsync(id) == false) return NotFound(null);
                await _carCrudService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}