using Nicknames.Shared.Entities;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Nicknames.Shared.Entities
{
    public class User
    {
#if NETCOREAPP
        [Required(ErrorMessage = "The name field is required.")]
#endif
        public int Id { get; set; }
#if NETCOREAPP
        [Required(ErrorMessage = "The game id field is required.")]
        public string? GameId { get; set; }
#else
        public string GameId { get; set; }
#endif

#if NETCOREAPP
        [Required(ErrorMessage = "The platform field is required.")]
#endif
        public Platform Platform { get; set; }

#if NETCOREAPP
        [MaxLength(80, ErrorMessage = "Max length of name cannot exceed 80 characters.")]

        public string? Nickname { get; set; }
#else
        public string Nickname { get; set; }
#endif
#if NETCOREAPP
        public string? Token { get; set; }
#else
        public string Token { get; set; }
#endif
    }
}