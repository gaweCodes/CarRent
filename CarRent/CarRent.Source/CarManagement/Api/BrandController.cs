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
                var brandList = await _brandCrudService.GetAllAsync();
                var listToReturn = _mapper.Map<List<Brand>, List<BrandDto>>(brandList).OrderBy(b => b.Title).ToList();
                return listToReturn;
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
                await _brandCrudService.AddAsync(_mapper.Map<BrandDto, Brand>(brandDto));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] BrandDto brandDto)
        {
            try
            {
                if (id == Guid.Empty) return NotFound(null);
                var brand = await _brandCrudService.GetByIdAsync(id);
                brand.Title = brandDto.Title;
                await _brandCrudService.UpdateAsync(brand);
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