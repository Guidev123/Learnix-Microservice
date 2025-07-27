﻿using Microsoft.Extensions.Options;
using Quartz;

namespace Users.Infrastructure.Inbox
{
    internal sealed class ConfigureProcessInboxJob(IOptions<InboxOptions> options)
        : IConfigureOptions<QuartzOptions>
    {
        private readonly InboxOptions _inboxOptions = options.Value;

        public void Configure(QuartzOptions options)
        {
            var jobName = typeof(ProcessInboxJob).FullName!;

            options
                .AddJob<ProcessInboxJob>(configure => configure.WithIdentity(jobName))
                .AddTrigger(configure => configure.ForJob(jobName)
                .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(_inboxOptions.IntervalInSeconds)
                .RepeatForever()));
        }
    }
}