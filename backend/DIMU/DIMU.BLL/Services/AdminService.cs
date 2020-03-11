using DIMU.BLL.ServiceInterfaces;
using DIMU.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DIMU.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly DimuContext context;

        public async Task<string> Hello()
        {
            return "Hello";
        }
    }
}
