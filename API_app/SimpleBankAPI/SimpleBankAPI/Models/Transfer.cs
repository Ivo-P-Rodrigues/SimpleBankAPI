using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankAPI.Models
{
    public partial class Transfer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransferId { get; set; }
        public decimal Amount { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
