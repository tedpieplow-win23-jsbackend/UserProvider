using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserProvider.Contexts;
using UserProvider.Factories;
using UserProvider.Repositories;
using UserProvider.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<UserFactory>();

        services.AddDbContext<DataContext>(x => x.UseSqlServer(context.Configuration.GetConnectionString("Database")));
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
