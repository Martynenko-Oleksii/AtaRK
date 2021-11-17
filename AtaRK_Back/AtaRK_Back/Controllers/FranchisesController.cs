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
    public class FranchisesController : ControllerBase
    {
        private readonly ServerDbContext dbContext;
        private readonly ITokenService tokenService;

        public FranchisesController(ServerDbContext dbContext, ITokenService tokenService)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }

        [Route("api/franchises/register")]
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
                FastFoodFranchise franchise = new FastFoodFranchise
                {
                    Email = authData.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(authData.Password)),
                    PasswordSalt = hmac.Key
                };

                dbContext.FastFoodFranchises.Add(franchise);
                await dbContext.SaveChangesAsync();

                UserDto userDto = new UserDto
                {
                    Email = franchise.Email,
                    Token = tokenService.CreateToken(franchise)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Route("api/franchises/login")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(AuthDto authData)
        {
            try
            {
                FastFoodFranchise franchise = await dbContext.FastFoodFranchises
                    .SingleOrDefaultAsync(x => x.Email == authData.Email);
                if (franchise == null)
                {
                    return Unauthorized("Invalid Email");
                }

                if (!PasswordService.CheckPassword(franchise, authData))
                {
                    return Unauthorized("Invalid Password");
                }

                UserDto userDto = new UserDto
                {
                    Email = franchise.Email,
                    Token = tokenService.CreateToken(franchise)
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Route("api/franchises/{email}")]
        [HttpGet]
        public async Task<ActionResult<FastFoodFranchise>> GetFranchise(string email)
        {
            try
            {
                return await dbContext.FastFoodFranchises
                    .Include(x => x.FranchiseImages)
                    .Include(x => x.FranchiseContactInfos)
                    .SingleOrDefaultAsync(x => x.Email == email);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Route("api/franchises")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FastFoodFranchise>>> GetAllFranchises()
        {
            try
            {
                return await dbContext.FastFoodFranchises
                    .Include(x => x.FranchiseImages)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/franchises/info")]
        [HttpPost]
        public async Task<ActionResult<FastFoodFranchise>> SetInfo(FastFoodFranchise franchiseInfo)
        {
            try
            {
                if (franchiseInfo == null)
                {
                    return BadRequest("Franchise Is Empty");
                }

                FastFoodFranchise franchise = dbContext.FastFoodFranchises
                    .SingleOrDefault(x => x.Email == franchiseInfo.Email);
                if (franchise == null)
                {
                    return NotFound("Franchise Not Found");
                }

                franchise.Title = franchiseInfo.Title;
                franchise.Description = franchise.Description;
                franchise.MinTemperature = franchiseInfo.MinTemperature;
                franchise.MaxTemperature = franchiseInfo.MaxTemperature;
                franchise.MinHuumidity = franchiseInfo.MinHuumidity;
                franchise.MaxHuumidity = franchiseInfo.MaxHuumidity;
                await dbContext.SaveChangesAsync();

                return Ok(franchise);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/franchises/banner")]
        [HttpPost]
        public async Task<ActionResult<FastFoodFranchise>> SetBanner([FromForm(Name = "email")] string email, 
            [FromForm(Name = "banner")] IFormFile banner)
        {
            try
            {
                if (email == null)
                {
                    return BadRequest("Email Is Empty");
                }

                if (banner == null)
                {
                    return BadRequest("File Is Empty");
                }

                FastFoodFranchise franchise = dbContext.FastFoodFranchises
                    .Include(x => x.FranchiseImages)
                    .SingleOrDefault(x => x.Email == email);
                if (franchise == null)
                {
                    return NotFound("Franchise Not Found");
                }

                FranchiseImage franchiseBanner = franchise.FranchiseImages.Find(x => x.IsBanner);
                if (franchiseBanner != null)
                {
                    FranchiseImage dbBanner = dbContext.FranchiseImages.Find(franchiseBanner.Id);
                    franchise.FranchiseImages.Remove(franchiseBanner);
                    dbContext.FranchiseImages.Remove(dbBanner);
                    // TODO: DeleteFile...
                }

                string path = null;
                // TODO: SaveFile...
                FranchiseImage imageBanner = new FranchiseImage
                {
                    Path = path,
                    IsBanner = true,
                    FastFoodFranchise = franchise
                };
                dbContext.FranchiseImages.Add(imageBanner);
                franchise.FranchiseImages.Add(imageBanner);
                await dbContext.SaveChangesAsync();

                return Ok(franchise);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/franchises/images")]
        [HttpPost]
        public async Task<ActionResult<FastFoodFranchise>> AddImages([FromForm(Name = "email")] string email,
            [FromForm(Name = "images")] List<IFormFile> images)
        {
            try
            {
                if (email == null)
                {
                    return BadRequest("Email Is Empty");
                }

                if (images == null || images.Count == 0)
                {
                    return BadRequest("Files Is Empty");
                }

                FastFoodFranchise franchise = dbContext.FastFoodFranchises
                    .Include(x => x.FranchiseImages)
                    .SingleOrDefault(x => x.Email == email);
                if (franchise == null)
                {
                    return NotFound("Franchise Not Found");
                }

                List<FranchiseImage> franchiseImages = new List<FranchiseImage>();
                foreach (var image in images)
                {
                    string path = null;
                    // TODO: SaveFile...
                    franchiseImages.Add(new FranchiseImage
                    {
                        Path = path,
                        IsBanner = true,
                        FastFoodFranchise = franchise
                    });
                }

                dbContext.FranchiseImages.AddRange(franchiseImages);
                franchise.FranchiseImages.AddRange(franchiseImages);
                await dbContext.SaveChangesAsync();

                return Ok(franchise);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/franchises/images/delete/{imageId}")]
        [HttpGet]
        public async Task<ActionResult> DeleteImage(int imageId)
        {
            try
            {
                if (imageId == 0)
                {
                    return BadRequest("ImageId Is Empty");
                }

                FranchiseImage image = dbContext.FranchiseImages
                    .Include(x => x.FastFoodFranchise)
                    .SingleOrDefault(x => x.Id == imageId);
                if (image == null)
                {
                    return NotFound("Image Not Found");
                }

                dbContext.FranchiseImages.Remove(image);
                // TODO: DeleteFile...
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/franchises/contactinfo")]
        [HttpPost] 
        public async Task<ActionResult<FastFoodFranchise>> SetContactInfo(FastFoodFranchise franchiseInfo)
        {
            try
            {
                if (franchiseInfo == null)
                {
                    return BadRequest("FranchiseInfo Is Empty");
                }

                FastFoodFranchise franchise = dbContext.FastFoodFranchises
                    .Include(x => x.FranchiseContactInfos)
                    .SingleOrDefault(x => x.Email == franchiseInfo.Email);
                if (franchise == null)
                {
                    return NotFound("Franchise Not Found");
                }

                dbContext.FranchiseContactInfos.RemoveRange(franchise.FranchiseContactInfos);
                dbContext.FranchiseContactInfos.AddRange(franchiseInfo.FranchiseContactInfos);
                franchise.FranchiseContactInfos = franchiseInfo.FranchiseContactInfos;
                await dbContext.SaveChangesAsync();

                return Ok(franchise);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/franchises/shops/{email}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseShop>>> GetShops(string email)
        {
            try
            {
                return await dbContext.FastFoodFranchises
                .Include(x => x.FranchiseShops)
                .Where(x => x.Email == email)
                .Select(x => x.FranchiseShops)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/franchises/delete/{email}")]
        [HttpGet]
        public async Task<ActionResult> DeleteFranchise(string email)
        {
            try
            {
                if (email == null)
                {
                    return BadRequest("Email Is Empty");
                }

                FastFoodFranchise franchise = dbContext.FastFoodFranchises
                    .SingleOrDefault(x => x.Email == email);
                if (franchise == null)
                {
                    return NotFound("Franchise Not Found");
                }

                dbContext.FastFoodFranchises.Remove(franchise);
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
            return await dbContext.FastFoodFranchises.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
