using AtaRK_Back.Services.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _imagesFolder = "\\images\\";
        private readonly string _dataFilesFolder = "\\dataFiles";

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile image)
        {
            try
            {
                string filename = ContentDispositionHeaderValue.
                    Parse(image.ContentDisposition).FileName.Trim('"');
                filename = EnsureCorrectFilename(filename);
                string imagePath = GetPathAndFilename(_imagesFolder + filename);

                using (FileStream output = System.IO.File.Create(imagePath))
                {
                    await image.CopyToAsync(output);
                }

                return $"/images/{filename}";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{ex.InnerException}";
            }
        }

        public string SaveDataFile(XLWorkbook workbook, int objectId, string objectName)
        {
            string dataFileName = $"{objectName}_{objectId}.xlsx";
            string dataFilesFolderPath = GetPathAndFilename(_dataFilesFolder);

            if (!Directory.Exists(dataFilesFolderPath))
            {
                Directory.CreateDirectory(dataFilesFolderPath);
            }

            string filePath = Path.Combine(dataFilesFolderPath, dataFileName);
            using Stream stream = new FileStream(filePath, FileMode.Create);
            workbook.SaveAs(stream);

            return filePath;
        }

        public async Task<string> SaveDataFileAsync(IFormFile dataFile)
        {
            string dataFileName = ContentDispositionHeaderValue.
                Parse(dataFile.ContentDisposition).FileName.Trim('"');
            dataFileName = EnsureCorrectFilename(dataFileName);
            string dataFilesFolderPath = GetPathAndFilename(_dataFilesFolder);

            if (!Directory.Exists(dataFilesFolderPath))
            {
                Directory.CreateDirectory(dataFilesFolderPath);
            }

            string filePath = Path.Combine(dataFilesFolderPath, dataFileName);
            using Stream stream = new FileStream(filePath, FileMode.Create);
            await dataFile.CopyToAsync(stream);

            return filePath;
        }

        public Task<string> GetDataFileAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public string ReadFile(string path)
        {
            try
            {
                string stringFile;
                using (FileStream fs = File.OpenRead($"{_env.WebRootPath}{path}"))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    stringFile = Encoding.Default.GetString(array);
                }

                return stringFile;
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{ex.InnerException}";
            }
        }

        public object DeleteImage(string path)
        {
            try
            {
                string imageName = EnsureCorrectFilename(path);
                string imagePath = GetPathAndFilename(_imagesFolder + imageName);

                File.Delete(imagePath);

                return true;
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{ex.InnerException}";
            }
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
            {
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);
            }
            else if (filename.Contains("/"))
            {
                filename = filename.Substring(filename.LastIndexOf("/") + 1);
            }

            return filename;
        }

        private string GetPathAndFilename(string filePath)
        {
            return _env.WebRootPath + filePath;
        }
    }
}
