using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.CustomerManagement.Dtos;
using CarRent.Source.CustomerManagement.SearchHelper;
using CarRent.Source.CustomerManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.CustomerManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> Get()
        {
            try
            {
                return await _customerService.GetAllAsync();
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
                if (id == Guid.Empty) return BadRequest("Id must be a valid Guid");
                return await _customerService.GetByIdAsync(id);
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
                await _customerService.AddAsync(customerDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] CustomerDto customerDto)
        {
            try
            {
                await _customerService.UpdateAsync(customerDto);
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
                await _customerService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<CustomerDto>>> Search([FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] string address)
        {
            try
            {
                return await _customerService.Search(new CustomerSearch{LastName = lastName, FirstName = firstName, Address = address});
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
