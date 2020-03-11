using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Entities.Models
{
    public class Intezmeny
    {
        public Guid Id { get; set; }
        public String Nev { get; set; }
        public int Alapitas { get; set; }
        public int Megszunes { get; set; }
        public String Helyszin { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IntezmenyTipus Tipus { get; set; }
        public List<IntezmenyVezeto> IntezmenyVezetok { get; set; }
        public String Leiras { get; set; }
        public String Esemenyek { get; set; }
        public String Fotok { get; set; }
        public String Videok { get; set; }
        public String Link { get; set; }
        public String Social { get; set; }
        public String Megjegyzes { get; set; }
        public List<Muvesz> Muveszek { get; set; }
    }
}
