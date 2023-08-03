using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TicketBooking.API.Helper;

namespace TicketBooking.API.Services
{
	public class BlobService : IBlobService
	{
		public BlobService() {}

		public async Task<string> UpLoadImage(IFormFile file, string name)
		{
			var connectionString = ConfigurationString.BlobStorage;
			var blobServiceClient = new BlobServiceClient(connectionString);
			var containerClient = blobServiceClient.GetBlobContainerClient("img");
			var stream = file.OpenReadStream();
			var blobClient = containerClient.GetBlobClient(name);

			await blobClient.UploadAsync(
				stream,
				httpHeaders: new BlobHttpHeaders { ContentType = file.ContentType },
				conditions: null);

			return blobClient.Uri.ToString();
		}

		public async Task<bool> RemoveImage(string blogName)
		{
			var connectionString = ConfigurationString.BlobStorage;
			var blobServiceClient = new BlobServiceClient(connectionString);
			var containerClient = blobServiceClient.GetBlobContainerClient("img");

			try
			{
				await containerClient.DeleteBlobAsync(blogName);
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}
	}
}