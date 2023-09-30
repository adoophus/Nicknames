using Nicknames.Shared.Entities;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Nicknames.Shared.Entities;

public class User
{
    [Required(ErrorMessage = "The name field is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "The game id field is required.")]
    public string? GameId { get; set; }

    [Required(ErrorMessage = "The platform field is required.")]
    public Platform Platform { get; set; }

    [MaxLength(80, ErrorMessage = "Max length of name cannot exceed 80 characters.")]
    public string? Nickname { get; set; }

    public string? Token { get; set; }
}
