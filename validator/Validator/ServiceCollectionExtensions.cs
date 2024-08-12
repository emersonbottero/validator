using System.Reflection;

public static class ServiceCollectionExtensions
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        // Load all assemblies in the current AppDomain
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies) RegisterValidatorsFromAssembly(services, assembly);
    }

    private static void RegisterValidatorsFromAssembly(IServiceCollection services, Assembly assembly)
    {
        // Get all types from the assembly
        var types = assembly.GetTypes();

        // Find all types that inherit from ValidatorBase<T>
        var validatorTypes = types
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Implementation = t, Interface = i })
            .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == typeof(Validator<>))
            .GroupBy(x => x.Implementation)
            .Select(g => g.First())
            .ToList();

        // Register the validators in the DI container
        foreach (var validatorType in validatorTypes) services.AddTransient(validatorType.Interface, validatorType.Implementation);
    }
}
