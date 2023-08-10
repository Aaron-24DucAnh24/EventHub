using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TicketBooking.API.Helper;

namespace TicketBooking.API.Services
{
	public class BlobService : IBlobService
	{
		public BlobService() {}

		public async Task<string> UpLoadImageAsync(IFormFile file, string name)
		{
			string? connectionString = ConfigurationString.BlobStorage;
			BlobServiceClient blobServiceClient = new(connectionString);
			BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("img");
			Stream stream = file.OpenReadStream();
			BlobClient blobClient = containerClient.GetBlobClient(name);

			await blobClient.UploadAsync(
				stream,
				httpHeaders: new BlobHttpHeaders { ContentType = file.ContentType },
				conditions: null);

			return blobClient.Uri.ToString();
		}

		public async Task<bool> RemoveImageAsync(string blogName)
		{
			string? connectionString = ConfigurationString.BlobStorage;
			BlobServiceClient blobServiceClient = new(connectionString);
			BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("img");

			try
			{
				await containerClient.DeleteBlobAsync(blogName);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}