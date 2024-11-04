using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.database.entities
{
    [Index(nameof(stake_address), IsUnique = true)]
    [Table("users")]
    public class User
    {
        [Key]
        public int id { get; set; }
        public string stake_address { get; set; }
        public string wallet_address { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone_number { get; set; }
        public string? invite_code { get; set; }
        public string? invited_by { get; set; }
        public string? confirmation_code_number{ get; set; }
        public string? confirmation_code_alphanumber { get; set; }
        public bool? confirmed { get; set; }
        public int? invitations_available { get; set; }
        public DateTime? expiration_date_invitations { get; set; }
        public string currentCulture { get; set; }

        public TokenBounty TokenBounty { get; set; }
    }
}