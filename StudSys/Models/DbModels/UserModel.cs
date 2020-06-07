using Microsoft.AspNetCore.Identity;

namespace StudSys.Models.DbModels
{
    public class UserModel: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Role { get; set; }

#pragma warning disable CA1819 // Свойства не должны возвращать массивы
        public byte[] AvatarImage { get; set; }
#pragma warning restore CA1819 // Свойства не должны возвращать массивы
        public int StudGroupId { get;set;}
    }
}