using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Extensions;

public static class DependencyInjectionExtension
{
    public static void AutowireServices<T>(this IServiceCollection services)
    {
        Type abstractType = typeof(T);
        RegisterServices(services, abstractType);
    }

    public static void AutowireScopedServices(this IServiceCollection services)
    {
        Type abstractType = typeof(IScopedService);
        RegisterServices(services, abstractType);
    }

    #region UtilMethods

    private static void RegisterServices(IServiceCollection services, Type abstractType, Func<Type, Type, Type> makeGenericTypeFn = null)
    {
        // Obter todas as classes que implementam a classe abstrata
        List<Type> classes = FindClassesDerivedFrom(abstractType);

        // Imprimir as classes encontradas
        foreach (Type serviceType in classes)
        {
            Type baseType = serviceType.BaseType;
            Type interfaceBaseType = serviceType.GetInterface(abstractType.Name);

            if (baseType != null && baseType.IsGenericType && makeGenericTypeFn != null)
            {
                Type tServiceType = makeGenericTypeFn(baseType, abstractType);

                // Registre o serviço no contêiner de injeção de dependência
                RegisterScopedService(services, serviceType, tServiceType);
            }
            else if (interfaceBaseType != null && interfaceBaseType.IsGenericType && makeGenericTypeFn != null)
            {
                Type tServiceType = makeGenericTypeFn(interfaceBaseType, abstractType);

                // Registre o serviço no contêiner de injeção de dependência
                RegisterScopedService(services, serviceType, tServiceType);
            }
            else
            {
                RegisterSingleScopedService(services, serviceType);
            }
        }
    }

    private static void RegisterScopedService(IServiceCollection services, Type serviceType, Type tServiceType)
    {
        // Registre o serviço usando AddScoped com os tipos gerados
        services.AddScoped(tServiceType, serviceType);
        services.AddScoped(serviceType);

        Console.WriteLine($"Service registered: {serviceType.Name} -> {tServiceType.Name}");
    }

    private static void RegisterSingleScopedService(IServiceCollection services, Type serviceType)
    {
        services.AddScoped(serviceType);

        Console.WriteLine($"Service registered: {serviceType.Name}");
    }

    private static List<Type> FindClassesDerivedFrom(Type abstractType)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        List<Type> implementingClasses = [];

        // Obter todos os tipos do assembly
        Type[] types = assembly.GetTypes();

        // Verificar cada tipo no assembly
        foreach (Type type in types)
        {
            // Verificar se o tipo é uma classe concreta (não é abstrata ou interface)
            if (type.IsClass && !type.IsAbstract)
            {
                // Verificar se o tipo é uma subclasse da classe abstrata genérica ou Interface
                if ((type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == abstractType)
                    || type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == abstractType)
                    || abstractType.IsAssignableFrom(type)
                )
                {
                    implementingClasses.Add(type);
                }
            }
        }

        return implementingClasses;
    }

    #endregion
}
