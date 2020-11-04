namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    using Microsoft.AspNetCore.Http;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IImageHelper
    {
        Task<String> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
