using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.database.entities
{
    [Table("tokenpool")]
    public class TokenPool
    {
        public int id { get; set; }
        public decimal total { get; set; }
        public decimal divisor { get; set; }
        public int tick_speed { get; set; }
    }
}
