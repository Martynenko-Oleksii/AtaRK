using AtaRK.Data;
using AtaRK.DTO;
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
    public class ClimatesController : ControllerBase
    {
        private readonly ServerDbContext dbContext;

        public ClimatesController(ServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [Route("api/climates")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClimateState>>> GetAllClimateData()
        {
            try
            {
                return await dbContext.ClimateStates
                    .Include(x => x.ClimateDevice)
                    .Include(x => x.FranchiseShop)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/climates/{dataId}")]
        [HttpGet]
        public async Task<ActionResult<ClimateState>> GetClimateData(int dataId)
        {
            try
            {
                return await dbContext.ClimateStates
                    .Include(x => x.ClimateDevice)
                    .Include(x => x.FranchiseShop)
                    .SingleOrDefaultAsync(x => x.Id == dataId);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Route("api/climates")]
        [HttpPost]
        public async Task<ActionResult> PostCliamteData(ClimateDto climateData)
        {
            try
            {
                if (climateData == null)
                {
                    return BadRequest();
                }

                ClimateDevice device = dbContext.ClimateDevices
                    .Include(x => x.FranchiseShop)
                    .Include(x => x.ClimateStates)
                    .SingleOrDefault(x => x.DeviceNumber == climateData.ClimateDeviceNumber);
                if (device == null)
                {
                    return BadRequest();
                }
                device.IsOnline = true;

                ClimateState climateState = new ClimateState
                { 
                    Temperature = climateData.Temperature,
                    Huumidity = climateData.Huumidity,
                    ClimateDevice = device,
                    FranchiseShop = device.FranchiseShop
                };

                dbContext.ClimateStates.Add(climateState);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/climates/delete/{dataId}")]
        [HttpGet]
        public async Task<ActionResult> DeleteClimateData(int dataId)
        {
            try
            {
                ClimateState climateState = dbContext.ClimateStates.Find(dataId);
                if (climateState == null)
                {
                    return BadRequest("ClimateData Not Found");
                }

                dbContext.ClimateStates.Remove(climateState);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }
    }
}
