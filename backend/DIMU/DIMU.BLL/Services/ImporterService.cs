using DIMU.BLL.ServiceInterfaces;
using DIMU.DAL;
using DIMU.DAL.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DIMU.BLL.Services
{
    public class ImporterService : IImporterService
    {
        private readonly DimuContext context;

        public ImporterService(DimuContext context)
        {
            this.context = context;
        }
        public async Task<List<string>> ImportIntezmenyekFromExcel(Stream fileStream)
        {
            List<string> log = new List<string>();
            using (TextFieldParser csvParser = new TextFieldParser("F:\\Downloads\\Mintaadatbázis50rekordos.csv", Encoding.UTF8))
            {
                csvParser.SetDelimiters(new string[] { "," });
                //tényleges adatig üres olvasás
                var firstfield = csvParser.ReadFields().First();
                int i = 1;
                while (firstfield != "Intézmény")
                {
                    i++;
                    firstfield = csvParser.ReadFields().First();
                    if (i >= 30)
                        throw new ArgumentException("A fájl nem megfelelően lett tagolva (fejléc hiányzik)");
                }

                string[] fields;
                string Intezmenynev;
                int Alapitas;
                int Megszunes = 0;
                string Helyszin;
                int Nyitas;
                int Koltozes = 0;
                string Gps;
                string Tipus;
                string IntezmenyVezetok;
                string Leiras;
                string Kepek;
                string Video;
                string Hivatkozasok;
                string Social;

                while (!csvParser.EndOfData)
                {
                    i++;
                    
                    Intezmenynev = "";
                    Alapitas = 0;
                    Megszunes = 0;
                    Helyszin = "";
                    Nyitas = 0;
                    Koltozes = 0;
                    Gps = "";
                    Tipus = "";
                    IntezmenyVezetok = "";
                    Leiras = "";
                    Kepek = "";
                    Video = "";
                    Hivatkozasok = "";
                    Social = "";

                    fields = csvParser.ReadFields();

                    Intezmenynev = fields[0];
                    if (String.IsNullOrEmpty(Intezmenynev))
                    {
                        log.Add($"Intézménynév nem lehet üres. {i}. sor");
                        continue;
                    }
                    if (!int.TryParse(fields[1], out Alapitas))
                    {
                        log.Add($"Alapitás nem szám( {fields[1]} ). {i}. sor");
                        continue;
                    }
                    if (!String.IsNullOrEmpty(fields[2]) && !int.TryParse(fields[2], out Megszunes))
                    {
                        log.Add($"Megszunes nem szám( {fields[2]} ). {i}. sor");
                        continue;
                    }
                    Helyszin = fields[3];
                    if (String.IsNullOrEmpty(Helyszin))
                    {
                        log.Add($"Helyszín nem lehet üres. {i}. sor");
                        continue;
                    }
                    if (!int.TryParse(fields[4], out Nyitas))
                    {
                        log.Add($"Nyitas nem szám( {fields[4]} ). {i}. sor");
                        continue;
                    }
                    if (!String.IsNullOrEmpty(fields[5]) && !int.TryParse(fields[5], out Koltozes))
                    {
                        log.Add($"Költözés nem szám( {fields[5]} ). {i}. sor");
                        continue;
                    }
                    Gps = fields[6];
                    if (String.IsNullOrEmpty(Gps))
                    {
                        log.Add($"Gps nem lehet üres. {i}. sor");
                        continue;
                    }
                    string[] gpsSplit = Gps.Split(",");
                    if (gpsSplit.Length != 2)
                    {
                        log.Add($"Rossz Gps formátum. {i}. sor");
                        continue;
                    }
                    double latitude;
                    double longitude;
                    try
                    {
                        double.TryParse(gpsSplit[0], out latitude);
                    }
                    catch
                    {
                        log.Add($"Latitude nem szám {gpsSplit[0]}. {i}. sor");
                        continue;
                    }
                    try
                    {
                        double.TryParse(gpsSplit[1], out longitude);
                    }
                    catch
                    {
                        log.Add($"Latitude nem szám {gpsSplit[1]}. {i}. sor");
                        continue;
                    }
                    Tipus = fields[7];
                    if (String.IsNullOrEmpty(Tipus))
                    {
                        log.Add($"Tipus nem lehet üres. {i}. sor");
                        continue;
                    }
                    if (GetIntezmenyTipusFromString(Tipus) == null)
                    {
                        log.Add($"Tipust nem sikerult beolvasni( {Tipus} ). {i}. sor");
                        continue;
                    }
                    IntezmenyVezetok = fields[8];
                    Leiras = fields[9];
                    Kepek = fields[10];
                    Video = fields[11];
                    Hivatkozasok = fields[12];
                    Social = fields[13];

                    var intezmeny = await context.Intezmenyek
                        .Include(intezmeny => intezmeny.IntezmenyVezetok)
                        .Where(intezmeny => intezmeny.Nev.Equals(Intezmenynev))
                        .FirstOrDefaultAsync();

                    var intezmenyVezetok = new List<IntezmenyVezeto>();
                    if (!String.IsNullOrEmpty(IntezmenyVezetok))
                        foreach (var intezmenyvezSor in IntezmenyVezetok.Split('\n'))
                        {
                            var intezmenyvezSplit = intezmenyvezSor.Split(";");
                            string[] intezmenyvezTolIg;
                            try
                            {
                                intezmenyvezTolIg = intezmenyvezSplit[1].Split("-");
                            }
                            catch
                            {
                                log.Add($"{intezmenyvezSor} Intézmény vezető formátum hiba.  {i}. sor");
                                continue;
                            }
                            int tol = 0;
                            try
                            {
                                tol = int.Parse(intezmenyvezTolIg[0]);
                            }
                            catch
                            {
                                log.Add($"{intezmenyvezSplit[1]} Intézmény vezető év dátum formátum hiba. {intezmenyvezSplit[0]} kihagyva.  {i}. sor");
                                continue;
                            }
                            int ig = 0;
                            if(intezmenyvezTolIg.Length != 2)
                            {
                                log.Add($"{intezmenyvezSplit[1]} Intézmény vezető év dátum formátum hiba. {intezmenyvezSplit[0]} kihagyva.  {i}. sor");
                                continue;
                            }
                            if (!String.IsNullOrEmpty(intezmenyvezTolIg[1]))
                            {
                                try
                                {
                                    ig = int.Parse(intezmenyvezTolIg[1]);
                                }
                                catch
                                {
                                    log.Add($"{intezmenyvezTolIg[1]} Intézmény vezető év dátum formátum hiba. {intezmenyvezSplit[0]} kihagyva.  {i}. sor");
                                    continue;
                                }
                            }

                            intezmenyVezetok.Add(new IntezmenyVezeto
                            {
                                Nev = intezmenyvezSplit[0],
                                Tol = tol,
                                Ig = ig == 0 ? null : (int?)ig,
                            });
                        }

                    if (intezmeny == null)
                    {
                        intezmeny = new Intezmeny
                        {
                            Nev = Intezmenynev,
                            Alapitas = Alapitas,
                            Megszunes = Megszunes == 0 ? null : (int?)Megszunes,
                            Leiras = Leiras,
                            Social = Social,
                            Tipus = (IntezmenyTipus)GetIntezmenyTipusFromString(Tipus),
                            Videok = Video,
                            Link = Hivatkozasok,
                            IntezmenyHelyszinek = new List<IntezmenyHelyszin>(),
                            IntezmenyVezetok = new List<IntezmenyVezeto>(),
                        };
                        context.Intezmenyek.Add(intezmeny);
                    }

                    foreach (var intezmenyvezeto in intezmenyVezetok)
                    {
                        if (intezmeny.IntezmenyVezetok.Where(iv => iv.Nev.Equals(intezmenyvezeto.Nev) && iv.Tol == intezmenyvezeto.Tol).FirstOrDefault() == null)
                        {
                            intezmenyvezeto.Intezmeny = intezmeny;
                            context.IntezmenyVezetok.Add(intezmenyvezeto);
                        }
                    }

                    var intezemenyHelyszin = context.IntezmenyHelyszinek.Where(ih => ih.Helyszin.Equals(Helyszin) && ih.Nyitas == Nyitas).FirstOrDefault();
                    if (intezemenyHelyszin == null)
                    {
                        intezemenyHelyszin = new IntezmenyHelyszin
                        {
                            Helyszin = Helyszin,
                            Intezmeny = intezmeny,
                            Nyitas = Nyitas,
                            Koltozes = Koltozes == 0 ? null : (int?)Koltozes,
                            Latitude = latitude,
                            Longitude = longitude,
                        };
                        context.IntezmenyHelyszinek.Add(intezemenyHelyszin);
                    }

                    await context.SaveChangesAsync();
                }
            }
            return log;
        }

        public async Task<List<string>> ImportEsemenyekFromExcel(Stream excelFile)
        {
            List<string> log = new List<string>();
            using (TextFieldParser csvParser = new TextFieldParser("F:\\Downloads\\kiallitasok.csv", Encoding.UTF8))
            {
                csvParser.SetDelimiters(new string[] { "," });
                //tényleges adatig üres olvasás
                var firstfield = csvParser.ReadFields().First();
                int i = 1;
                while (firstfield != "Intézmény")
                {
                    i++;
                    firstfield = csvParser.ReadFields().First();
                    if (i >= 30)
                        throw new ArgumentException("A fájl nem megfelelően lett tagolva (fejléc hiányzik)");
                }

                string[] fields;
                while (!csvParser.EndOfData)
                {
                    i++;

                    fields = csvParser.ReadFields();

                    string intezmenyNev = fields[0];
                    var intezmeny = context.Intezmenyek.Include(intezmeny => intezmeny.Esemenyek).Where(intezmeny => intezmeny.Nev == intezmenyNev).FirstOrDefault();
                    if (intezmeny == null)
                    {
                        log.Add($"Nem létező intézmény. {intezmenyNev} {i}. sor.");
                        continue;
                    }

                    string[] esemenyek = fields[1].Split('\n');

                    foreach(var esemeny in esemenyek)
                    {
                        string esemenyNev;
                        string datum;
                        string szervezo = "";
                        var esemenySplit = esemeny.Split(";");
                        try
                        {
                            datum = esemenySplit[0];
                            esemenyNev = esemenySplit[1];
                        }
                        catch
                        {
                            log.Add($"Esemény formátum nem megfelelő ( {esemeny} ). {i}. sor.");
                            continue;
                        }
                        try
                        {
                            szervezo = esemenySplit[2];
                        }
                        catch 
                        { 
                            //szervezo nem kotelezo
                        }
                        if(!intezmeny.Esemenyek.Any(letezoEsemeny => letezoEsemeny.Nev == esemenyNev))
                        {
                            intezmeny.Esemenyek.Add(new Esemeny
                            {
                                Datum = datum,
                                Nev = esemenyNev,
                                Szervezo = szervezo
                            });
                        }
                    }
                    await context.SaveChangesAsync();
                }
                return log;
            }
        }

        public async Task PurgeDatabase()
        {
            var esemenyek = context.Esemenyek;
            context.Esemenyek.RemoveRange(esemenyek);
            await context.SaveChangesAsync();

            var intezmenyVezetok = context.IntezmenyVezetok;
            context.IntezmenyVezetok.RemoveRange(intezmenyVezetok);
            await context.SaveChangesAsync();

            var intezmenyHelyzinek = context.IntezmenyHelyszinek;
            context.IntezmenyHelyszinek.RemoveRange(intezmenyHelyzinek);
            await context.SaveChangesAsync();

            var intezmenyek = context.Intezmenyek;
            context.Intezmenyek.RemoveRange(intezmenyek);
            await context.SaveChangesAsync();
        }

        public IntezmenyTipus? GetIntezmenyTipusFromString(string tipus)
        {
            IntezmenyTipus? intezmenyTipus = null;
            switch (tipus)
            {
                case "Állami múzeum":
                    intezmenyTipus = IntezmenyTipus.AllamiMuzeum;
                    break;
                case "Állami kulturális központ":
                    intezmenyTipus = IntezmenyTipus.AllamiKulturalis;
                    break;
                case "Önkormányzati múzeum":
                    intezmenyTipus = IntezmenyTipus.OnkormanyzatiMuzeum;
                    break;
                case "Önkormányzati kulturális központ":
                    intezmenyTipus = IntezmenyTipus.OnkormanyzatiKulturalis;
                    break;
                case "Önkormányzati galéria":
                    intezmenyTipus = IntezmenyTipus.OnkormanyzatiGaleria;
                    break;
                case "Kereskedelmi galéria":
                    intezmenyTipus = IntezmenyTipus.KereskedelmiGaleria;
                    break;
                case "Független kulturális intézmény":
                    intezmenyTipus = IntezmenyTipus.FuggetlenKulturalisIntezmeny;
                    break;
                case "Non-profit galéria":
                    intezmenyTipus = IntezmenyTipus.NonProfitGaleria;
                    break;
                case "Kulturális intézet":
                    intezmenyTipus = IntezmenyTipus.KulturalisIntezet;
                    break;
                case "Egyesület":
                    intezmenyTipus = IntezmenyTipus.Egyesulet;
                    break;
                case "Oktatási intézmény":
                    intezmenyTipus = IntezmenyTipus.Oktatasi;
                    break;
                case "Étterem, kocsma galéria":
                    intezmenyTipus = IntezmenyTipus.EtteremKocsmaGaleria;
                    break;
            }
            return intezmenyTipus;
        }
    }
}
