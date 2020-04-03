using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Entities.Models
{
    public class IntezmenyHelyszin
    {
        public Guid Id { get; set; }
        public string Helyszin { get; set; }
        public int Nyitas { get; set; }
        public int? Koltozes { get; set; }
        public Guid IntezmenyId { get; set; }
        public Intezmeny Intezmeny { get; set; }
    }
}
