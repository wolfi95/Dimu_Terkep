using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Dto
{
    public class IntezmenyHeader
    {
        public String Nev { get; set; }
        public Guid IntezmenyId { get; set; }
        public int Alapitas { get; set; }
        public int? Megszunes { get; set; }
    }
}
