using System.Drawing;

namespace DatingAPI.DTOs
{
    public class UserDTO
    {
        public required string Username {  get; set; }
        public required string KnownAs { get; set; }
        public required string token { get; set; }
        public required string Gender { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
