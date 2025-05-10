using Core.Lib.EventHandler.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Lib.EventHandler.Extensions;

public static class EventListenerRegistrationExtensions
{
    public static IServiceCollection AddEventListener<TListener>(
        this IServiceCollection services,
        Action<EventListenerOptions> configure)
        where TListener : class, IEventListener
    {
        services.AddTransient<TListener>();

        services.AddSingleton<IEventListenerRegistration>(sp =>
        {
            var listener = sp.GetRequiredService<TListener>();
            return new EventListenerRegistration<TListener>(listener, configure);
        });

        return services;
    }
}
