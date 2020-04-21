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
    [Route("admin")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly SignInManager<AdminUser> _signInManager;
        private readonly UserManager<AdminUser> _userManager;
        private readonly IConfiguration _configuration;

        public AdminController(IAdminService adminService, SignInManager<AdminUser> signInManager, UserManager<AdminUser> userManager, IConfiguration configuration)
        {
            _adminService = adminService;
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
        [Route("importdata")]
        public async Task<IActionResult> ImportFromExcel()
        {
            Request.HttpContext.WebSockets.;

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
