using AtaRK_Back.Services.Interfaces;
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

        private string GetPathAndFilename(string filename)
        {
            return _env.WebRootPath + filename;
        }
    }
}
