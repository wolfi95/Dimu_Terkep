using DIMU.DAL.Dto;
using DIMU.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DIMU.BLL.ServiceInterfaces
{
    public interface IIntezmenyService
    {
        public Task<IEnumerable<IntezmenyPinDto>> GetIntezmenyekAsync(IntezmenySearchParams searchParams);
        public Task<IntezmenyDetailDto> GetIntezmenyAsync(Guid id);
        public Task<bool> PutIntezmeny(Guid id, Intezmeny intezmeny);
        public Task<Intezmeny> PostIntezmeny(Intezmeny intezmeny);
        public Task<bool> DeleteIntezmeny(Guid Id);
    }
}
