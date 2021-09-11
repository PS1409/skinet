using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
              if(!userManager.Users.Any())
              {
                  var user = new AppUser{
                     DisplayName = "Pradeep Sharma",
                     Email = "ps14091988@gmail.com",
                     UserName = "ps14091988@gmail.com",
                     Address = new Address {
                         FirstName = "Pradeep",
                         LastName= "Sharma",
                         Street = "T-14, 1103, Nirala Estate",
                         City= "Greater Noida West",
                         State = "Uttar Pradesh",
                         ZipCode = "201308"
                     }
                  };
                  await userManager.CreateAsync(user, "Pa$$w0rd");
              }
        }
    }
}