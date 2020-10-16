using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    public interface IImageHelper
    {
        Task<String> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
