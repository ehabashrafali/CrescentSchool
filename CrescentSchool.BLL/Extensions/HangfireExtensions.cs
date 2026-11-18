
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
        var UpcomingSessionsOptions = configuration.GetSection("UpcommingSessionsOptions").Get<UpcommingSessionsOptions>();
        if (UpcomingSessionsOptions is null)
            throw new ArgumentNullException(nameof(UpcomingSessionsOptions), "UpcommingSessionsOptions configuration section is missing or invalid.");

        CreateMonthlyUpcommingSessions(builder, UpcomingSessionsOptions);
        ProcessZoomWebhookInbox(builder);
        SettleUpcomingSessions(builder);
    }

    private static void ProcessZoomWebhookInbox(IApplicationBuilder builder)
    {
        var recurringJobManager = builder.ApplicationServices.GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<ProcessZoomWebhookInboxJob>(ProcessZoomWebhookInboxJob.Name,
                                                      job => job.Execute(CancellationToken.None),
                                                      Cron.Daily());
    }

    private static void CreateMonthlyUpcommingSessions(IApplicationBuilder builder, UpcommingSessionsOptions configuration)
    {
        var recurringJobManager = builder.ApplicationServices.GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<CreateUpcomingSessionsJob>(CreateUpcomingSessionsJob.Name,
                                                      job => job.Execute(null, CancellationToken.None),
                                                      configuration.CronExpression);
    }

    private static void SettleUpcomingSessions(IApplicationBuilder builder)
    {
        var recurringJobManager = builder.ApplicationServices.GetRequiredService<IRecurringJobManager>();
        recurringJobManager.AddOrUpdate<SettleUpcomingSessionsJob>(SettleUpcomingSessionsJob.Name,
                                                      job => job.Execute(null, CancellationToken.None),
                                                     Cron.Daily());
    }

}
