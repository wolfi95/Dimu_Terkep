using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DIMU.BLL.ServiceInterfaces
{
    public interface IImporterService
    {
        public Task ImportIntezmenyekFromExcel(File excelFile);
    }
}
