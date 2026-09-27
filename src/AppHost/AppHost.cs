namespace TenantAuth.AppHost;

internal static class Program
{
    private static void Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres");
        IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identitydb");
        IResourceBuilder<RedisResource> redis = builder.AddRedis("redis");

        builder.Build().Run();
    }
}