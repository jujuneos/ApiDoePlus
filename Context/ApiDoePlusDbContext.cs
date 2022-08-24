using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiDoePlus.Context;

public class ApiDoePlusDbContext : IdentityDbContext<ApplicationUser>
{
    public ApiDoePlusDbContext(DbContextOptions<ApiDoePlusDbContext> options) : base(options)
    {
    }
}
