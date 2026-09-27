namespace TenantAuth.AppHost;

internal static class Program
{
    private static void Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("TenantAuthPostgres");
        IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("TenantIdentityDb");
        IResourceBuilder<RedisResource> redis = builder.AddRedis("TenantAuthRedis");

        builder.Build().Run();
    }
}