using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtaRK.Data;
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
        private readonly string _contentType = "application/vnd.openxmlformats-officedocument-spreadsheetml.sheet";

        public ImportExportController(ServerDbContext dbContext, IFileService fileService, IDataBaseService dataBaseService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _dataBaseService = dataBaseService;
        }

        [Authorize]
        [Route("api/import")]
        [HttpPost]
        public IActionResult ImportData([FromForm(Name = "table")] string table, 
            [FromForm(Name = "files")] List<IFormFile> files)
        {
            try
            {
                _dataBaseService.ImportData(files, table);

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
        public IActionResult ExportData([FromForm] List<string> tables)
        {
            try
            {
                using MemoryStream stream = new MemoryStream();
                XLWorkbook workbook = _dataBaseService.ExportData();
                workbook.SaveAs(stream);
                byte[] content = stream.ToArray();

                return File(content, _contentType, "export.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/copy")]
        [HttpPost]
        public IActionResult CopyData([FromForm(Name = "table")] string table)
        {
            try
            {
                using MemoryStream stream = new MemoryStream();
                XLWorkbook workbook = _dataBaseService.CopyData(table);
                workbook.SaveAs(stream);
                byte[] content = stream.ToArray();

                return File(content, _contentType, $"{table}_copy.xlsx");
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
            [FromForm(Name = "tables")] List<string> tables)
        {
            try
            {
                using MemoryStream stream = new MemoryStream();
                XLWorkbook workbook = _dataBaseService.CopyData(ObjectName.Franchise, franchiseIds, tables);
                workbook.SaveAs(stream);
                byte[] content = stream.ToArray();

                return File(content, _contentType, "franchises_copy.xlsx");
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
            [FromForm(Name = "tables")] List<string> tables)
        {
            try
            {
                using MemoryStream stream = new MemoryStream();
                XLWorkbook workbook = _dataBaseService.CopyData(ObjectName.Shop, shopIds, tables);
                workbook.SaveAs(stream);
                byte[] content = stream.ToArray();

                return File(content, _contentType, "shops_copy.xlsx");
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
            [FromForm(Name = "tables")] List<string> tables)
        {
            try
            {
                using MemoryStream stream = new MemoryStream();
                XLWorkbook workbook = _dataBaseService.CopyData(ObjectName.Device, deviceIds, tables);
                workbook.SaveAs(stream);
                byte[] content = stream.ToArray();

                return File(content, _contentType, "devices_copy.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }

        [Authorize]
        [Route("api/export/climate_statistic")]
        [HttpPost]
        public async Task<IActionResult> ExportClimateStatistic([FromForm(Name = "object_name")] string objectName)
        {
            try
            {
                // TODO: ExportClimateStatistic...

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }
    }
}
