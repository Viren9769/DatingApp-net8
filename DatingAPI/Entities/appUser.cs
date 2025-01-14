namespace DatingAPI.Entities
{
    public class appUser
    {
        public int Id { get; set; }
        public required string userName { get; set; }
        public required byte[]  PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; } 

    }
}
