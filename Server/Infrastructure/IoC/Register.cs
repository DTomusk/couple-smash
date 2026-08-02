using Application.Interface;
using Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC;

public static class Register
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DATABASE_URL")));

        services.AddScoped<IPairingRepo, PairingRepo>();
        services.AddScoped<IMemberRepo, MemberRepo>();
    }
}
