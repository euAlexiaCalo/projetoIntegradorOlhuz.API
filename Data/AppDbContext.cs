using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OlhuzApiWeb.Model;
using projetoIntegradorOlhuz.API.Models;
using System.Data;

namespace projetoIntegradorOlhuz.API.Data
{
    public class AppDbContext : DbContext 
    {
        public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
         
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet <Configuracao> Configuracoes { get; set; }
    }
}
