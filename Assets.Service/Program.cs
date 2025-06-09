using Assets.Service.Services;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Assets.Service
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var configuration = builder.Configuration;
			var connectionString = configuration["AzureBlobStorage:ConnectionString"];

			var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2020_10_02);
			builder.Services.AddSingleton(new BlobServiceClient(connectionString, options));

			builder.Services.AddControllers();

			builder.Services.AddScoped<IAssetService, AssetService>();

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
					ValidateAudience = true,
					ValidAudience = builder.Configuration["JwtSettings:Audience"],
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)
					),
					RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
				};
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
