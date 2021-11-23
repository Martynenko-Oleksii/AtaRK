using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtaRK.Data;
using AtaRK.DTO;
using AtaRK.Models;
using AtaRK_Back.Services;
using AtaRK_Back.Services.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtaRK_Back.Controllers
{
    [ApiController]
    public class ImportExportController : ControllerBase
    {
        private readonly ServerDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IDataBaseService _dataBaseService;
        private readonly string _contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ImportExportController(ServerDbContext dbContext, IFileService fileService, IDataBaseService dataBaseService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _dataBaseService = dataBaseService;
        }

        [Authorize]
        [Route("api/import")]
        [HttpPost]
        public async Task<IActionResult> ImportData([FromForm(Name = "table")] string table, 
            [FromForm(Name = "file")] IFormFile file)
        {
            try
            {
                await _dataBaseService.ImportDataAsync(file, table);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/export")]
        [HttpPost]
        public IActionResult ExportData(AuthDto authDto)
        {
            try
            {
                SystemAdmin admin = _dbContext.SystemAdmins
                    .SingleOrDefault(x => x.Email == authDto.Email);
                if (admin == null)
                {
                    return BadRequest("Admin Not Found");
                }
                else if (!admin.IsMaster)
                {
                    return BadRequest("Admin Is Not Master");
                }

                using XLWorkbook workbook = _dataBaseService.ExportData();
                _fileService.SaveDataFile(workbook, admin.Id, "export");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/copy")]
        [HttpPost]
        public IActionResult CopyData([FromForm(Name = "email")] string email, 
            [FromForm(Name = "table")] string table)
        {
            try
            {
                SystemAdmin admin = _dbContext.SystemAdmins
                    .SingleOrDefault(x => x.Email == email);
                if (admin == null)
                {
                    return BadRequest("Admin Not Found");
                }
                else if (!admin.IsMaster)
                {
                    return BadRequest("Admin Is Not Master");
                }

                using XLWorkbook workbook = _dataBaseService.CopyData(table);
                _fileService.SaveDataFile(workbook, admin.Id, $"copy_{table}");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/copy/franchises")]
        [HttpPost]
        public IActionResult CopyDataPerFranchise([FromForm(Name = "franchise_ids")] List<int> franchiseIds,
            [FromForm(Name = "tables")] List<string> tables,
            [FromForm(Name = "email")] string email)
        {
            try
            {
                SystemAdmin admin = _dbContext.SystemAdmins
                    .SingleOrDefault(x => x.Email == email);
                if (admin == null)
                {
                    return BadRequest("Admin Not Found");
                }
                else if (!admin.IsMaster)
                {
                    return BadRequest("Admin Is Not Master");
                }

                using XLWorkbook workbook = _dataBaseService.CopyData(ObjectName.Franchise, franchiseIds, tables);
                _fileService.SaveDataFile(workbook, admin.Id, "copy_franchises");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/copy/shops")]
        [HttpPost]
        public IActionResult CopyDataPerShop([FromForm(Name = "shop_ids")] List<int> shopIds,
            [FromForm(Name = "tables")] List<string> tables,
            [FromForm(Name = "email")] string email)
        {
            try
            {
                SystemAdmin admin = _dbContext.SystemAdmins
                    .SingleOrDefault(x => x.Email == email);
                if (admin == null)
                {
                    return BadRequest("Admin Not Found");
                }
                else if (!admin.IsMaster)
                {
                    return BadRequest("Admin Is Not Master");
                }

                using XLWorkbook workbook = _dataBaseService.CopyData(ObjectName.Shop, shopIds, tables);
                _fileService.SaveDataFile(workbook, admin.Id, "copy_shops");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/copy/devices")]
        [HttpPost]
        public IActionResult CopyDataPeDevice([FromForm(Name = "device_ids")] List<int> deviceIds,
            [FromForm(Name = "tables")] List<string> tables,
            [FromForm(Name = "email")] string email)
        {
            try
            {
                SystemAdmin admin = _dbContext.SystemAdmins
                    .SingleOrDefault(x => x.Email == email);
                if (admin == null)
                {
                    return BadRequest("Admin Not Found");
                }
                else if (!admin.IsMaster)
                {
                    return BadRequest("Admin Is Not Master");
                }

                using XLWorkbook workbook = _dataBaseService.CopyData(ObjectName.Device, deviceIds, tables);
                _fileService.SaveDataFile(workbook, admin.Id, "copy_devices");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/export/climate_statistic/{objectId}")]
        [HttpPost]
        public IActionResult ExportClimateStatistic([FromRoute] int objectId, [FromForm(Name = "object_name")] string objectName)
        {
            try
            {
                using XLWorkbook climateStatistic = _dataBaseService.CopyData(objectId, objectName);
                _fileService.SaveDataFile(climateStatistic, objectId, objectName);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }
    }
}
