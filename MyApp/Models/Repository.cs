using Microsoft.AspNetCore.Components.Forms;

namespace MyApp.Models
{
    internal static class Repository
    {
        public static List<Issue> _issues = new List<Issue>();

        public async static Task<Response> AddIssue(Issue newIssue)
        {
            Response storageResponse = await StoreAttachments(newIssue.MediaAttachments);

            if (!storageResponse.Success) return storageResponse;

            _issues.Add(newIssue);

            Response response = new Response()
            {
                Success = true,
                Message = "Data Saved Successfully!"
            };

            return response;
        }


        // The following file funcitons were adapted from youtube.com
        // Author: Claudio Bernasconi
        // Link: https://www.youtube.com/watch?v=a4vUjyf-sjQ
        public async static Task<Response> StoreAttachments(IBrowserFile[] attachments)
        {
            Response response = new Response();

            try
            {
                // Stores files under the wwwroot folder to enable easy access for display
                string rootPath = GetRootPath();


                for (int i = 0; i < attachments.Length; i++)
                {
                    if (attachments[i] is null) continue;

                    // Generate new metadata about the file
                    MediaAttachment newMetaData = new()
                    {
                        Name = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(attachments[i].Name)),
                        Size = attachments[i].Size,
                        ContentType = attachments[i].ContentType,
                        LastModified = attachments[i].LastModified
                    };

                    // attempts to store file the file using the new metadata
                    var fileStream = attachments[i].OpenReadStream(MediaAttachment.Settings.MAX_FILESIZE);

                    // save a copy of the file to project files and current application directory
                    var newFilePath = Path.Combine(rootPath, "wwwroot", "files", newMetaData.Name);
                    var currentApplicationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "files", newMetaData.Name);

                    await using FileStream targetStream = new FileStream(newFilePath, FileMode.Create);
                    await fileStream.CopyToAsync(targetStream);

                    await using FileStream applicationStream = new FileStream(currentApplicationFilePath, FileMode.Create);
                    await fileStream.CopyToAsync(applicationStream);

                    fileStream.Close();
                    targetStream.Close();
                    applicationStream.Close();

                    // Replace the metadata of the file
                    attachments[i] = newMetaData;
                }


                response.Success = true;
                response.Message = "Attachments Saved Successfully!";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Something went wrong... Try again later.";
            }


            return response;
        }


        public static List<Issue> GetIssues()
        {
            return _issues;
        }


        public static string GetRootPath()
        {
            var directories = AppDomain.CurrentDomain.BaseDirectory.Split("\\").ToList();
            var indexOfRootPath = directories.IndexOf(directories.Where(item => item.Equals("MyApp")).Last());

            string rootPath = "";

            foreach (var item in directories.GetRange(0, indexOfRootPath + 1))
            {
                rootPath = rootPath + item + "\\";
            }

            return rootPath;
        }

    }
}
