using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Helpers.Interfaces
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string directory);
    }
}
