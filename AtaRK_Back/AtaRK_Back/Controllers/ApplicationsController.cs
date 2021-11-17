using AtaRK.Data;
using AtaRK.Models;
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
        private readonly ServerDbContext dbContext;

        public ApplicationsController (ServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [Route("api/applications")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopApplication>>> GetApplications()
        {
            try
            {
                return await dbContext.ShopApplications
                    .Include(x => x.FastFoodFranchise)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/applications/{applicationId}")]
        [HttpGet]
        public async Task<ActionResult<ShopApplication>> GetApplication(int applicationId)
        {
            try
            {
                return await dbContext.ShopApplications
                    .Include(x => x.FastFoodFranchise)
                    .SingleOrDefaultAsync(x => x.Id == applicationId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
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

                FastFoodFranchise franchise = dbContext.FastFoodFranchises
                    .Include(x => x.ShopApplications)
                    .SingleOrDefault(x => x.Email == application.FastFoodFranchise.Email);
                if (franchise == null)
                {
                    return NotFound("Franchise Not Found");
                }

                dbContext.ShopApplications.Add(application);
                franchise.ShopApplications.Add(application);

                // TODO: SendMail...

                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/applications/delete/{applicationId}")]
        [HttpGet]
        public async Task<ActionResult> DeleteApplication(int applicationId)
        {
            try
            {
                ShopApplication application = dbContext.ShopApplications.Find(applicationId);
                if (application == null)
                {
                    return NotFound($"Application {applicationId} Not Found");
                }

                dbContext.ShopApplications.Remove(application);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }
    }
}
