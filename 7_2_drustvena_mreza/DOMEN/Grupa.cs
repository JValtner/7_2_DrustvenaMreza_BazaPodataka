using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
namespace _7_2_drustvena_mreza.DOMEN
{
    public class Grupa
            {
        
        public int Id { get; set; }
        public string Ime { get; set; }
        public DateTime DatumOsnivanja { get; set; }

        public Grupa(int id, string ime, DateTime datumOsnivanja)
        {
            this.Id = id;
            this.Ime = ime;
            this.DatumOsnivanja = datumOsnivanja;
        }
    }
}
