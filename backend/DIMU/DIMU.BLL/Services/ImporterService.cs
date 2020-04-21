using DIMU.BLL.ServiceInterfaces;
using DIMU.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DIMU.BLL.Services
{
    public class ImporterService : IImporterService
    {
        private readonly DimuContext context;

        public ImporterService(DimuContext context)
        {
            this.context = context;
        }
        public Task ImportIntezmenyekFromExcel(File excelFile)
        {
            throw new NotImplementedException();
        }
    }
}
