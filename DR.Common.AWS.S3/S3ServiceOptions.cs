using Amazon;

namespace DR.Common.AWS.S3
{
    public class S3ServiceOptions
    {
        public string BucketName { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public RegionEndpoint RegionEndpoint { get; set; } = RegionEndpoint.EUWest1;
        public bool UseAccelerateEndpoint { get; set; } = false;
    }
}
