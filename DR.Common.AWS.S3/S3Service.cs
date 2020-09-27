using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace DR.Common.AWS.S3
{
    public class S3Service : IS3Service
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Client;

        public S3Service(S3ServiceOptions options): 
            this(options.BucketName,
                options.Key,
                options.Secret,
                options.RegionEndpoint,
                options.UseAccelerateEndpoint) {  }

        public S3Service(string bucketName, string key, string secret,
            RegionEndpoint region, bool useAccelerateEndpoint = false)
        {
            _bucketName = bucketName;
            _s3Client = new AmazonS3Client(key,secret, new AmazonS3Config
            {
                RegionEndpoint = region,
                UseAccelerateEndpoint = useAccelerateEndpoint
            });
        }

        public async Task<string> UploadFileAsync(string path, string key = null,
            EventHandler<UploadProgressArgs> progressHandler = null) => 
            await UploadFileAsync(new FileInfo(path), key, progressHandler);

        public async Task<string> UploadFileAsync(FileInfo fileInfo, string key = null,
            EventHandler<UploadProgressArgs> progressHandler = null)
        {
            key ??= fileInfo.Name;
            
            using var fileTransferUtility = new TransferUtility(_s3Client);

            var req = new TransferUtilityUploadRequest
            {
                FilePath = fileInfo.FullName,
                BucketName = _bucketName,
                Key = key
            };

            if (progressHandler != null)
                req.UploadProgressEvent += progressHandler;

            await fileTransferUtility.UploadAsync(req);
            return key;
        }

        public async Task<string> DownloadFileAsync(string key, string path,
            EventHandler<WriteObjectProgressArgs> progressHandler = null)
        {
            var res = await DownloadFileAsync(key, new DirectoryInfo(path), progressHandler);
            return res.FullName;
        }

        public async Task<FileInfo> DownloadFileAsync(string key, DirectoryInfo directoryInfo,
            EventHandler<WriteObjectProgressArgs> progressHandler = null)
        {
            using var fileTransferUtility = new TransferUtility(_s3Client);
            var filePath = Path.Combine(directoryInfo.FullName, key);
            var req = new TransferUtilityDownloadRequest()
            {
                FilePath = filePath,
                BucketName = _bucketName,
                Key = key
            };

            if (progressHandler != null)
                req.WriteObjectProgressEvent += progressHandler;

            await fileTransferUtility.DownloadAsync(req);

            return new FileInfo(filePath);
        }

        public string GetPreSignedUrl(string key, TimeSpan? timeToLive = null)
        {
            timeToLive ??= TimeSpan.FromDays(1);
            var req = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.UtcNow.Add(timeToLive.Value)
            };
            return _s3Client.GetPreSignedURL(req);
        }
    }
}
