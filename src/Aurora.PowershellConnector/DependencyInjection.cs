namespace Microsoft.Extensions.DependencyInjection;

using Aurora.PowershellConnector.Interfaces;
using Aurora.PowershellConnector.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddPowershellConnector(this IServiceCollection services)
    {
        services.AddScoped<ITeamsPowershellConnector, TeamsPowershellConnector>();

        return services;
    }
}