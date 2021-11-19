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

        Task<string> SaveDataFileAsync(IFormFile file);

        string ReadFile(string path);

        object DeleteImage(string path);

        string EnsureCorrectFilename(string filename);
    }
}
