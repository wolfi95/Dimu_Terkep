using DIMU.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Dto
{
    public class IntezmenyDetailDto
    {
        public String Nev { get; set; }
        public int Alapitas { get; set; }
        public int? Megszunes { get; set; }
        public List<string> IntezmenyHelyszinek { get; set; }
        public List<string> IntezmenyVezetok { get; set; }
        public String Leiras { get; set; }
        public List<string> Esemenyek { get; set; }
        public String Fotok { get; set; }
        public String Videok { get; set; }
        public String Link { get; set; }
        public String Social { get; set; }
    }
}
