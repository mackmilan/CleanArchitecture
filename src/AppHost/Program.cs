using CleanArchitecture.Shared;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureContainerAppEnvironment("aca-env");

#if (UsePostgreSQL)
var databaseServer = builder
    .AddAzurePostgresFlexibleServer(Services.DatabaseServer)
    .WithPasswordAuthentication()
    .RunAsContainer(container => 
        container.WithLifetime(ContainerLifetime.Persistent))
    .AddDatabase(Services.Database);
#elif (UseSqlServer)
var databaseServer = builder
    .AddAzureSqlServer(Services.DatabaseServer)
    .RunAsContainer(container => 
        container.WithLifetime(ContainerLifetime.Persistent))
    .AddDatabase(Services.Database);
#else
var databaseServer = builder
    .AddSqlite(Services.Database);
#endif

var web = builder.AddProject<Projects.Web>(Services.WebApi)
    .WithReference(databaseServer)
    .WaitFor(databaseServer)
    .WithExternalHttpEndpoints()
    .WithAspNetCoreEnvironment()
    .WithUrlForEndpoint("http", url =>
    {
#if (UseBlazor)
        url.DisplayText = "Blazor frontend";
        url.Url = "/";
#else
        url.DisplayText = "Scalar API Reference";
        url.Url = "/scalar";
#endif
    });

#if (!UseApiOnly)
#if (!UseBlazor)
if (builder.ExecutionContext.IsRunMode)
{
    builder.AddJavaScriptApp(Services.WebFrontend, "./../Web/ClientApp")
        .WithRunScript("start")
        .WithReference(web)
        .WaitFor(web)
        .WithHttpEndpoint(env: "PORT")
        .WithExternalHttpEndpoints();
}
#endif
#endif

builder.Build().Run();
