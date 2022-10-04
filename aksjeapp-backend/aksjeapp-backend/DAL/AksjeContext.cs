using AksjeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AksjeAPI.DAL
{
    public class AksjeContext : DbContext
    {
        public AksjeContext (DbContextOptions<AksjeContext> options) : base(options)
        {
            Database.EnsureCreated();


        }
        public DbSet<Aksje> Aksjer { get; set; }



    }
}
