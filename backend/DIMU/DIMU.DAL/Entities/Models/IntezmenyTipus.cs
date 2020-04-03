using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL.Entities.Models
{
    public enum IntezmenyTipus
    {
        //Állami Múzeum
        AllamiMuzeum = 0,
        //Állami kulturális központ
        AllamiKulturalis = 1,
        //Önkormányzat múzeum
        OnkormanyzatiMuzeum = 2,
        //Önkormányzati kulturális központ
        OnkormanyzatiKulturalis = 3,
        //Önkormányzati galéria
        OnkormanyzatiGaleria = 4,
        //Kereskedelmi galéria
        KereskedelmiGaleria = 5,
        //Független kulturálisintézmény
        FuggetlenKulturalisIntezmeny = 6,
        //Non profit galéria
        NonProfitGaleria = 7,
        //Kulturális intézet
        KulturalisIntezet = 8,
        //Egyesület
        Egyesulet = 9,
        //Oktatási intézmény
        Oktatasi = 10,
        //Étterem, kocsma galéria
        EtteremKocsmaGaleria = 11,
    }
}
