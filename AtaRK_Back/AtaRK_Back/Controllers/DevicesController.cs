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
    public class DevicesController : ControllerBase
    {
        private readonly ServerDbContext dbContext;

        public DevicesController(ServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [Route("api/devices")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClimateDevice>>> GetAllClimateDevices()
        {
            try
            {
                return await dbContext.ClimateDevices
                    .Include(x => x.FranchiseShop)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/devices/{deviceId}")]
        [HttpGet]
        public async Task<ActionResult<ClimateDevice>> GetClimateDevice(int deviceId)
        {
            try
            {
                return await dbContext.ClimateDevices
                    .Include(x => x.FranchiseShop)
                    .SingleOrDefaultAsync(x => x.Id == deviceId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/devices")]
        [HttpPost]
        public async Task<ActionResult> RegisterClimateDevice(ClimateDevice climateDevice)
        {
            try
            {
                if (climateDevice == null)
                {
                    return BadRequest("Device Is Empty");
                }

                FranchiseShop shop = dbContext.FranchiseShops
                    .Include(x => x.ClimateDevices)
                    .SingleOrDefault(x => x.Email == climateDevice.FranchiseShop.Email);
                if (shop == null)
                {
                    return BadRequest("Franchise Not Found");
                }

                if (shop.ClimateDevices.Find(x => x.DeviceNumber == climateDevice.DeviceNumber) != null)
                {
                    return BadRequest("Device Already Added");
                }

                dbContext.ClimateDevices.Add(climateDevice);
                shop.ClimateDevices.Add(climateDevice);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/devices/delete/{deviceId}")]
        [HttpGet]
        public async Task<ActionResult> DeleteClimateDevice(int deviceId)
        {
            try
            {
                ClimateDevice climateDevice = dbContext.ClimateDevices.Find(deviceId);
                if (climateDevice == null)
                {
                    return BadRequest("ClimateDevice Not Found");
                }

                dbContext.ClimateDevices.Remove(climateDevice);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/devices/climate/{deviceId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClimateState>>> GetClimateDataBByDevice(int deviceId)
        {
            try
            {
                return await dbContext.ClimateDevices
                    .Include(x => x.ClimateStates)
                    .Select(x => x.ClimateStates)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }
    }
}
