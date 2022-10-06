using ApiDoePlus.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDoePlus.Context;

public class FotosContext : DbContext
{
    public FotosContext(DbContextOptions<FotosContext> options) : base(options)
    { }

    public DbSet<Fotos> fotos { get; set; }

    public void RegistrarAlterado(object entidade)
    {
        Entry(entidade).State = EntityState.Modified;
    }
}
