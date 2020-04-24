using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DIMU.BLL.ServiceInterfaces
{
    public interface IImporterService
    {
        public Task<List<string>> ImportIntezmenyekFromExcel(Stream excelFile);
        public Task<List<string>> ImportEsemenyekFromExcel(Stream excelFile);
        public Task PurgeDatabase();
    }
}
