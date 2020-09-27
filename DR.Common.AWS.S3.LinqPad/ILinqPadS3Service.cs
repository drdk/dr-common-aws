using System;
using System.IO;
using System.Threading.Tasks;
using LINQPad;

namespace DR.Common.AWS.S3.LinqPad
{
    public interface ILinqPadS3Service
    {
        Task<string> UploadFileAsync(string path, string key = null);
        Task<string> UploadFileAsync(FileInfo fileInfo, string key = null);
        Task<string> DownloadFileAsync(string key, string path);
        Task<FileInfo> DownloadFileAsync(string key, DirectoryInfo directoryInfo);
        Hyperlinq GetPreSignedUrlAsHyperlinq(string key, TimeSpan? timeToLive = null);
    }
}