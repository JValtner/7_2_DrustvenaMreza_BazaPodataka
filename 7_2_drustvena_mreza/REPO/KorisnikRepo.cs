using System.Collections.Generic;
using System;
using _6_1_drustvena_mreza.DOMEN;

namespace _6_1_drustvena_mreza.REPO
{
    public class KorisnikRepo
    {
        private const string putanjaKorisnik = "DATA/korisnici.csv";
        private const string putanjaClanstva = "DATA/clanstva.csv";
        public static Dictionary<int, Korisnik> korisnikRepo { get; set; } 

        public KorisnikRepo()
        {
            if (korisnikRepo == null)
            {
                new GrupaRepo();
                Procitaj();
            }
        }
        public void Procitaj()
        {
            try
            {
                //Procitaj grupe

                //Procitaj Korisnike
                korisnikRepo = new Dictionary<int, Korisnik>();
                string[] sadrzaj = File.ReadAllLines(putanjaKorisnik);
                string[] veze = File.ReadAllLines(putanjaClanstva);
                foreach (string linija in sadrzaj)
                {
                    string[] delovi = linija.Split(",");
                    int kljuc = int.Parse(delovi[0]);
                    List<Grupa> grupeKorisnika = new List<Grupa>();
                    Korisnik k = new Korisnik(int.Parse(delovi[0]), delovi[1], delovi[2], delovi[3], DateTime.ParseExact(delovi[4], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                    foreach (string veza in veze)
                    //Dodeli grupe korisnicima
                    {
                        string[] deloviVeze = veza.Split(",");
                        int idKorisnik = int.Parse(deloviVeze[0]);
                        int idGrupe = int.Parse(deloviVeze[1]);
                        if (k.Id.Equals(idKorisnik))
                        {
                            Grupa g = null;
                            foreach (Grupa grupa in GrupaRepo.grupaRepo.Values)
                            {
                                if (grupa.Id == idGrupe)
                                {
                                    g = grupa;
                                }
                            }
                            if (g != null) {k.GrupeKorisnika.Add(g);}
                            
                        }
                    }

                    korisnikRepo[kljuc] = k;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Sacuvaj()
        {
            try
            {
                List<string> sadrzajKorisnici = new List<string>();
                foreach (Korisnik k in korisnikRepo.Values)
                {
                    sadrzajKorisnici.Add($"{k.Id},{k.KorisnickoIme},{k.Ime},{k.Prezime},{k.DatumRodjenja:yyyy-MM-dd}");
                }
                File.WriteAllLines(putanjaKorisnik, sadrzajKorisnici);

                // Save clanstva.csv
                List<string> sadrzajClanstva = new List<string>();
                foreach (Korisnik k in korisnikRepo.Values)
                {
                    if (k?.GrupeKorisnika == null)
                    {
                    continue;
                    }
                        
                    foreach (Grupa g in k.GrupeKorisnika)
                    {
                        if (g != null && k.Id != null && g.Id != null)
                        {
                            sadrzajClanstva.Add($"{k.Id},{g.Id}");
                        }
                        
                    }
                }
                File.WriteAllLines(putanjaClanstva, sadrzajClanstva);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Korisnik NadjiKorisnika(int korisnikId, int grupaId)
        {

            foreach (Korisnik korisnik in KorisnikRepo.korisnikRepo.Values)
            {
                if (korisnik.Id == korisnikId)
                {
                    foreach (Grupa grupa in korisnik.GrupeKorisnika)
                    {
                        if (grupa.Id == grupaId)
                        {

                            return korisnik;
                        }
                    }
                }
            }
            return null;
        }
        public Korisnik NadjiKorisnikaId(int korisnikId)
        {
            foreach (Korisnik korisnik in KorisnikRepo.korisnikRepo.Values)
            {
                if (korisnik.Id == korisnikId)
                {
                    return korisnik;
                }
            }
            return null;
        }
        public List<Korisnik> NadjiKorisnike(int grupaId)
        {
            Console.WriteLine(grupaId);
            List<Korisnik> listaKorisnikaGrupe = new List<Korisnik>();
            foreach (Korisnik korisnik in KorisnikRepo.korisnikRepo.Values)
            {
                if (korisnik.GrupeKorisnika.Any(g => g != null && g.Id == grupaId))
                {
                    listaKorisnikaGrupe.Add(korisnik);
                }
            }
            return listaKorisnikaGrupe;
        }
    }
}
