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
        public virtual List<TransactionBought> TransactionsBought { get; set; } = null!;
        public virtual List<TransactionSold> TransactionsSold { get; set; } = null!;
        public virtual Portfolio? Portfolio { get; set; }
        public virtual PostalAreas PostalArea { get; set; } = null!;
    }
    public class PostalAreas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PostalCode { get; set; }
        public string PostCity { get; set; }
    }
    public class Users
    {
        [Key]
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }
    public class TransactionBought
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoughtId { get; set; }

        //public string SocialSecurityNumber { get; set; }
        public string Date { get; set; }
        public string Symbol { get; set; }
        public int Amount { get; set; }

        private double totalPrice;
        public double TotalPrice
        {
            get
            {
                return totalPrice;
            }
            set
            {
                totalPrice = Math.Round(value, 2);
            }
        }

    }
    public class TransactionSold
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SoldId { get; set; }
        public string Date { get; set; } //TODO: Bytte til datetime datatype?
        public string Symbol { get; set; }
        public int Amount { get; set; }

        private double totalPrice;
        public double TotalPrice
        {
            get
            {
                return this.totalPrice;
            }
            set
            {
                this.totalPrice = Math.Round(value, 2);
            }
        }
    }
    public class PortfolioList
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PortfolioListId { get; set; }
        public string Symbol { get; set; }
        public string? Name { get; set; }
        public int Amount { get; set; } = 0;

        private double change;
        private double value;
        public double Change
        {
            get
            {
                return this.change;
            }
            set
            {
                this.change = Math.Round(value, 2);
            }
        }

        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = Math.Round(value, 2);
            }
        }
        [ForeignKey("PortfolioId")]
        public int PortfolioId { get; set; }
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

        public DbSet<Users> Users { get; set; }

        public DbSet<TransactionBought> TransactionsBought { get; set; }

        public DbSet<TransactionSold> TransactionsSold { get; set; }

        public DbSet<StockChangeValue> StockChangeValues { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<PortfolioList> PortfolioList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
