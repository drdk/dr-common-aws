using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using NUnit.Framework;

namespace DR.Common.AWS.S3.Test
{
    public class Tests
    {

        private IS3Service _service;
        private AmazonS3Client _client;

        [SetUp]
        public async Task Setup()
        {
            _service = new S3Service(Startup.S3ServiceOptions);
            _client = new AmazonS3Client(
                Startup.S3ServiceOptions.Key,
                Startup.S3ServiceOptions.Secret,
                 new AmazonS3Config
                {
                    RegionEndpoint = Startup.S3ServiceOptions.RegionEndpoint,
                    UseAccelerateEndpoint = false
                });
            await EmptyBucket();
        }

        private async Task EmptyBucket()
        {
            var res = await _client.ListObjectsAsync(Startup.S3ServiceOptions.BucketName);
            foreach (var obj in res.S3Objects)
            {
                await _client.DeleteObjectAsync(obj.BucketName, obj.Key);
            }
        }

        [TearDown]
        public async Task TearDown()
        {
            await EmptyBucket();
        }

        [TestCase("hello.txt")]
        public async Task Test(string fileName)
        {
            var randomName = Path.GetRandomFileName();
            var key = await _service.UploadFileAsync(fileName, randomName);
            Assert.AreEqual(key,randomName);
            var res = await _service.DownloadFileAsync(key, Path.GetTempPath());
            Assert.IsTrue(File.Exists(res));
            var url = _service.GetPreSignedUrl(key);
            var fileContent = await File.ReadAllTextAsync(fileName);
            var webContent = await new WebClient().DownloadStringTaskAsync(url);
            Assert.AreEqual(fileContent,webContent);
        }
    }
}