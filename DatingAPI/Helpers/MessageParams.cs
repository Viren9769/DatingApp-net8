using Microsoft.AspNetCore.Mvc.Formatters;

namespace DatingAPI.Helpers
{
    public class MessageParams : PaginationParams
    {
        public  string? Username {  get; set; }

        public string Container { get; set; } = "Unread";
    }
}
