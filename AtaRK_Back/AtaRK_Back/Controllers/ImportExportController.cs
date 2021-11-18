using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtaRK.Data;
using AtaRK_Back.Services.Interfaces;
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

        public ImportExportController(ServerDbContext dbContext, IFileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }

        [Authorize]
        [Route("api/import")]
        [HttpPost]
        public async Task<ActionResult> ImportData([FromForm(Name = "table")] string table, 
            [FromForm(Name = "files")] List<IFormFile> files)
        {
            try
            {
                // TODO: ImportData...

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
        public async Task<ActionResult> ExportData([FromForm] List<string> tables)
        {
            try
            {
                // TODO: ExportData...

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
        public async Task<ActionResult> CopyData()
        {
            try
            {
                // TODO: CopyData...

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
        public async Task<ActionResult> CopyDataPerFranchise([FromForm(Name = "franchise_ids")] List<int> franchiseIds,
            [FromForm(Name = "tables")] List<string> tables)
        {
            try
            {
                // TODO: CopyDataPerFranchise...

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
        public async Task<ActionResult> CopyDataPerShop([FromForm(Name = "shop_ids")] List<int> shopIds,
            [FromForm(Name = "tables")] List<string> tables)
        {
            try
            {
                // TODO: CopyDataPerShop...

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
        public async Task<ActionResult> CopyDataPeDevice([FromForm(Name = "device_ids")] List<int> deviceIds,
            [FromForm(Name = "tables")] List<string> tables)
        {
            try
            {
                // TODO: CopyDataPeDevice...

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.InnerException}");
            }
        }
    }
}
