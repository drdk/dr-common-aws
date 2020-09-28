
cd "DR.Common.AWS.S3.Test"
dotnet user-secrets set S3Client:BucketName (vault kv get -field=bucketname dr-common-aws/main)
dotnet user-secrets set S3Client:Key (vault kv get -field=key dr-common-aws/main)
dotnet user-secrets set S3Client:Secret (vault kv get -field=secret dr-common-aws/main)
dotnet user-secrets set S3Client:RegionEndpoint (vault kv get -field=region-endpoint dr-common-aws/main)
cd ..