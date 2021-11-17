using AtaRK.Data;
using AtaRK.DTO;
using AtaRK.Models;
using AtaRK.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Controllers
{
    [ApiController]
    public class SysadminsController : ControllerBase
    {
        private readonly ServerDbContext dbContext;
        private readonly ITokenService tokenService;

        public SysadminsController(ServerDbContext dbContext, ITokenService tokenService)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }

        [Authorize]
        [Route("api/sysadmins/register")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(AuthDto authData)
        {
            try
            {
                if (await UserExists(authData.Email))
                {
                    return BadRequest("Email Is Already Taken");
                }

                HMACSHA512 hmac = new HMACSHA512();
                SystemAdmin systemAdmin = new SystemAdmin
                {
                    Email = authData.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(authData.Password)),
                    PasswordSalt = hmac.Key,
                    IsMaster = false
                };

                dbContext.SystemAdmins.Add(systemAdmin);
                await dbContext.SaveChangesAsync();

                UserDto userDto = new UserDto
                {
                    Email = systemAdmin.Email,
                    Token = tokenService.CreateToken(systemAdmin)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Route("api/sysadmins/login")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(AuthDto authData)
        {
            try
            {
                SystemAdmin systemAdmin = await dbContext.SystemAdmins
                    .SingleOrDefaultAsync(x => x.Email == authData.Email);
                if (systemAdmin == null)
                {
                    return Unauthorized("Invalid Email");
                }

                if (!PasswordService.CheckPassword(systemAdmin, authData))
                {
                    return Unauthorized("Invalid Password");
                }

                UserDto userDto = new UserDto
                {
                    Email = systemAdmin.Email,
                    Token = tokenService.CreateToken(systemAdmin)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/sysadmins")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemAdmin>>> GetAllSysadmins()
        {
            try
            {
                return await dbContext.SystemAdmins.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/sysadmins/delete/{adminId}")]
        [HttpGet]
        public async Task<ActionResult> DeleteSysadmin(int adminId)
        {
            try
            {
                SystemAdmin admin = dbContext.SystemAdmins.Find(adminId);
                if (admin == null)
                {
                    return BadRequest("Admin Not Found");
                }

                dbContext.SystemAdmins.Remove(admin);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        private async Task<bool> UserExists(string email)
        {
            return await dbContext.SystemAdmins.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
