using Microsoft.AspNetCore.Identity;

namespace Projeto02.Models;

public class ApplicationUser : IdentityUser
{
    public decimal Cpf { get; set; }
}
