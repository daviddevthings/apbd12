using APBD12.Models.DTOs;

namespace APBD12.Services;

public interface IDbService
{
    Task<GetTripsResponseDTO> GetTripsAsync(int page = 1, int pageSize = 10);
    Task DeleteClientAsync(int idClient);
    Task AddClientToTripAsync(int idTrip, AddClientToTripRequestDTO request);
}
