using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Entities.Models
{
    public class Esemeny
    {
        public Guid Id { get; set; }
        public string Nev { get; set; }
        public string Datum { get; set; }
        public string Szervezo { get; set; }
        public Guid IntezmenyId { get; set; }
        public Intezmeny Intezmeny { get; set; }
    }
}
