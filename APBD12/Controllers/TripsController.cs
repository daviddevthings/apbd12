using APBD12.Exceptions;
using APBD12.Models.DTOs;
using APBD12.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD12.Controllers
{
    [Route("api")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public TripsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("trips")]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { message = "Nieprawidłowe parametry stronicowania" });
            }
            
            try
            {
                var response = await _dbService.GetTripsAsync(page, pageSize);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Wystąpił błąd podczas pobierania wycieczek" });
            }
        }

        [HttpPost("trips/{idTrip:int}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] AddClientToTripRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Count > 0)
                    .SelectMany(e => e.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(new { message = "Dane wejściowe są nieprawidłowe", errors });
            }

            try
            {
                await _dbService.AddClientToTripAsync(idTrip, request);
                return Ok(new { message = "Klient został pomyślnie dodany do wycieczki" });
            }
            catch (TripNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (TripAlreadyStartedException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ClientAlreadyAssignedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Wystąpił błąd podczas dodawania klienta do wycieczki" });
            }
        }
    }
}
