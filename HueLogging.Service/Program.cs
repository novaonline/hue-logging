using HueLogging.Standard.Library.Extensions;
using HueLogging.Standard.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using Topshelf;
using Topshelf.Quartz;
using AppSettings = HueLogging.Service.Properties.Settings;

namespace HueLogging.Service
{
	class Program
	{
		public static string exePath;
		public static int delayMilliseconds;
		public static string arguments;
		public static IHueLoggingManager hueLoggingManager;

		static void Main(string[] args)
		{
			try
			{
				// TODO: I think the problem is that these connections are stumbling upon each other, which is not ideal,
				// SO need to make an arch change or determine a new way to instantianze an IHueLoggingManager
				var services = new ServiceCollection();
				services.AddHueLogging(options =>
				{
					options.UseSqlServer(AppSettings.Default.HueLoggingDbConnection);
				});
				services.AddSingleton(new LoggerFactory()
				.AddConsole()
				.AddEventLog(new Microsoft.Extensions.Logging.EventLog.EventLogSettings() { SourceName = "HueLogging",
					Filter = (source, level) => (level >= LogLevel.Warning) }));
				var serviceProvider = services.BuildServiceProvider();
				//serviceProvider.GetService<HueLoggingContext>().Database.Migrate();
				hueLoggingManager = serviceProvider.GetService<IHueLoggingManager>();

				var rc = HostFactory.Run(x =>
				{
					x.Service<App>(s =>
					{
						s.ConstructUsing(name => new App(AppSettings.Default.ExePath, AppSettings.Default.Arguments, AppSettings.Default.DelayInSeconds));
						s.WhenStarted(app => app.Start());
						s.WhenStopped(app => app.Stop());
						s.ScheduleQuartzJob(q =>
						{
							q.WithJob(() => JobBuilder.Create<Job>().Build())
							.AddTrigger(() =>
								TriggerBuilder.Create().WithSimpleSchedule(b => 
									b.WithIntervalInSeconds(AppSettings.Default.DelayInSeconds).RepeatForever())
							.Build());
						});
					});
					x.RunAsLocalService();
					x.DependsOnEventLog();
					x.StartAutomatically();
					x.SetDescription("Polling the hue bridge to get the light states");
					x.SetDisplayName("Hue Logging Service");
					x.SetServiceName("HueLogging.Service");
				});

				var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
				Environment.ExitCode = exitCode;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
	}
}
