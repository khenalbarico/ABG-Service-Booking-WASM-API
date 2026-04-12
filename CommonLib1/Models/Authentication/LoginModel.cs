using System.ComponentModel.DataAnnotations;

namespace CommonLib1.Models.Authentication;

public sealed class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email   { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}
