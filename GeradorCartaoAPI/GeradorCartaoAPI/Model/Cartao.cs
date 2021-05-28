using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorCartaoAPI.Model
{
    public class Cartao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Bandeira { get; set; }
        public Cliente Cliente { get; set; }

    }
}
