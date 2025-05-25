using APBD12.Data;
using APBD12.Exceptions;
using APBD12.Models;
using APBD12.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace APBD12.Services;

public class DbService : IDbService
{
    private readonly _2019sbdContext _context;

    public DbService(_2019sbdContext context)
    {
        _context = context;
    }

    public async Task<GetTripsResponseDTO> GetTripsAsync(int page = 1, int pageSize = 10)
    {
        var trips = await _context.Trips
            .Include(t => t.IdCountries)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync();

        var totalCount = trips.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var pagedTrips = trips
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom.ToString("yyyy-MM-dd"),
                DateTo = t.DateTo.ToString("yyyy-MM-dd"),
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDTO { Name = c.Name }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToList();

        return new GetTripsResponseDTO
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = pagedTrips
        };
    }

    public async Task DeleteClientAsync(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            throw new ClientNotFoundException($"Klient o ID {idClient} nie istnieje.");
        }

        if (client.ClientTrips.Any())
        {
            throw new ClientHasTripsException(
                $"Klient o ID {idClient} ma przypisane wycieczki i nie może zostać usunięty.");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }

    public async Task AddClientToTripAsync(int idTrip, AddClientToTripRequestDTO request)
    {
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
        if (trip == null)
        {
            throw new TripNotFoundException($"Wycieczka o ID {idTrip} nie istnieje.");
        }

        if (trip.DateFrom <= DateTime.Now)
        {
            throw new TripAlreadyStartedException(
                $"Nie można zapisać się na wycieczkę o ID {idTrip}, ponieważ już się odbyła lub jest w trakcie.");
        }


        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == request.Pesel);
        Client client;

        if (existingClient != null)
        {
            client = existingClient;


            var isClientAlreadyAssigned = await _context.ClientTrips
                .AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);

            if (isClientAlreadyAssigned)
            {
                throw new ClientAlreadyAssignedException(
                    $"Klient o PESEL {request.Pesel} jest już zapisany na wycieczkę o ID {idTrip}.");
            }
        }
        else
        {
            client = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Telephone = request.Telephone,
                Pesel = request.Pesel
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }


        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = request.PaymentDate
        };

        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();
    }
}