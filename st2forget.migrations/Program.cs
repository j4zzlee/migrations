using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using st2forget.console.utils;
using st2forget.utils.commands;
using st2forget.utils.sql;

namespace st2forget.migrations
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var provider = Init();
            using (var scope = provider.CreateScope())
            {
                ICommand runner;
                if (args == null || args.Length <= 0)
                {
                    runner = scope.ServiceProvider
                        .GetService<HelpListCommand>();
                    runner.Execute();
                    return;
                }

                var command = args[0];
                if (command.Equals("--help"))
                {
                    runner = scope.ServiceProvider
                        .GetService<HelpListCommand>();
                    runner.Execute();
                    return;
                }

                runner = scope.ServiceProvider
                    .GetServices<ICommand>()
                    .FirstOrDefault(r => r.CommandName.Equals(command));
                if (runner == null)
                {
                    $"Command {{f:Green}}{command}{{f:d}} is not supported".PrettyPrint(ConsoleColor.Red);
                    runner = scope.ServiceProvider
                        .GetService<HelpListCommand>();
                    runner.Execute();
                    return;
                }
                var hasError = false;
                try
                {
                    runner
                        .ReadArguments(args)
                        .Execute();
                }
                catch (MissingFieldException ex)
                {
                    ex.Message.PrettyPrint(ConsoleColor.Red);
                    hasError = true;
                }
                catch (Exception ex)
                {
                    ex.ToString().PrettyPrint(ConsoleColor.Red);
                    hasError = true;
                }

                if (hasError)
                {
                    runner.Help();
                }
            }
        }


        private static IServiceProvider Init()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(
                    AppContext.BaseDirectory.Substring(
                        0,
                        AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal)
                    )
                )
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<ConnectionSettings>(connectionSettings =>
            {
                connectionSettings.ConnectionString = configuration.GetConnectionString("MigrationDatabase");
            });

            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IMigrationExecuter, SqlMigrationExecuter>();
            services.AddScoped<ICommand, MigrateUpCommand>();
            services.AddScoped<ICommand, MigrateDownCommand>();
            services.AddScoped<ICommand, GenerateMigrationCommand>();
            services.AddScoped(provider => new HelpListCommand(provider.GetServices<ICommand>().Where(c => !c.CommandName.Equals("commands:list")).ToList()));

            return services.BuildServiceProvider();
        }
    }
}