using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankAPI.Core.Models
{
    public partial class User : Entity
    {
        public User()
        {
            Accounts = new HashSet<Account>();
            Sessions = new HashSet<Session>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public byte[] Password { get; set; } 
        public byte[] Salt { get; set; } 
        //public string Password { get; set; } 
        public string Username { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? PasswordChangedAt { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }

    }
}
