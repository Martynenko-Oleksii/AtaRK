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
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ServerDbContext dbContext;

        public MessagesController(ServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [Route("api/messages")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechMessage>>> GetAllTechMessages()
        {
            try
            {
                return await dbContext.TechMessages
                    .Include(x => x.ClimateDevice)
                    .Include(x => x.ShopAdmin)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/messages/{messageId}")]
        [HttpGet]
        public async Task<ActionResult<TechMessage>> GetTechMessage(int messageId)
        {
            try
            {
                return await dbContext.TechMessages
                       .Include(x => x.ClimateDevice)
                       .Include(x => x.ShopAdmin)
                       .SingleOrDefaultAsync(x => x.Id == messageId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/messages")]
        [HttpPost]
        public async Task<ActionResult> SendTechMessage(TechMessage message)
        {
            try
            {
                if (message == null)
                {
                    return BadRequest("Message Is Empty");
                }

                ClimateDevice device = dbContext.ClimateDevices
                    .Include(x => x.TechMessages)
                    .SingleOrDefault(x => x.Id == message.ClimateDevice.Id);
                if (device == null)
                {
                    return BadRequest("Device Not Found");
                }

                ShopAdmin shopAdmin = dbContext.ShopAdmins
                    .Include(x => x.TechMessages)
                    .SingleOrDefault(x => x.Id == message.ShopAdmin.Id);
                if (shopAdmin == null)
                {
                    return BadRequest("Shop Admin Not Found");
                }

                TechMessage techMessage = new TechMessage
                {
                    Title = message.Title,
                    State = message.State,
                    Message = message.Message,
                    ContactEmail = message.ContactEmail,
                    ClimateDevice = device,
                    ShopAdmin = shopAdmin
                };
                dbContext.TechMessages.Add(techMessage);
                device.TechMessages.Add(techMessage);
                shopAdmin.TechMessages.Add(techMessage);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/messages/delete/{messageId}")]
        [HttpGet]
        public async Task<ActionResult> DeleteClimateDevice(int messageId)
        {
            try
            {
                TechMessage techMessage = dbContext.TechMessages.Find(messageId);
                if (techMessage == null)
                {
                    return BadRequest("Message Not Found");
                }

                dbContext.TechMessages.Remove(techMessage);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/messages/answer/{messageId}")]
        [HttpPost]
        public async Task<ActionResult> SendTechMessageAnswer([FromRoute] int messageId, [FromBody] TechMessageAnswer answer)
        {
            try
            {
                if (answer == null)
                {
                    return BadRequest("Answer Is Empty");
                }

                TechMessage techMessage = dbContext.TechMessages
                    .Include(x => x.TechMessageAnswer)
                    .SingleOrDefault(x => x.Id == messageId);
                if (techMessage == null)
                {
                    return BadRequest("Tech Message Not Found");
                }

                dbContext.TechMessageAnswers.Add(answer);
                techMessage.TechMessageAnswer = answer;
                techMessage.State = 1;
                await dbContext.SaveChangesAsync();

                // TODO: SendMail...

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }

        [Authorize]
        [Route("api/messages/close/{messageId}")]
        [HttpPost]
        public async Task<ActionResult> CloseTechMessage(int messageId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }
        }
    }
}
