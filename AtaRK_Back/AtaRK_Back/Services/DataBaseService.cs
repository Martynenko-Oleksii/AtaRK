using AtaRK.Data;
using AtaRK_Back.Services.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AtaRK_Back.Services
{
    public enum ObjectName
    {
        Franchise,
        Shop,
        Device
    }

    public class DataBaseService : IDataBaseService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public DataBaseService(IConfiguration configuration, IFileService fileService)
        {
            _configuration = configuration;
            _fileService = fileService;
        }

        public XLWorkbook ExportData()
        {
            using XLWorkbook workbook = new XLWorkbook();
            AddWorksheet(workbook, "Climate Devices", "ClimateDevices");
            AddWorksheet(workbook, "Climate States", "ClimateStates");
            AddWorksheet(workbook, "Fast-food Franchises", "FastFoodFranchises");
            AddWorksheet(workbook, "Franchise Contact Info", "FranchiseContactInfos");
            AddWorksheet(workbook, "Franchise Images", "FranchiseImages");
            AddWorksheet(workbook, "Franchise Shops", "FranchiseShops");
            AddWorksheet(workbook, "Shop Admins per Shops", "FranchiseShopShopAdmin");
            AddWorksheet(workbook, "Shop Admins", "ShopAdmins");
            AddWorksheet(workbook, "Shop Applications", "ShopApplications");
            AddWorksheet(workbook, "System Admins", "SystemAdmins");
            AddWorksheet(workbook, "TechMessage Answers", "TechMessageAnswers");
            AddWorksheet(workbook, "TechMessages", "TechMessages");
            AddWorksheet(workbook, "Users Auth Data", "Users");

            return workbook;
        }

        public async Task ImportDataAsync(IFormFile dataFiles, string tableName)
        {
            string filePath = await _fileService.SaveDataFileAsync(dataFiles);
            string connectionString = GetConnectionString(filePath);

            DataTable dtExcel = new DataTable();
            FillExcelDataTable(dtExcel, connectionString);

            FillDataBase(dtExcel);
        }

        public XLWorkbook CopyData(string table)
        {
            using XLWorkbook workbook = new XLWorkbook();
            AddWorksheet(workbook, table, table);

            return workbook;
        }

        public XLWorkbook CopyData(ObjectName? objectName = null, List<int> objectIds = null, List<string> tables = null)
        {
            using XLWorkbook workbook = new XLWorkbook();

            switch (objectName)
            {
                case ObjectName.Franchise:
                    CopyDataForObjects(workbook, objectIds, tables,
                        "Fast-food Franchises", "FastFoodFranchises", "FastFoodFranchiseId");
                    break;
                case ObjectName.Shop:
                    CopyDataForObjects(workbook, objectIds, tables,
                        "Franchise Shops", "FranchiseShops", "FranchiseShopId");
                    break;
                case ObjectName.Device:
                    CopyDataForObjects(workbook, objectIds, tables,
                        "Climate Devices", "ClimateDevices", "ClimateDeviceId");
                    break;
                default:
                    break;
            }

            return workbook;
        }

        private void CopyDataForObjects(XLWorkbook workbook, List<int> objectIds, List<string> tables,
            string sheetName, string mainTable, string mainIdName)
        {
            int startRowIndex = 2;
            int[] rowIndexes = new int[tables.Count];
            FillStartRowIndexes(rowIndexes);

            foreach (int id in objectIds)
            {
                startRowIndex = AddWorksheet(workbook, sheetName, $"{mainTable} WHERE Id = {id}", startRowIndex);
                int i = 0;
                foreach (string table in tables)
                {
                    rowIndexes[i] = AddWorksheet(workbook, table, $"{table} WHERE {mainIdName} = {id}", rowIndexes[i]);
                    i++;
                }
            }
        }

        private int AddWorksheet(XLWorkbook workbook, string worksheetName, string table, int startRowIndex = 2)
        {
            IXLWorksheet worksheet = null;

            if (!workbook.Worksheets.Contains(worksheetName))
            {
                worksheet = workbook.Worksheets.Add(worksheetName);
            }
            else
            {
                worksheet = workbook.Worksheets.Worksheet(worksheetName);
            }

            return FillWorsheet(worksheet, table, startRowIndex);
        }

        private int FillWorsheet(IXLWorksheet worksheet, string table, int startRowindex)
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(_configuration["DbConnectionString"]);
            SqlDataAdapter dataAdapter = new SqlDataAdapter($"SELECT * FROM {table}", connection);
            dataAdapter.Fill(dataTable);

            int columnIndex = 1;
            foreach (DataColumn column in dataTable.Columns)
            {
                worksheet.Cell(1, columnIndex).Value = column.ColumnName;
                columnIndex++;
            }

            int rowIndex = startRowindex;
            foreach (DataRow row in dataTable.Rows)
            {
                columnIndex = 0;
                foreach (DataColumn column in dataTable.Columns)
                {
                    worksheet.Cell(rowIndex, columnIndex + 1).Value = row[columnIndex];
                    columnIndex++;
                }
                rowIndex++;
            }

            return rowIndex;
        }

        private void FillStartRowIndexes(int[] startRowIndexes)
        {
            for (int i = 0; i < startRowIndexes.Length; i++)
            {
                startRowIndexes[i] = 2;
            }
        }

        private string GetConnectionString(string filePath)
        {
            string fileName = _fileService.EnsureCorrectFilename(filePath);
            string extension = Path.GetExtension(fileName);

            string connectionString = String.Empty;
            switch (extension)
            {
                case ".xls":
                    connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx":
                    connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            return connectionString;
        }

        private void FillExcelDataTable(DataTable dtExcel, string connectionString)
        {
            using OleDbConnection connectionExcel = new OleDbConnection(connectionString);
            using OleDbCommand commandExcel = new OleDbCommand();
            using OleDbDataAdapter dataAdapterExcel = new OleDbDataAdapter();
            commandExcel.Connection = connectionExcel;

            connectionExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connectionExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connectionExcel.Close();

            connectionExcel.Open();
            commandExcel.CommandText = "SELECT * From [" + sheetName + "]";
            dataAdapterExcel.SelectCommand = commandExcel;
            dataAdapterExcel.Fill(dtExcel);
            connectionExcel.Close();
        }

        private void FillDataBase(DataTable dtExcel)
        {
            using SqlConnection con = new SqlConnection(_configuration["DbConnectionString"]);
            using SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con);

            sqlBulkCopy.DestinationTableName = "dbo.Student_details";

            con.Open();
            sqlBulkCopy.WriteToServer(dtExcel);
            con.Close();
        }
    }
}
