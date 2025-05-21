using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Services.Storage;
using MinhasReceitasApp.Domain.ValueObjects;

namespace MinhasReceitasApp.Infrastructure.Services.Storage;

public class AzureStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> GetImageUrl(User user, string fileName)
    {
        var containerName = user.UserIdentifier.ToString();

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var exist = await containerClient.ExistsAsync();

        if (exist.Value.IsFalse())
            return string.Empty;

        var blobClient = containerClient.GetBlobClient(fileName);
        exist = await blobClient.ExistsAsync();
        if (exist.Value)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(MinhasReceitasAppRuleConstants.MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES)
            };

            sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }

        return string.Empty;
    }

    public async Task Upload(User user, Stream file, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        await container.CreateIfNotExistsAsync();

        var blobClient = container.GetBlobClient(fileName);
        await blobClient.UploadAsync(file, overwrite: true);
    }
}
