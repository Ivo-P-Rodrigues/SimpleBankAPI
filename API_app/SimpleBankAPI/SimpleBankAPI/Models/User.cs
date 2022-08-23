using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankAPI.Models
{
    public partial class User
    {
        public User()
        {
            Accounts = new HashSet<Account>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; } 
        public string Username { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? PasswordChangedAt { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
