using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.database.entities
{
    public class user
    {
        public int id { get; set; }
        public string wallet_address { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone_number { get; set; }
        public string? invite_code { get; set; }
        public string? invited_by { get; set; }
        public int? status { get; set; }
        public string? confirmation_code_number{ get; set; }
        public string? confirmation_code_alphanumber { get; set; }
        public bool? confirmed { get; set; }
        public int? invitations_available { get; set; }
        public DateTime? expiration_date_invitations { get; set; }

        public tokenbounty tokenbounty { get; set; }
    }
}