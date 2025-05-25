using APBD12.Exceptions;
using APBD12.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public ClientsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpDelete("{idClient:int}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            try
            {
                await _dbService.DeleteClientAsync(idClient);
                return Ok(new { message = "Klient został pomyślnie usunięty" });
            }
            catch (ClientNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ClientHasTripsException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Wystąpił błąd podczas usuwania klienta" });
            }
        }
    }
}
