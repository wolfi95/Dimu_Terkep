using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIMU.Web.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet]
        [Route("hello")]
        public async Task<IActionResult> Hello()
        {
            return Ok("Hello");
        }
    }
}
