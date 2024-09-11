using Microsoft.AspNetCore.Components.Forms;

namespace MyApp.Models
{
    public class MediaAttachment : IBrowserFile
    {
        public string Name { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public long Size { get; set; }

        public string ContentType { get; set; }

        public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public static class Settings
        {
            public static readonly int MAX_FILESIZE = 5000 * 1024; // 5 MB
            public static readonly int MAX_ALLOWEDFILES = 3;
        }
    }


}
