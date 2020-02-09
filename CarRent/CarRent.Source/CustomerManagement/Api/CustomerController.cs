using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarRent.Source.Common;
using CarRent.Source.CustomerManagement.Domain;
using CarRent.Source.CustomerManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CustomerManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICrudService<Customer> _customerCrudService;
        private readonly IMapper _mapper;
        public CustomerController(ICrudService<Customer> customerCrudService, IMapper mapper)
        {
            _customerCrudService = customerCrudService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> Get()
        {
            try
            {
                var customerList = await _customerCrudService.GetAllAsync();
                var listToReturn = _mapper.Map<List<Customer>, List<CustomerDto>>(customerList).OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
                return listToReturn;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(Guid id)
        {
            try
            {
                var customerDto = _mapper.Map<Customer, CustomerDto>(await _customerCrudService.GetByIdAsync(id));
                if (customerDto == null) return NotFound(null);
                return customerDto;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CustomerDto customerDto)
        {
            try
            {
                if (customerDto == null) return BadRequest($"{nameof(customerDto)} can not not be null!");
                await _customerCrudService.AddAsync(_mapper.Map<CustomerDto, Customer>(customerDto));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] CustomerDto customerDto)
        {
            try
            {
                if (id == Guid.Empty) return NotFound(null);
                var customer = await _customerCrudService.GetByIdAsync(id);
                customer.FirstName = customerDto.FirstName;
                customer.LastName = customerDto.LastName;
                customer.Address = customerDto.Address;
                await _customerCrudService.UpdateAsync(customer);
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
                if (await _customerCrudService.CheckIfExistAsync(id) == false) return NotFound(null);
                await _customerCrudService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
