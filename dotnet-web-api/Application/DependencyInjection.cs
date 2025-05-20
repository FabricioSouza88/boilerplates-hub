using Application.Config;
using Application.Extensions;
using Application.Interfaces;
using Application.Services;
using Application.Services.Notification;
using CoreKit.EventHandler.Extensions;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StarterApp.Application;
using static Application.Constants.AppConstants;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("SampleDatabase"));

        // Add Configurations
        // -> Configure Authorozation Rules
        services.ConfigureAuthorization();

        // Add EventHanler
        services.AddEventHandler();

        // Dependency Injection Definitions
        services.AddAutoMapper(typeof(Default).Assembly);
        services.AutowireScopedServices();
        services.AddScopedServices();

        // Add Event Listeners
        services.AddEventListeners();
    }

    public static void AddEventListeners(this IServiceCollection services)
    {
        services.AddEventListener<NotificationEventListener>(options => { options.Processes = new[] { ProcessName.Sample }; });
    }

    public static void AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<ISampleService, SampleService>();
    }
}
