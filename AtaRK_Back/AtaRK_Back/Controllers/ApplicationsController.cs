using AtaRK.Data;
using AtaRK.Models;
using AtaRK_Back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Controllers
{
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ServerDbContext _dbContext;
        private readonly IMailService _mailService;

        public ApplicationsController (ServerDbContext dbContext, IMailService mailService)
        {
            _dbContext = dbContext;
            _mailService = mailService;
        }

        [Authorize]
        [Route("api/applications")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopApplication>>> GetApplications()
        {
            try
            {
                return await _dbContext.ShopApplications
                    .Include(x => x.FastFoodFranchise)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/applications/{applicationId}")]
        [HttpGet]
        public async Task<ActionResult<ShopApplication>> GetApplication(int applicationId)
        {
            try
            {
                return await _dbContext.ShopApplications
                    .Include(x => x.FastFoodFranchise)
                    .SingleOrDefaultAsync(x => x.Id == applicationId);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Route("api/applications/send")]
        [HttpPost]
        public async Task<ActionResult> SendApplication(ShopApplication application)
        {
            try
            {
                if (application == null)
                {
                    return BadRequest("Application Is Empty");
                }

                FastFoodFranchise franchise = _dbContext.FastFoodFranchises
                    .Include(x => x.ShopApplications)
                    .Include(x => x.FranchiseContactInfos)
                    .SingleOrDefault(x => x.Email == application.FastFoodFranchise.Email);
                if (franchise == null)
                {
                    return NotFound("Franchise Not Found");
                }

                ShopApplication dbApplication = new ShopApplication
                {
                    Name = application.Name,
                    Surname = application.Surname,
                    ContactEmail = application.ContactEmail,
                    ContactPhone = application.ContactPhone,
                    City = application.City,
                    Message = application.Message,
                    FastFoodFranchise = franchise
                };
                _dbContext.ShopApplications.Add(dbApplication);

                object sendResult = _mailService.SendApplication(dbApplication);
                if (sendResult is string)
                {
                    return BadRequest(sendResult);
                }

                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/applications/delete/{applicationId}")]
        [HttpGet]
        public async Task<ActionResult> DeleteApplication(int applicationId)
        {
            try
            {
                ShopApplication application = _dbContext.ShopApplications.Find(applicationId);
                if (application == null)
                {
                    return NotFound($"Application {applicationId} Not Found");
                }

                _dbContext.ShopApplications.Remove(application);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }
    }
}
