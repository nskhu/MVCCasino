using Microsoft.AspNetCore.Identity;

namespace MVCCasino.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdNumber { get; set; }
}