namespace TenantAuth.AppHost;

internal static class Program
{
    private static void Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
            .WithDataVolume();
        
        IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identitydb");
        
        IResourceBuilder<RedisResource> redis = builder.AddRedis("redis");

        IResourceBuilder<ProjectResource> identity = builder.AddProject<Projects.TenantAuth_Identity>("identity")
            .WithReference(identityDb)
            .WaitFor(identityDb);

        builder.Build().Run();
    }
}