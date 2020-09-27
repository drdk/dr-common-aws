using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using LINQPad;

namespace DR.Common.AWS.S3.LinqPad
{
    public class LinqPadS3Service : S3Service, ILinqPadS3Service
    {
        private readonly DumpContainer _dumpContainer;

        public LinqPadS3Service(S3ServiceOptions options) :
            base(options)
        {
            _dumpContainer = new DumpContainer().Dump($"S3 Service for {options.BucketName}");
        }

        public LinqPadS3Service(string bucketName, string key, string secret,
                RegionEndpoint region, bool useAccelerateEndpoint = false) :
            base(bucketName, key, secret, region, useAccelerateEndpoint)
        {
            _dumpContainer = new DumpContainer().Dump($"S3 Service for {bucketName}");
        }

        public async Task<string> UploadFileAsync(string path, string key = null)
        {
            return await this.UploadFileAsync(new FileInfo(path), key);
        }

        public async Task<string> UploadFileAsync(FileInfo fileInfo, string key = null)
        {
            key = await base.UploadFileAsync(fileInfo, key, DisplayUploadProgress);
            GetPreSignedUrlAsHyperlinq(key).Dump($"Uploaded {fileInfo.Name}");
            return key;
        }

        public async Task<string> DownloadFileAsync(string key, string path)
        {
            var res = await this.DownloadFileAsync(key, new DirectoryInfo(path));
            return res.FullName;
        }

        public async Task<FileInfo> DownloadFileAsync(string key, DirectoryInfo directoryInfo)
        {
            var res = await base.DownloadFileAsync(key, directoryInfo, DisplayDownloadProgress);
            new Hyperlinq(res.FullName).Dump($"Downloaded {key}");
            return res;
        }

        private void DisplayUploadProgress(object sender, UploadProgressArgs args)
        {
            _dumpContainer.Content = $"Uploading: {args}";
        }

        private void DisplayDownloadProgress(object sender, WriteObjectProgressArgs args)
        {
            _dumpContainer.Content = $"Downloading: {args}";
        }

        public Hyperlinq GetPreSignedUrlAsHyperlinq(string key, TimeSpan? timeToLive = null) =>
            new Hyperlinq(base.GetPreSignedUrl(key, timeToLive));
    }
}
