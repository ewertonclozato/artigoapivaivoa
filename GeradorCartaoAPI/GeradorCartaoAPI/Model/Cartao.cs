using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GeradorCartaoAPI.Model
{
    public class Cartao
    {
        //Indica que será chave primária no banco de dados.
        [Key]
        //Gerando números aleatórios para o ID
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Bandeira { get; set; }
        public DateTime? DataCriacao { get; set; }
        [JsonIgnore]

        //Indica que existe uma referência entre Cartao e Cliente
        public Cliente Cliente { get; set; }
    }
}
