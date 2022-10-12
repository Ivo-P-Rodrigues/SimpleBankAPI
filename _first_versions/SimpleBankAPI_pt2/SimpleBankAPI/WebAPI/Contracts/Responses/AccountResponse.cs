using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimpleBankAPI.Contracts
{
    public partial class AccountResponse 
    {
        public int AccountId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Currency { get; set; }


    }
}
