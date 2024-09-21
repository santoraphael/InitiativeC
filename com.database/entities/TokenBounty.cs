using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.database.entities
{
    public class tokenbounty
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("user")]
        public int id_usuario { get; set; }

        public decimal valor_reserva_total { get; set; }
        public decimal valor_reservado { get; set; }

        public user user { get; set; }
    }
}
