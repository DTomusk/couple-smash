using Microsoft.Extensions.DependencyInjection;

namespace Application.IoC;

public static class Register
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPairingService, PairingService>();
        services.AddScoped<IMemberService, MemberService>();
    }
}
