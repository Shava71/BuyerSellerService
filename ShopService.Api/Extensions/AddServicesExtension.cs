using System.Data;
using Microsoft.Extensions.Options;
using ShopService.Application.Services.Implementations;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Repositories;
using ShopService.Infrastructure.Data.DbConnection;
using ShopService.Infrastructure.Data.DbConnection.Implementation;
using ShopService.Infrastructure.Repositories;
using ShopService.Infrastructure.Stats;

namespace ShopService.Api.Extensions;

/// <summary>
/// Добавление DI зависимостей
/// </summary>
/// todo: переписать под раздельные классы
public static class AddServicesExtension 
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IOptionsMonitorCache<StatsConfig>, OptionsCache<StatsConfig>>();
        services.AddSingleton<IOptionsFactory<StatsConfig>, OptionsFactory<StatsConfig>>();

        // Factory
        services.AddScoped<IDbConncetionFactory, DapperConnectionFactory>();
        
        
        // Репозитории (DAL)
        services.AddScoped<IItemsRepository, ItemsRepository>();

        // Сервисы (BLL)
        services.AddScoped<IStatsService, StatsService>();
        services.AddScoped<ISellerService, SellerService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<IMetricService, MetricService>();
        
        return services;
    }
} // это вообще кто-нибудь смотрит, я ж не зря всё это выделяю стараюсь o_O