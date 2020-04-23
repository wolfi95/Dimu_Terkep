using DIMU.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Dto
{
    public class IntezmenySearchParams
    {
        public string IntezmenyNev { get; set; }
        public string IntezmenyCim { get; set; }
        public string IntezmenyVezeto { get; set; }
        public int? MukodesTol { get; set; }
        public int? MukodesIg { get; set; }
        public IntezmenyTipus?[] IntezmenyTipus { get; set; }
    }
}
