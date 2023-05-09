using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Services
{
    public interface ICloudStorageService
    {
        Task<Uri> UploadFile(Stream stream, string cloudStorageContainer, string extension, string prepend = null, string fileName = null);
        Task<Uri> UploadProductBinary(Stream stream, string cloudStorageContainer);
        Task<bool> TryDelete(Uri blobUri);
        Task<bool> TryDelete(Uri blobUri, string cloudStorageContainer);
        Task<bool> TryDelete(string fileName, string cloudStorageContainer);
        Task<bool> TryDeleteProductImage(string fileName);
        Task<bool> TryDeleteProductBinary(string fileName);
        Task<Stream> TryDownload(Stream stream, string cloudStorageContainer, string fileName);
        Task<bool> FileExists(string container, string fileName);
        Task<Uri> GetFile(string container, string fileName);
    }
}
