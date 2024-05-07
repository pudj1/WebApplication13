using Quartz;

namespace WebApplication13.Services
{
    public class QuartzHostedService : IHostedService
{
    private readonly IScheduler _scheduler;
    private readonly JobSchedulerOptions _options;

    public QuartzHostedService(ISchedulerFactory schedulerFactory, JobSchedulerOptions options)
    {
        _options = options;
        _scheduler = schedulerFactory.GetScheduler().Result;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _scheduler.Start(cancellationToken);

        var job = JobBuilder.Create<BackupJob>()
            .WithIdentity("backup-job")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("backup-trigger")
            .WithCronSchedule(_options.CronExpression)
            .Build();

        await _scheduler.ScheduleJob(job, trigger, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduler.Shutdown(cancellationToken);
    }
}

public class BackupJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {

        try
        {
            string sourceFilePath = "website_checker_log.txt";
            string backupFilePath = $"{sourceFilePath}.bak";

            File.Copy(sourceFilePath, backupFilePath, true);
            Console.WriteLine($"Backup of {sourceFilePath} created at {backupFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create backup: {ex.Message}");
        }
    }
}

public class JobSchedulerOptions
{
    public string CronExpression { get; set; }
}
}
