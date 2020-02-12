using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRent.Source.ReservationManagement.Domain;
using CarRent.Source.ReservationManagement.Dtos;
using CarRent.Source.ReservationManagement.SearchHelper;
using CarRent.Source.ReservationManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Source.ReservationManagement.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ReservationDto>>> Get()
        {
            try
            {
                return await _reservationService.GetAllAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest("Id must be a valid Guid");
                return await _reservationService.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ReservationDto reservationDto)
        {
            try
            {
                if (reservationDto == null)  return BadRequest($"{nameof(reservationDto)} can not not be null!");
                await _reservationService.AddAsync(reservationDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] ReservationDto reservationDto)
        {
            try
            {
                await _reservationService.UpdateAsync(reservationDto);
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
                await _reservationService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<ReservationDto>>> Search([FromQuery] Guid? carId, [FromQuery] Guid? customerId, [FromQuery] string state, [FromQuery] decimal? totalCost)
        {
            try
            {
                ReservationState? parsedState;
                if (state == "Active")
                    parsedState = ReservationState.Active;
                else if (state == "Closed")
                    parsedState = ReservationState.Closed;
                else
                    parsedState = null;
                return await _reservationService.Search(new ReservationSearch
                    {CarId = carId, CustomerId = customerId, State = parsedState, TotalCost = totalCost});
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}