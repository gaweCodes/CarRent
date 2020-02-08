using System;
using System.Collections.Generic;
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
    public class BrandController : Controller
    {
        private readonly ICrudService<Brand> _brandCrudService;
        private readonly IMapper _mapper;
        public BrandController(ICrudService<Brand> brandCrudService, IMapper mapper)
        {
            _brandCrudService = brandCrudService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<BrandDto>>> Get()
        {
            try
            {
                return _mapper.Map<List<Brand>, List<BrandDto>>(await _brandCrudService.GetAllAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetById(Guid id)
        {
            try
            {

                var brandDto = _mapper.Map<Brand, BrandDto>(await _brandCrudService.GetByIdAsync(id));
                if (brandDto == null) return NotFound(null);
                return brandDto;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BrandDto brandDto)
        {
            try
            {
                if (brandDto == null)  return BadRequest($"{nameof(brandDto)} can not not be null!");
                await _brandCrudService.AddBrandAsync(_mapper.Map<BrandDto, Brand>(brandDto));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        /*[HttpPut("{id}")]
        public async Task<ActionResult> UpdateBrand([FromBody] BrandDto brandDto)
        {
            try
            {
                if (brandDto == null) return BadRequest($"{nameof(brandDto)} can not not be null!");
                if (await _brandCrudService.CheckIfExistAsync(brandDto.Id) == false) return NotFound(null);
                await _brandCrudService.UpdateAsync(_mapper.Map<BrandDto, Brand>(brandDto));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }*/
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(Guid id)
        {
            try
            {
                if (await _brandCrudService.CheckIfExistAsync(id) == false) return NotFound(null);
                await _brandCrudService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}