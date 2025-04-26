using Microsoft.AspNetCore.Identity;
using System.Net.Sockets;

namespace Ecom.Core.Entities;

public class AppUser:IdentityUser
{
    public string DispalyName { get; set; }
    public Address Address { get; set; }
}
