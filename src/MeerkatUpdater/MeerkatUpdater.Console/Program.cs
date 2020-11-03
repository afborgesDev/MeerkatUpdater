using MeerkatUpdater.Console.CliConfiguration;
using MeerkatUpdater.Console.Execution;
using MeerkatUpdater.Console.Options;
using MeerkatUpdater.Console.Options.InputOptions;
using MeerkatUpdater.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace MeerkatUpdater.Console
{
    internal static class Program
    {
        public static void Main(string[] args) =>
            BuildCommandLine()
                .UseHost(_ => Host.CreateDefaultBuilder(), host =>
                {
                    host.ConfigureServices(services =>
                    {
                        services.AddMeerkatUpdater();
                        services.AddScoped<IUpdateExecution, UpdateExecution>();
                    });
                })
                .UseDefaults()
                .Build()
                .Invoke(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var root = RootCommandBuilder.Build();

            root.Handler = CommandHandler.Create((YmlConfigInputOption ymlConfigInputOption, SolutionPathInputOption solutionPathInputOption,
                NugetSourcesOption nugetSourcesOption, AllowedVersionToUpdateOption allowedVersionToUpdateOption, IHost host) =>
            {
                var serviceProvider = host.Services;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                var config = ExecutionOptionsBuilder.StartBuild(loggerFactory.CreateLogger<ExecutionOptionsBuilder>())
                                                    .WithYmlConfig(ymlConfigInputOption)
                                                    .WithSolutionPathConfig(solutionPathInputOption)
                                                    .WithNugetSourcesConfig(nugetSourcesOption)
                                                    .WithAllowedVersionToUpdateConfig(allowedVersionToUpdateOption)
                                                    .Build();

                Run(config, serviceProvider);
            });

            return new CommandLineBuilder(root);
        }

        private static void Run(ExecutionOptions? executionOptions, IServiceProvider serviceProvider)
        {
            var updateExecution = serviceProvider.GetRequiredService<IUpdateExecution>();
            updateExecution.Execute(executionOptions);
        }
    }
}