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
    public class ShopadminsController : ControllerBase
    {
        private readonly ServerDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public ShopadminsController(ServerDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [Authorize]
        [Route("api/shopadmins/register")]
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
                ShopAdmin shopAdmin = new ShopAdmin
                {
                    Email = authData.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(authData.Password)),
                    PasswordSalt = hmac.Key
                };

                _dbContext.ShopAdmins.Add(shopAdmin);
                await _dbContext.SaveChangesAsync();

                UserDto userDto = new UserDto
                {
                    Email = shopAdmin.Email,
                    Token = _tokenService.CreateToken(shopAdmin)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Route("api/shopadmins/login")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(AuthDto authData)
        {
            try
            {
                ShopAdmin shopAdmin = await _dbContext.ShopAdmins
                    .SingleOrDefaultAsync(x => x.Email == authData.Email);
                if (shopAdmin == null)
                {
                    return Unauthorized("Invalid Email");
                }

                if (!PasswordService.CheckPassword(shopAdmin, authData))
                {
                    return Unauthorized("Invalid Password");
                }

                UserDto userDto = new UserDto
                {
                    Id = shopAdmin.Id,
                    Email = shopAdmin.Email,
                    Token = _tokenService.CreateToken(shopAdmin)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shopadmins/change_authdata")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> ChangeShopadminAuthData(AuthDto authData)
        {
            try
            {
                ShopAdmin dbShopAdmin = _dbContext.ShopAdmins.
                    SingleOrDefault(x => x.Email == authData.Email);
                if (dbShopAdmin == null)
                {
                    return BadRequest("Shop Admin Not Found");
                }

                HMACSHA512 hmac = new HMACSHA512();

                dbShopAdmin.Email = authData.Email;
                dbShopAdmin.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(authData.Password));
                dbShopAdmin.PasswordSalt = hmac.Key;
                await _dbContext.SaveChangesAsync();

                UserDto userDto = new UserDto
                {
                    Email = dbShopAdmin.Email,
                    Token = _tokenService.CreateToken(dbShopAdmin)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shopadmins")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopAdmin>>> GetAllShopAdmins()
        {
            try
            {
                return await _dbContext.ShopAdmins
                    .Include(x => x.FranchiseShops)
                    .Include(x => x.TechMessages)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shopadmins/{adminEmail}")]
        [HttpGet]
        public async Task<ActionResult<ShopAdmin>> GetShopAdmin(string adminEmail)
        {
            try
            {
                return await _dbContext.ShopAdmins
                    .Include(x => x.FranchiseShops)
                    .Include(x => x.TechMessages)
                    .SingleOrDefaultAsync(x => x.Email == adminEmail);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shopadmins/delete/{adminEmail}")]
        [HttpGet]
        public async Task<ActionResult> DeleteShopAdmin(string adminEmail)
        {
            try
            {
                ShopAdmin admin = _dbContext.ShopAdmins
                    .SingleOrDefault(x => x.Email == adminEmail);
                if (admin == null)
                {
                    return BadRequest("ShopAdmin Not Found");
                }

                _dbContext.ShopAdmins.Remove(admin);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        private async Task<bool> UserExists(string email)
        {
            return await _dbContext.ShopAdmins.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
