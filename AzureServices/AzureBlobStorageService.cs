using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightDemo.AzureServices
{
    public class AzureBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobStorageService()
        {
            string storageAccountName = "stoedhrfilestoreeustst";
            string containerName = "processes";

            var serviceClient = new BlobServiceClient(
                new Uri(
                    $"https://{storageAccountName}.blob.core.windows.net"),
                new AzureCliCredential());

            _containerClient =
                serviceClient.GetBlobContainerClient(containerName);
        }

        public async Task UploadFileAsync(
            string localFilePath,
            string blobName)
        {
            BlobClient blobClient =
                _containerClient.GetBlobClient(blobName);

            await using var fileStream =
                File.OpenRead(localFilePath);


            await blobClient.UploadAsync(
                fileStream,
                overwrite: true);
        }

        public async Task ListFilesAsync()
        {
            await foreach (BlobItem blob in
                _containerClient.GetBlobsAsync())
            {
                Console.WriteLine(blob.Name);
            }
        }

        public async Task<string> DownloadFileAsync(string filename, int maxAttempts = 12)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filename);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxAttempts);

            string downloadFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            Directory.CreateDirectory(downloadFolder);
            string destinationPath =
                Path.Combine(downloadFolder, filename);
            BlobClient blobClient =
                _containerClient.GetBlobClient(filename);

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await blobClient.DownloadToAsync(destinationPath);
                    return destinationPath;
                }
                catch (Azure.RequestFailedException exception) when (exception.Status == 404 && attempt < maxAttempts)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }

            throw new InvalidOperationException($"Blob '{filename}' was not available after {maxAttempts} attempts.");
        }
    }
}
