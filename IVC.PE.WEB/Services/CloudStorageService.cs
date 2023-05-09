using IVC.PE.WEB.Options;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly CloudBlobClient _blobClient;
        private readonly CloudStorageCredentials _settings;

        public CloudStorageService(IOptions<CloudStorageCredentials> settings)
        {
            _settings = settings.Value;

            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(
                    _settings.StorageName,
                    _settings.AccessKey), true);

            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        private async Task<Uri> Upload(Stream stream, string cloudStorageContainer, string fileName)
        {
            var container = _blobClient.GetContainerReference(cloudStorageContainer);

            if (await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    }
                );
            }

            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(stream);

            return blockBlob.Uri;
        }

        public async Task<Uri> UploadFile(Stream stream, string cloudStorageContainer, string extension, string prepend = null, string fileName = null)
        {
            return await Upload(stream, cloudStorageContainer, $"{(!string.IsNullOrEmpty(prepend) ? $"{prepend}/" : string.Empty)}{fileName ?? Guid.NewGuid().ToString()}{extension}");
        }

        public async Task<Uri> UploadFileWithExtension(Stream stream, string cloudStorageContainer, string fileNameWithExtension, string prepend = null)
        {
            return await Upload(stream, cloudStorageContainer, $"{(!string.IsNullOrEmpty(prepend) ? $"{prepend}/" : string.Empty)}{fileNameWithExtension}");
        }

        public async Task<Uri> UploadProductBinary(Stream stream, string cloudStorageContainer)
        {
            return await Upload(stream, cloudStorageContainer, Guid.NewGuid().ToString());
        }

        private async Task<bool> Delete(string fileName, string cloudStorageContainer)
        {
            var container = _blobClient.GetContainerReference(cloudStorageContainer);
            var blockBlob = container.GetBlockBlobReference(fileName);
            return await blockBlob.DeleteIfExistsAsync();
        }

        public async Task<bool> TryDelete(Uri blobUri)
        {
            var blockBlob = await _blobClient.GetBlobReferenceFromServerAsync(blobUri);
            return await blockBlob.DeleteIfExistsAsync();
        }

        public async Task<bool> TryDelete(Uri blobUri, string cloudStorageContainer)
        {
            var fileName = Uri.UnescapeDataString(string.Join(string.Empty, blobUri.Segments.Skip(2).ToArray()));
            return await Delete(fileName, cloudStorageContainer);
        }

        public async Task<bool> TryDelete(string fileName, string cloudStorageContainer)
        {
            return await Delete(fileName, cloudStorageContainer);
        }

        public async Task<bool> TryDeleteProductBinary(string fileName)
        {
            return await Delete(fileName, "binaries");
        }

        public async Task<bool> TryDeleteProductImage(string fileName)
        {
            return await Delete(fileName, "applications-images");
        }

        private async Task<Stream> Download(Stream stream, string cloudStorageContainer, string fileName)
        {
            var container = _blobClient.GetContainerReference(cloudStorageContainer);
            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.DownloadToStreamAsync(stream);
            return stream;
        }

        public async Task<Stream> TryDownload(Stream stream, string cloudStorageContainer, string fileName)
        {
            return await Download(stream, cloudStorageContainer, fileName);
        }

        public async Task<Stream> TryDownloadProductBinary(Stream stream, string fileName)
        {
            return await Download(stream, "binaries", fileName);
        }

        public async Task<Stream> TryDownloadProductImage(Stream stream, string fileName)
        {
            return await Download(stream, "applications-images", fileName);
        }

        public async Task<bool> FileExists(string cloudStorageContainer, string fileName)
        {
            var container = _blobClient.GetContainerReference(cloudStorageContainer);
            return await container.ExistsAsync()
                ? await container.GetBlockBlobReference(fileName).ExistsAsync()
                : false;
        }

        public async Task<Uri> GetFile(string cloudStorageContainer, string fileName)
        {
            var container = _blobClient.GetContainerReference(cloudStorageContainer);
            var blockBlob = await container.ExistsAsync()
                ? container.GetBlockBlobReference(fileName)
                : null;
            return await blockBlob.ExistsAsync()
                ? blockBlob.Uri
                : null;
        }
    }
}
