using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using projetoIntegradorOlhuz.API.Models;

namespace projetoIntegradorOlhuz.API.Data
{
    public class AppDbContext : DbContext 
    {
        public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
         
        }

        public DbSet<Usuario> Usuarios { get; set; }
       

    }
}
