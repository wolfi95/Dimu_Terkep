using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Entities.Models
{
    public class Muvesz
    {
        public Guid Id { get; set; }
        public String Nev { get; set; }
        public String Url { get; set; }
        public Intezmeny Intezmeny { get; set; }
    }
}
