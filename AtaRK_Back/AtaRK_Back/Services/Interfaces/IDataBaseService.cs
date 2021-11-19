using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK_Back.Services.Interfaces
{
    public interface IDataBaseService
    {
        XLWorkbook ExportData();

        void ImportData(List<IFormFile> dataFiles, string tableName);

        XLWorkbook CopyData(string table);

        XLWorkbook CopyData(ObjectName? objectName = null, List<int> objectIds = null, List<string> tables = null);
    }
}
