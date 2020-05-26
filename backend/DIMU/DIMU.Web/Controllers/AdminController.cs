using DIMU.BLL.ServiceInterfaces;
using DIMU.DAL.Dto;
using DIMU.DAL.Entities.Authentication;
using DIMU.Web.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DIMU.Web.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IImporterService _importerService;
        private readonly SignInManager<AdminUser> _signInManager;
        private readonly UserManager<AdminUser> _userManager;
        private readonly IConfiguration _configuration;

        public AdminController(IAdminService adminService, IImporterService importerService, SignInManager<AdminUser> signInManager, UserManager<AdminUser> userManager, IConfiguration configuration)
        {
            _adminService = adminService;
            _importerService = importerService;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("hello")]
        public async Task<IActionResult> Hello()
        {
            var result = await _adminService.Hello();
            return Ok(result);
        }

        [HttpPost]
        [Route("import/intezmenyek")]
        public async Task<ActionResult<List<String>>> ImportFromExcel()
        {
            Microsoft.AspNetCore.Http.IFormFile file=null;
            try
            {
                file = Request.Form.Files.FirstOrDefault();
            }
            catch
            {

            }
            List<string> log = new List<string>();
            try
            {
                log = await _importerService.ImportIntezmenyekFromExcel(file?.OpenReadStream());
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }

            return Ok(log);
        }

        [HttpPost]
        [Route("import/esemenyek")]
        public async Task<ActionResult<List<String>>> ImportEsemenyFromExcel()
        {
            Microsoft.AspNetCore.Http.IFormFile file = null;
            try
            {
                file = Request.Form.Files.FirstOrDefault();
            }
            catch
            {

            }
            List<string> log;
            try
            {
                log = await _importerService.ImportEsemenyekFromExcel(file?.OpenReadStream());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(log);
        }

        [HttpDelete]
        [Route("purgeDatabase")]
        public async Task<IActionResult> PurgeDatabase()
        {
            try
            {
                await _importerService.PurgeDatabase();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginData)
        {
            if(loginData.UserName == null || loginData.Password == null)
            {
                return Unauthorized("Felhasználónév és jelszó mező kitöltése kötelező");
            }
            var user = await _userManager.FindByNameAsync(loginData.UserName);
            if(user == null)
            {
                return Unauthorized("Felhasználónév nem található");
            }

            var result = await _signInManager.PasswordSignInAsync(loginData.UserName, loginData.Password, false,false);
            if (result.Succeeded)
            {
                var res = AuthenticationHelper.GenerateJwtToken(user,_configuration);
                return Ok(res);
            }
            else
            {
                return Unauthorized("Hibás jelszó");
            }
        }
    }
}
