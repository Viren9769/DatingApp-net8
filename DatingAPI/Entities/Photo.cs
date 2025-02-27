using System.ComponentModel.DataAnnotations.Schema;

namespace DatingAPI.Entities
{
    [Table("Photos")]
    public class Photo
    {
      public int Id { get; set; }
        public required string Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        //Navigate
        public int AppUserId { get; set; }
        public appUser appUser { get; set; } = null!;

    }
}
