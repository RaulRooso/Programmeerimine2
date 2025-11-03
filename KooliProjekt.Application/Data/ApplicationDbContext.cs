using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BeerSort> BeerSorts {get; set;} 
        public DbSet<BeerBatch> BeerBatches { get; set; }
        public DbSet<Ingredient> Ingredients  { get; set; }
        public DbSet<BatchLog> BatchLogs  { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<TasteLog> TasteLogs  { get; set; }
        public DbSet<User> Users { get; set; }
}
}
