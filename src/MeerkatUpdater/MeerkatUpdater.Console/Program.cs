using MeerkatUpdater.Console.Options;
using MeerkatUpdater.Console.Options.InputOptions;
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
                        //Place holder for service injection
                    });
                })
                .UseDefaults()
                .Build()
                .Invoke(args);

        private static CommandLineBuilder BuildCommandLine()
        {
            var root = new RootCommand()
            {
                YmlConfigInputOption.YmlConfigOption,
                SolutionPathInputOption.SolutionPathConfigOption
            };

            root.Handler = CommandHandler.Create((YmlConfigInputOption ymlConfigInputOption, SolutionPathInputOption solutionPathInputOption, IHost host) =>
            {
                var serviceProvider = host.Services;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                var config = ExecutionOptionsBuilder.StartBuild(loggerFactory.CreateLogger<ExecutionOptionsBuilder>())
                                                    .WithYmlConfig(ymlConfigInputOption)
                                                    .WithSolutionPathConfig(solutionPathInputOption)
                                                    .Build();

                Run(config, serviceProvider);
            });

            return new CommandLineBuilder(root);
        }

        private static void Run(ExecutionOptions? executionOptions, IServiceProvider serviceProvider)
        {
        }
    }
}