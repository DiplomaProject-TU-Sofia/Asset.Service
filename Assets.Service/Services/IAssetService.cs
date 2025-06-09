namespace Assets.Service.Services
{
	public interface IAssetService
	{
		Task<string> UploadAsync(IFormFile file);
		Task<Stream> GetAsync(string fileName);
	}

}
