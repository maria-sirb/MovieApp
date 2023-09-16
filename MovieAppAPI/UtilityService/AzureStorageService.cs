using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

namespace MovieAppAPI.UtilityService
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly string _storageAccount;
        private readonly string _storageAccessKey;
        public AzureStorageService(IConfiguration configuration) 
        {
            _storageConnectionString = configuration.GetValue<string>("AzureStorage:BlobConnectionString");
            _storageContainerName = configuration.GetValue<string>("AzureStorage:BlobContainerName");
            _storageAccount = configuration.GetValue<string>("AzureStorage:Account");
            _storageAccessKey = configuration.GetValue<string>("AzureStorage:AccessKey");
        }

        public void DeleteImage(string imageName)
        {
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            BlobClient blob = container.GetBlobClient(imageName);
            blob.Delete();
        }

        public string UploadImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-')
                                + DateTime.Now.ToString("yymmddssfff");
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            BlobClient blob = container.GetBlobClient(imageName);
            using(Stream stream = imageFile.OpenReadStream())
            {
                blob.Upload(stream);
            }
            return imageName;
        }

        public string GetFileUrl(string imageName)
        {
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _storageContainerName,
                BlobName = imageName,
                ExpiresOn = DateTime.UtcNow.AddMinutes(2)
            };
            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
            var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_storageAccount, _storageAccessKey)).ToString();

            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            BlobClient blob = container.GetBlobClient(imageName);
            return blob.Uri.ToString() + "?" + sasToken;
        }
    }
}
