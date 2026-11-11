
using CrescentSchool.BLL.Extensions;
using CrescentSchool.BLL.Jobs;
using CrescentSchool.Core.Extensions;
using CrescentSchool.Core.Options;
using Hangfire;
using Hangfire.Console;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrescentSchool.BLL.Extensions;

public static class HangfireExtensions
{
    public static void AddHangfire(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSerilogLogProvider()
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage()
            .UseConsole());

        services.AddHangfireServer();
    }

    public static async Task AddHangfireJobsAsync(this IApplicationBuilder builder, IConfiguration configuration)
    {
        var monthlyUpcommingSessionsOptions = configuration.GetSection("MonthlyUpcommingSessionsOptions").Get<MonthlyUpcommingSessionsOptions>();
        if (monthlyUpcommingSessionsOptions is null)
            throw new ArgumentNullException(nameof(monthlyUpcommingSessionsOptions), "MonthlyUpcommingSessionsOptions configuration section is missing or invalid.");

        CreateMonthlyUpcommingSessions(builder, monthlyUpcommingSessionsOptions);
    }

    private static void CreateMonthlyUpcommingSessions(IApplicationBuilder builder, MonthlyUpcommingSessionsOptions configuration)
    {
        var recurringJobManager = builder.ApplicationServices.GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<CreateUpcommingSessionsJob>(CreateUpcommingSessionsJob.Name,
                                                      job => job.Execute(null, CancellationToken.None),
                                                      configuration.CronExpression);
    }
}
