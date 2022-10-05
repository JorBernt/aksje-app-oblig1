using test_backend.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace test_backend.DAL
{
    public class StockContext : DbContext
    {
        public StockContext (DbContextOptions<StockContext> options) : base(options)
        {
            Database.EnsureCreated();


        }
        public DbSet<Stock> Stocks { get; set; }



    }
}
