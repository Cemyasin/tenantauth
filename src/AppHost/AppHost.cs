var builder = DistributedApplication.CreateBuilder(args);

// Backing services run as containers, never as managed Azure services (brief section 2.1).
// AddPostgres/AddRedis stay containers under `azd up` too; AddAzure* would provision paid resources.
var postgres = builder.AddPostgres("postgres");
var identityDb = postgres.AddDatabase("identitydb");

var cache = builder.AddRedis("cache");

// Identity: OpenIddict server + admin panel. Owns the database; Redis holds shared Data Protection keys.
var identity = builder.AddProject<Projects.TenantAuth_Identity>("identity")
    .WithReference(identityDb)
    .WaitFor(identityDb)
    .WithReference(cache)
    .WaitFor(cache)
    .WithExternalHttpEndpoints();

// Api: protected resource server. Only Web calls it, so it gets no public endpoint.
var api = builder.AddProject<Projects.TenantAuth_Api>("api")
    .WithReference(identity)
    .WaitFor(identity);

// Web: demo client behind the BFF. Redis will hold its authentication tickets.
builder.AddProject<Projects.TenantAuth_Web>("web")
    .WithReference(identity)
    .WithReference(api)
    .WithReference(cache)
    .WaitFor(identity)
    .WaitFor(api)
    .WaitFor(cache)
    .WithExternalHttpEndpoints();

builder.Build().Run();
