using Assets.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assets.Service.Controllers
{
	[ApiController]
	[Route("api/assets")]
	public class AssetController : ControllerBase
	{
		private readonly IAssetService _assetService;

		public AssetController(IAssetService assetService)
		{
			_assetService = assetService;
		}

		[HttpPost("upload")]
		public async Task<IActionResult> Upload([FromForm] IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("File is required.");

			var fileName = await _assetService.UploadAsync(file);
			return Ok(new { FileName = fileName });
		}

		[HttpGet("{fileName}")]
		public async Task<IActionResult> Get(string fileName)
		{
			var stream = await _assetService.GetAsync(fileName);
			return File(stream, "application/octet-stream", fileName);
		}
	}
}
