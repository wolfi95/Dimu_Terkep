using DIMU.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Dto
{
    public class IntezmenyHelyszinDto
    {
        public string Helyszin { get; set; }
        public int Nyitas { get; set; }
        public int? Koltozes { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
