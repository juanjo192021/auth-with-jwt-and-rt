using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// Convierte un IFormFile en un arreglo de bytes.
        /// </summary>
        public static async Task<byte[]?> ToByteArrayAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Convierte un IFormFile en un MemoryStream (por si prefieres trabajarlo como flujo).
        /// </summary>
        public static async Task<MemoryStream?> ToMemoryStreamAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
