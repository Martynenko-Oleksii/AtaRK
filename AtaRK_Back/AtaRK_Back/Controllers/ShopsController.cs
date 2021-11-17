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
    public class ShopsController : ControllerBase
    {
        private readonly ServerDbContext dbContext;
        private readonly ITokenService tokenService;

        public ShopsController(ServerDbContext dbContext, ITokenService tokenService)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }

        [Authorize]
        [Route("api/shops/register")]
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
                FranchiseShop shop = new FranchiseShop
                {
                    Email = authData.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(authData.Password)),
                    PasswordSalt = hmac.Key
                };

                dbContext.FranchiseShops.Add(shop);
                await dbContext.SaveChangesAsync();

                UserDto userDto = new UserDto
                {
                    Email = shop.Email,
                    Token = tokenService.CreateToken(shop)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Route("api/shops/login")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(AuthDto authData)
        {
            try
            {
                FranchiseShop shop = await dbContext.FranchiseShops
                    .SingleOrDefaultAsync(x => x.Email == authData.Email);
                if (shop == null)
                {
                    return Unauthorized("Invalid Email");
                }

                if (!PasswordService.CheckPassword(shop, authData))
                {
                    return Unauthorized("Invalid Password");
                }

                UserDto userDto = new UserDto
                {
                    Email = shop.Email,
                    Token = tokenService.CreateToken(shop)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/shops")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseShop>>> GetAllShops()
        {
            try
            {
                return await dbContext.FranchiseShops
                    .Include(x => x.FastFoodFranchise)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/shops/{shopEmail}")]
        [HttpGet]
        public async Task<ActionResult<FranchiseShop>> GetShop(string shopEmail)
        {
            try
            {
                return await dbContext.FranchiseShops
                    .Include(x => x.FastFoodFranchise)
                    .Include(x => x.ClimateDevices)
                    .Include(x => x.ShopAdmins)
                    .SingleOrDefaultAsync(x => x.Email == shopEmail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/shops/delete/{shopEmail}")]
        [HttpGet]
        public async Task<ActionResult> DeleteShop(string shopEmail)
        {
            try
            {
                FranchiseShop shop = dbContext.FranchiseShops
                    .SingleOrDefault(x => x.Email == shopEmail);
                if (shop == null)
                {
                    return BadRequest("Shop Not Found");
                }

                dbContext.FranchiseShops.Remove(shop);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/shops/devices/{shopEmail}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClimateDevice>>> GetShopDevices(string shopEmail)
        {
            try
            {
                return await dbContext.FranchiseShops
                    .Include(x => x.ClimateDevices)
                    .Where(x => x.Email == shopEmail)
                    .Select(x => x.ClimateDevices)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        private async Task<bool> UserExists(string email)
        {
            return await dbContext.FranchiseShops.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
