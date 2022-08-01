using ApiDoePlus.Models.Instituicoes;
using ApiDoePlus.Models.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace ApiDoePlus.Context;

public class ApiDoePlusDbContext : DbContext
{
    public ApiDoePlusDbContext(DbContextOptions<ApiDoePlusDbContext> options) : base(options)
    {
    }

    public DbSet<Instituicao>? Instituicoes { get; set; }
    public DbSet<Usuario>? Usuarios { get; set; }
}
