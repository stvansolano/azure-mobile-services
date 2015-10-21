namespace Backend
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    // https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/

    public class MediaStorageRepository
    {
        public MediaStorageRepository(string container, CloudContext context)
        {
            Container = container;
            Context = context;
        }

        protected string Container { get; set; }
        protected CloudContext Context { get; set; }

        public async Task<string> Add(string contents)
        {
            var container = await Context.GetContainer(Container);

            var blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString());

            // Create or overwrite the "myblob" blob with contents from a local file.
            await blockBlob.UploadTextAsync(contents);

            return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
        }

        public async Task Download(string identifier, Stream fileStream)
        {
            var container = await Context.GetContainer(Container);

            var blockBlob = container.GetBlockBlobReference(identifier);

            await blockBlob.DownloadToStreamAsync(fileStream);
        }
    }
}