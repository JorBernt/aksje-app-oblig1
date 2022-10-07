﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aksjeapp_backend.Models
{
    public class Transaction
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Lage et tilfeldig nummer

        public string SocialSecurityNumber { get; set; }
        public string Date { get; set; }
        public string Symbol { get; set; }
        public int Number { get; set; }
        public double TotalPrice { get; set; }

    }
}
