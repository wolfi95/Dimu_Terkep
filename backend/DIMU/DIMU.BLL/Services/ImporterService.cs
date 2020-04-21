using DIMU.BLL.ServiceInterfaces;
using DIMU.DAL;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task ImportIntezmenyekFromExcel(Stream fileStream)
        {
            List<string> log = new List<string>();
            using (TextFieldParser csvParser = new TextFieldParser(fileStream))
            {
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

                using (var transaction = context.Database.BeginTransaction())
                {
                    string[] fields;
                    string Intezmenynev;
                    string Alapitas;
                    string Megszunes;
                    string Helyszin;
                    string Nyitas;
                    string Koltozes;
                    string Gps;
                    string Tipus;
                    string IntezmenyVezetok;
                    string Leiras;
                    string Video;
                    string Hivatkozasok;
                    string Social;

                    while (!csvParser.EndOfData)
                    {
                        i++;

                        fields = csvParser.ReadFields();

                        Intezmenynev = fields[0];
                        if (String.IsNullOrEmpty(Intezmenynev))
                        {
                            log.Add($"Intézménynév nem lehet üres. {i}. sor");
                        }
                        Alapitas = fields[1];
                        if (String.IsNullOrEmpty(Intezmenynev))
                        {
                            log.Add($"Alapitas nem lehet üres. {i}. sor");
                        }

                    }

                    transaction.Commit();
                }
            }
        }
    }
}
