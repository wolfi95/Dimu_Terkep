using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Entities.Models
{
    public enum IntezmenyTipus
    {
        //Állami intézmény
        Allami = 0,
        //Önkormányzat által fenntartott intézmény
        Onkormanyzati = 1,
        //Kereskedelmi galéria
        KereskedelmiGaleria = 2,
        //Non profit intézmény
        NonProfit = 3,
        //Kulturális intézet, egyesület
        Kulturalis = 4,
        //Oktatási intézmény
        Oktatasi = 5
    }
}
