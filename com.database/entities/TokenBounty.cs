using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.database.entities
{
    public class TokenBounty
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("User")]
        public int id_usuario { get; set; }

        public decimal valor_reserva_total { get; set; }
        public decimal valor_reservado { get; set; }

        public User User { get; set; }
    }
}
