using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _6_1_drustvena_mreza.DOMEN
{
    public class Grupa
            {
        
        public int Id { get; set; }
        [Required] 
        public string Ime { get; set; }
        [Required]
        public DateTime DatumOsnivanja { get; set; }

        public Grupa(int id, string ime, DateTime datumOsnivanja)
        {
            this.Id = id;
            this.Ime = ime;
            this.DatumOsnivanja = datumOsnivanja;
        }
    }
}
