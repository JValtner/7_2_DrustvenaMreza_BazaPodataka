using _6_1_drustvena_mreza.DOMEN;

namespace _6_1_drustvena_mreza.REPO
{
    public class GrupaRepo
    {
        private const string putanjaGrupe = "DATA/grupe.csv";
        public static Dictionary<int, Grupa> grupaRepo { get; set; }

        public GrupaRepo()
        {
            if (grupaRepo == null)
            {
                Procitaj();
            }
        }
        public void Procitaj()
        {
            try
            {
                grupaRepo = new Dictionary<int, Grupa>();
                string[] sadrzaj = File.ReadAllLines(putanjaGrupe);
                foreach (string linija in sadrzaj)
                {
                    string[] delovi = linija.Split(",");
                    int kljuc = int.Parse(delovi[0]);
                    Grupa g = new Grupa(int.Parse(delovi[0]), delovi[1], DateTime.ParseExact(delovi[2], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));

                    grupaRepo[kljuc] = g;
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
                List<string> sadrzaj = new List<string>();
                foreach (KeyValuePair<int, Grupa> entryValue in grupaRepo)
                {
                    Grupa g = entryValue.Value;
                    sadrzaj.Add($"{g.Id},{g.Ime},{g.DatumOsnivanja.ToString("yyyy-MM-dd")}");
                }
                File.WriteAllLines(putanjaGrupe, sadrzaj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public Grupa NadjiGrupu(int grupaId)
        {
            foreach (Grupa grupa in GrupaRepo.grupaRepo.Values)
            {
                if (grupa.Id == grupaId)
                {
                    return grupa;
                }
            }
            return null;
        }
    }
}
