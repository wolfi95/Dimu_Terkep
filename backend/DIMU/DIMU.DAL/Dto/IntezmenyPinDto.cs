using DIMU.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Dto
{
    public class IntezmenyPinDto
    {
        public Guid IntezmenyId { get; set; }
        public IntezmenyTipus IntezmenyTipus { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
