using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankAPI.Models
{
    public partial class Movement
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovementId { get; set; }
        [ForeignKey("AccountId")]
        public int AccountId { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Account Account { get; set; }
    }
}
