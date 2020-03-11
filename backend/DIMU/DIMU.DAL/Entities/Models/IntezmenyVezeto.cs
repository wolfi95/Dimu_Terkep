using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Entities.Models
{
    public class IntezmenyVezeto
    {
        public Guid Id { get; set; }
        public String Nev { get; set; }
        public int Tol { get; set; }
        public int Ig { get; set; }        
        public Intezmeny Intezmeny { get; set; }
    }
}
