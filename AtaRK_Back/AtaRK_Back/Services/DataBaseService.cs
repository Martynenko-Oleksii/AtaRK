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

        public DataBaseService(IConfiguration configuration)
        {
            _configuration = configuration;
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

        public void ImportData(List<IFormFile> dataFiles, string tableName)
        {
            throw new NotImplementedException();
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
                    break;
                case ObjectName.Shop:
                    break;
                case ObjectName.Device:
                    break;
                default:
                    break;
            }

            return workbook;
        }

        private void AddWorksheet(XLWorkbook workbook, string worksheetName, string table)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add(worksheetName);
            FillWorsheet(worksheet, table);
        }

        private void FillWorsheet(IXLWorksheet worksheet, string table)
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

            int rowIndex = 2;
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
        }
    }
}
