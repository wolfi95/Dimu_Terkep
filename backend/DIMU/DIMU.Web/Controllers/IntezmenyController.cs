using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DIMU.DAL.Entities.Models;
using DIMU.BLL.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using DIMU.DAL.Dto;

namespace DIMU.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntezmenyController : ControllerBase
    {        
        private readonly IIntezmenyService intezmenyService;

        public IntezmenyController(IIntezmenyService intezmenyService)
        {            
            this.intezmenyService = intezmenyService;
        }

        // POST: api/Intezmeny
        [HttpPost]
        public async Task<IEnumerable<IntezmenyPinDto>> GetIntezmenyek(IntezmenySearchParams searchParams)
        {
            return await intezmenyService.GetIntezmenyekAsync(searchParams);
        }

        // GET: api/Intezmeny/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IntezmenyDetailDto>> GetIntezmenyDetail(Guid id)
        {
            var intezmeny = await intezmenyService.GetIntezmenyAsync(id);

            if (intezmeny == null)
            {
                return NotFound();
            }

            return Ok(intezmeny);
        }

        [Authorize]
        [HttpPost("intezmenyHeaders")]
        public async Task<ActionResult<IntezmenyDetailDto>> GetIntezmenyHeaders(string searchParam)
        {
            var intezmenyek = await intezmenyService.GetIntezmenyHeadersAsync(searchParam);           

            return Ok(intezmenyek);
        }        

        // PUT: api/Intezmeny/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutIntezmeny(Guid id,[FromBody] Intezmeny intezmeny)
        {
            bool result;
            try
            {
               result = await intezmenyService.PutIntezmeny(id, intezmeny);
            }
            catch(Exception e)
            {
                throw e;
            }

            if(result)
            {
                return Ok();
            }else
            {
                return NotFound();
            }
        }

        // POST: api/Intezmeny
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("new")]
        [Authorize]
        public async Task<ActionResult<Intezmeny>> PostIntezmeny([FromBody]Intezmeny intezmeny)
        {
            if (String.IsNullOrEmpty(intezmeny.Nev)){
                return BadRequest("Intézmény név nem lehet null");
            }            
            intezmeny = await intezmenyService.PostIntezmeny(intezmeny);
            return CreatedAtAction("GetIntezmeny", new { id = intezmeny.Id }, intezmeny);
        }

        // DELETE: api/Intezmeny/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteIntezmeny(Guid id)
        {
            var result = await intezmenyService.DeleteIntezmeny(id);
            if (!result)
            {
                return NotFound();
            }            

            return Ok();
        }
    }
}
