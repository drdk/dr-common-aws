using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace DR.Common.AWS.S3
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(string path, string key = null,
            EventHandler<UploadProgressArgs> progressHandler = null);

        Task<string> UploadFileAsync(FileInfo fileInfo, string key = null,
            EventHandler<UploadProgressArgs> progressHandler = null);

        Task<string> DownloadFileAsync(string key, string path,
            EventHandler<WriteObjectProgressArgs> progressHandler = null);

        Task<FileInfo> DownloadFileAsync(string key, DirectoryInfo directoryInfo,
            EventHandler<WriteObjectProgressArgs> progressHandler = null);

        string GetPreSignedUrl(string key, TimeSpan? timeToLive = null);
    }
}