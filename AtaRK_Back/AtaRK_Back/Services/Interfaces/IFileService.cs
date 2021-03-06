using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK_Back.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile image);

        string SaveDataFile(XLWorkbook workbook, int objectId, string objectName);

        Task<string> SaveDataFileAsync(IFormFile file);

        Task<string> GetDataFileAsync(IFormFile file);

        string ReadFile(string path);

        object DeleteImage(string path);
    }
}
