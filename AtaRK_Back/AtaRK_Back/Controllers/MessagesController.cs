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
    public class MessagesController : ControllerBase
    {
        private readonly ServerDbContext _dbContext;
        private readonly IMailService _mailService;

        public MessagesController(ServerDbContext dbContext, IMailService mailService)
        {
            _dbContext = dbContext;
            _mailService = mailService;
        }

        [Authorize]
        [Route("api/messages")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechMessage>>> GetAllTechMessages()
        {
            try
            {
                return await _dbContext.TechMessages
                    .Include(x => x.ClimateDevice)
                    .Include(x => x.ShopAdmin)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/messages/notready")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechMessage>>> GetTechMessages()
        {
            try
            {
                return await _dbContext.TechMessages
                    .Include(x => x.ClimateDevice)
                    .Include(x => x.ShopAdmin)
                    .Where(x => x.State == 0 || x.State == 1)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/answers/{email}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechMessageAnswer>>> GetAdminAnswers(string email)
        {
            try
            {
                return await _dbContext.TechMessageAnswers
                    .Include(x => x.TechMessage)
                        .ThenInclude(x => x.ShopAdmin)
                    .Include(x => x.SystemAdmin)
                    .Where(x => x.SystemAdmin.Email == email)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/messages/{messageId}")]
        [HttpGet]
        public async Task<ActionResult<TechMessage>> GetTechMessage(int messageId)
        {
            try
            {
                return await _dbContext.TechMessages
                       .Include(x => x.ClimateDevice)
                       .Include(x => x.ShopAdmin)
                       .SingleOrDefaultAsync(x => x.Id == messageId);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/messages/shopadmin/{adminId}")]
        [HttpGet]
        public async Task<ActionResult<TechMessage>> GetTechMessagePerShopAdmin(int adminId)
        {
            try
            {
                return await _dbContext.TechMessages
                       .Include(x => x.ClimateDevice)
                       .Include(x => x.ShopAdmin)
                       .SingleOrDefaultAsync(x => x.ShopAdmin.Id == adminId);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/messages/device/{deviceId}")]
        [HttpGet]
        public async Task<ActionResult<TechMessage>> GetTechMessagePerDevice(int deviceId)
        {
            try
            {
                return await _dbContext.TechMessages
                       .Include(x => x.ClimateDevice)
                       .Include(x => x.ShopAdmin)
                       .SingleOrDefaultAsync(x => x.ClimateDevice.Id == deviceId);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
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

                ClimateDevice device = _dbContext.ClimateDevices
                    .Include(x => x.TechMessages)
                    .SingleOrDefault(x => x.Id == message.ClimateDevice.Id);
                if (device == null)
                {
                    return BadRequest("Device Not Found");
                }

                ShopAdmin shopAdmin = _dbContext.ShopAdmins
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
                _dbContext.TechMessages.Add(techMessage);
                device.TechMessages.Add(techMessage);
                shopAdmin.TechMessages.Add(techMessage);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/messages/delete/{messageId}")]
        [HttpGet]
        public async Task<ActionResult> DeleteClimateDevice(int messageId)
        {
            try
            {
                TechMessage techMessage = _dbContext.TechMessages.Find(messageId);
                if (techMessage == null)
                {
                    return BadRequest("Message Not Found");
                }

                _dbContext.TechMessages.Remove(techMessage);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
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

                TechMessage techMessage = _dbContext.TechMessages
                    .Include(x => x.TechMessageAnswer)
                    .Include(x => x.ClimateDevice)
                    .SingleOrDefault(x => x.Id == messageId);
                if (techMessage == null)
                {
                    return BadRequest("Tech Message Not Found");
                }

                SystemAdmin admin = _dbContext.SystemAdmins.Find(answer.Id);
                if (admin == null)
                {
                    return BadRequest("Admin Not Found");
                }

                TechMessageAnswer dbAnswer = new TechMessageAnswer
                {
                    Answer = answer.Answer,
                    SystemAdmin = admin
                };

                _dbContext.TechMessageAnswers.Add(dbAnswer);
                techMessage.TechMessageAnswer = dbAnswer;
                techMessage.State = 1;

                object sendResult = _mailService.SendTechAnswer(techMessage);
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
        [Route("api/messages/close/{messageId}")]
        [HttpPost]
        public async Task<ActionResult> CloseTechMessage(int messageId)
        {
            try
            {
                TechMessage techMessage = _dbContext.TechMessages.Find(messageId);
                techMessage.State = 2;
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
