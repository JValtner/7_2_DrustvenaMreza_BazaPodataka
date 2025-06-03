using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
namespace _6_1_drustvena_mreza.DOMEN
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        
        public DateTime DatumRodjenja { get; set; }
        public List<Grupa>? GrupeKorisnika { get; set; } = new List<Grupa>();

        public Korisnik (int id, string korisnickoIme, string ime, string prezime, DateTime datumRodjenja)
        {
            this.Id = id;
            this.KorisnickoIme = korisnickoIme;
            this.Ime = ime;
            this.Prezime = prezime;
            this.DatumRodjenja = datumRodjenja;
        }
    }
}
