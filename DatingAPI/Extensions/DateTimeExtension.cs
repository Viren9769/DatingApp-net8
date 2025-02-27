using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.WebSockets;

namespace DatingAPI.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;
        }
    }
}
