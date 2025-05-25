namespace APBD12.Models.DTOs;

public class GetTripsResponseDTO
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDTO> Trips { get; set; } = new();
}

public class TripDTO
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string DateFrom { get; set; } = null!;
    public string DateTo { get; set; } = null!;
    public int MaxPeople { get; set; }
    public List<CountryDTO> Countries { get; set; } = new();
    public List<ClientDTO> Clients { get; set; } = new();
}

public class CountryDTO
{
    public string Name { get; set; } = null!;
}

public class ClientDTO
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
