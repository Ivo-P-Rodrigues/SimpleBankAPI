using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimpleBankAPI.Models
{
    public partial class Account 
    {
        public Account()
        {
            Movements = new HashSet<Movement>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Currency { get; set; }

        
        public virtual User User { get; set; }
        
        public virtual ICollection<Movement> Movements { get; set; }
    }
}
