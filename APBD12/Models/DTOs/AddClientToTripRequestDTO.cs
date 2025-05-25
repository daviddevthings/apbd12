using System.ComponentModel.DataAnnotations;

namespace APBD12.Models.DTOs;

public class AddClientToTripRequestDTO
{
    [Required(ErrorMessage = "Imię jest wymagane")]
    [MaxLength(120, ErrorMessage = "Imię nie może przekraczać 120 znaków")]
    public string FirstName { get; set; } = null!;
    
    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [MaxLength(120, ErrorMessage = "Nazwisko nie może przekraczać 120 znaków")]
    public string LastName { get; set; } = null!;
    
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
    [MaxLength(120, ErrorMessage = "Email nie może przekraczać 120 znaków")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Numer telefonu jest wymagany")]
    [MaxLength(120, ErrorMessage = "Numer telefonu nie może przekraczać 120 znaków")]
    [RegularExpression(@"^\d{3}-\d{3}-\d{3}$", ErrorMessage = "Numer telefonu powinien być w formacie XXX-XXX-XXX")]
    public string Telephone { get; set; } = null!;
    
    [Required(ErrorMessage = "PESEL jest wymagany")]
    [MaxLength(120, ErrorMessage = "PESEL nie może przekraczać 120 znaków")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL musi mieć dokładnie 11 cyfr")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL powinien składać się z 11 cyfr")]
    public string Pesel { get; set; } = null!;
    
    public int IdTrip { get; set; }
    
    [Required(ErrorMessage = "Nazwa wycieczki jest wymagana")]
    public string TripName { get; set; } = null!;
    
    public DateTime? PaymentDate { get; set; }
}
