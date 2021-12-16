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
        private readonly ServerDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public ShopsController(ServerDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [Authorize]
        [Route("api/shops/register/{adminId}")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register([FromRoute] int adminId, [FromBody] AuthDto authData)
        {
            try
            {
                if (await UserExists(authData.Email))
                {
                    return BadRequest("Email Is Already Taken");
                }

                FastFoodFranchise dbFranchise = _dbContext.FastFoodFranchises
                    .SingleOrDefault(x => x.Email == authData.FastFoodFranchise.Email);
                if (dbFranchise == null)
                {
                    return BadRequest("Franchise Not Found");
                }

                HMACSHA512 hmac = new HMACSHA512();
                List<ShopAdmin> admins = new List<ShopAdmin>();
                admins.Add(_dbContext.ShopAdmins.SingleOrDefault(x => x.Id == adminId));
                FranchiseShop shop = new FranchiseShop
                {
                    Email = authData.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(authData.Password)),
                    PasswordSalt = hmac.Key,
                    FastFoodFranchise = dbFranchise,
                    ShopAdmins = admins
                };

                _dbContext.FranchiseShops.Add(shop);
                await _dbContext.SaveChangesAsync();

                UserDto userDto = new UserDto
                {
                    Email = shop.Email,
                    Token = _tokenService.CreateToken(shop)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Route("api/shops/login")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(AuthDto authData)
        {
            try
            {
                FranchiseShop shop = await _dbContext.FranchiseShops
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
                    Id = shop.Id,
                    Email = shop.Email,
                    Token = _tokenService.CreateToken(shop)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shops")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseShop>>> GetAllShops()
        {
            try
            {
                return await _dbContext.FranchiseShops
                    .Include(x => x.FastFoodFranchise)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Route("api/shops/admin/{adminId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseShop>>> GetAdminShops(int adminId)
        {
            try
            {
                ShopAdmin admin = _dbContext.ShopAdmins.Find(adminId);
                return await _dbContext.FranchiseShops
                   .Include(x => x.ShopAdmins)
                   .Where(x => x.ShopAdmins.Contains(admin))
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shops/{shopEmail}")]
        [HttpGet]
        public async Task<ActionResult<FranchiseShop>> GetShop(string shopEmail)
        {
            try
            {
                return await _dbContext.FranchiseShops
                    .Include(x => x.FastFoodFranchise)
                    .Include(x => x.ClimateDevices)
                    .Include(x => x.ShopAdmins)
                    .SingleOrDefaultAsync(x => x.Email == shopEmail);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shops/update")]
        [HttpPost]
        public async Task<ActionResult<FranchiseShop>> UpdateShopInfo(FranchiseShop shop)
        {
            try
            {
                FranchiseShop dbShop = _dbContext.FranchiseShops.Find(shop.Id);
                if (dbShop == null)
                {
                    return BadRequest("Shop Not Found");
                }

                dbShop.City = shop.City;
                dbShop.Street = shop.Street;
                dbShop.BuildingNumber = shop.BuildingNumber;
                dbShop.ContactEmail = shop.ContactEmail;
                dbShop.ContactPhone = shop.ContactPhone;
                await _dbContext.SaveChangesAsync();

                return Ok(dbShop);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shops/change_authdata")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> ChangeShopAuthData(AuthDto authData)
        {
            try
            {
                FranchiseShop dbShop = _dbContext.FranchiseShops.
                    SingleOrDefault(x => x.Email == authData.Email);
                if (dbShop == null)
                {
                    return BadRequest("Shop Not Found");
                }

                HMACSHA512 hmac = new HMACSHA512();

                dbShop.Email = authData.Email;
                dbShop.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(authData.Password));
                dbShop.PasswordSalt = hmac.Key;
                await _dbContext.SaveChangesAsync();

                UserDto userDto = new UserDto
                {
                    Email = dbShop.Email,
                    Token = _tokenService.CreateToken(dbShop)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shops/delete/{shopEmail}")]
        [HttpGet]
        public async Task<ActionResult> DeleteShop(string shopEmail)
        {
            try
            {
                FranchiseShop shop = _dbContext.FranchiseShops
                    .SingleOrDefault(x => x.Email == shopEmail);
                if (shop == null)
                {
                    return BadRequest("Shop Not Found");
                }

                ShopAdmin admin = _dbContext.ShopAdmins.Find(shop.Id);
                _dbContext.ShopAdmins.Remove(admin);
                _dbContext.FranchiseShops.Remove(shop);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/shops/devices/{shopEmail}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClimateDevice>>> GetShopDevices(string shopEmail)
        {
            try
            {
                return await _dbContext.FranchiseShops
                    .Include(x => x.ClimateDevices)
                    .Where(x => x.Email == shopEmail)
                    .Select(x => x.ClimateDevices)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        private async Task<bool> UserExists(string email)
        {
            return await _dbContext.FranchiseShops.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
