using Azure.Storage.Blobs;

namespace Assets.Service.Services
{
	public class AssetService : IAssetService
	{
		private readonly BlobContainerClient _containerClient;

		public AssetService(BlobServiceClient blobServiceClient, IConfiguration config)
		{
			var containerName = config["AzureBlobStorage:ContainerName"];
			_containerClient = blobServiceClient.GetBlobContainerClient(containerName);
			_containerClient.CreateIfNotExists();
		}

		public async Task<string> UploadAsync(IFormFile file)
		{
			var blobClient = _containerClient.GetBlobClient(file.FileName);
			await using var stream = file.OpenReadStream();
			await blobClient.UploadAsync(stream, overwrite: true);
			return file.FileName;
		}

		public async Task<Stream> GetAsync(string fileName)
		{
			var blobClient = _containerClient.GetBlobClient(fileName);
			var response = await blobClient.DownloadAsync();
			return response.Value.Content;
		}
	}
}
