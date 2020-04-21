using System.IO;
using System.Threading.Tasks;

namespace DIMU.BLL.ServiceInterfaces
{
    public interface IImporterService
    {
        public Task ImportIntezmenyekFromExcel(Stream excelFile);
    }
}
