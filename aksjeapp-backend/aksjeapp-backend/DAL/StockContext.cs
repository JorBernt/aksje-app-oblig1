using aksjeapp_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aksjeapp_backend.DAL
{
    public class Customers
    {
        [Key]
        public string SocialSecurityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public double Balance { get; set; }
        virtual public List<TransactionBought> TransactionsBought { get; set; }
        virtual public List<TransactionSold> TransactionsSold { get; set; }
        virtual public Portfolio Portfolio { get; set; }
        virtual public PostalAreas PostalArea { get; set; }
    }
    public class PostalAreas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PostalCode { get; set; }
        public string PostCity { get; set; }
    }

    public class TransactionBought
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string SocialSecurityNumber { get; set; }
        public string Date { get; set; }
        public string Symbol { get; set; }
        public int Amount { get; set; }
        public double TotalPrice { get; set; }

    }
    public class TransactionSold
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Date { get; set; }
        public string Symbol { get; set; }
        public int Amount { get; set; }
        public double TotalPrice { get; set; }
    }

    public class StockContext : DbContext
    {

        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
            Database.EnsureCreated();


        }
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<PostalAreas> PostalAreas { get; set; }

        //public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionBought> TransactionsBought { get; set; }

        public DbSet<TransactionSold> TransactionsSold { get; set; }

        public DbSet<StockChangeValue> StockChangeValues { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<StockOverview> PortfolioList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
