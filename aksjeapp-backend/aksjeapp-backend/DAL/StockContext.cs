using aksjeapp_backend.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using aksjeapp_backend.DAL;

namespace aksjeapp_backend.DAL
{
    public class Customers {
        public int SocialSecurityNumber { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Address { get; set; }
        List<Transaction> Transactions { get; set; }
        public virtual PostalAreas PostalArea { get; set; }
    }
    public class PostalAreas(){
        string PostalCode { get; set; }
        string PostCity { get; set; }
    }
        
    public class StockContext : DbContext
    {
        
        public StockContext (DbContextOptions<StockContext> options) : base(options)
        {
            Database.EnsureCreated();


        }
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Customers> Customers { get; set; }
        
        public DbSet<PostalAreas> PostalAreas { get; set; }

        protected override void OnConfiguration(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
