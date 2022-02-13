using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using GodelTech.CodeReview.Evaluator.Commands;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using GodelTech.CodeReview.Evaluator.Services;
using GodelTech.CodeReview.Evaluator.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator
{
    // 1. Add manifest and filter validators
    public class Program
    {
        public static int Main(string[] args)
        {
            using var container = CreateServiceProvider();

            var parser = new Parser(x =>
            {
                x.HelpWriter = TextWriter.Null;
            });

            var result = parser.ParseArguments<EvaluateOptions, ImportFileDetailsOptions, ImportIssuesOptions, NewManifestOptions, NewFilterOptions, ExtractMetadataOptions>(args);

            var exitCode = result
                .MapResult(
                    (EvaluateOptions x) => ProcessEvaluateAsync(x, container).GetAwaiter().GetResult(),
                    (ImportIssuesOptions x) => ProcessImportIssuesAsync(x, container).GetAwaiter().GetResult(),
                    (ImportFileDetailsOptions x) => ProcessImportFileDetailsAsync(x, container).GetAwaiter().GetResult(),
                    (NewManifestOptions x) => ProcessNewManifestAsync(x, container).GetAwaiter().GetResult(),
                    (NewFilterOptions x) => ProcessNewFilterAsync(x, container).GetAwaiter().GetResult(),
                    (ExtractMetadataOptions x) => ProcessExtractOptionsAsync(x, container).GetAwaiter().GetResult(),
                    _ => ProcessErrors(result));

            return exitCode;
        }

        private static Task<int> ProcessImportFileDetailsAsync(ImportFileDetailsOptions options, ServiceProvider container)
        {
            return container.GetRequiredService<IImportFileDetailsCommand>().ExecuteAsync(options);
        }

        private static Task<int> ProcessImportIssuesAsync(ImportIssuesOptions options, ServiceProvider container)
        {
            return container.GetRequiredService<IImportIssuesCommand>().ExecuteAsync(options);
        }

        private static Task<int> ProcessExtractOptionsAsync(ExtractMetadataOptions options, IServiceProvider container)
        {
            return container.GetRequiredService<IExtractMetadataCommand>().ExecuteAsync(options);
        }

        private static Task<int> ProcessNewFilterAsync(NewFilterOptions options, IServiceProvider container)
        {
            return container.GetRequiredService<ICreateNewFilterCommand>().ExecuteAsync(options);
        }

        private static Task<int> ProcessNewManifestAsync(NewManifestOptions options, IServiceProvider container)
        {
            return container.GetRequiredService<ICreateNewManifestCommand>().ExecuteAsync(options);
        }

        private static Task<int> ProcessEvaluateAsync(EvaluateOptions options, IServiceProvider container)
        {
            return container.GetRequiredService<IEvaluateCommand>().ExecuteAsync(options);
        }

        private static int ProcessErrors(ParserResult<object> result)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);

            Console.WriteLine(helpText);

            return Constants.ErrorExitCode;
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var serviceProvider = new ServiceCollection();

            serviceProvider.AddLogging(x =>
            {
                x.ClearProviders();
                x.AddProvider(new SimplifiedConsoleLoggerProvider());
            });

            serviceProvider.AddSingleton<IFileService, FileService>();
            serviceProvider.AddSingleton<IYamlSerializer, YamlSerializer>();
            serviceProvider.AddSingleton<IInitScriptProvider, InitScriptProvider>();
            serviceProvider.AddSingleton<IJsonSerializer, JsonSerializer>();

            serviceProvider.AddTransient<IObjectValidator, ObjectValidator>();
            serviceProvider.AddTransient<IScopeManifestValidator, ScopeManifestValidator>();
            serviceProvider.AddTransient<IEvaluationManifestValidator, EvaluationManifestValidator>();
            serviceProvider.AddTransient<IOptionMetadataProvider, OptionMetadataProvider>();
            serviceProvider.AddTransient<ICommandLineOptionsTypeProvider, CommandLineOptionsTypeProvider>();
            
            serviceProvider.AddTransient<IDbRequestExecutorFactory, DbRequestExecutorFactory>();
            serviceProvider.AddTransient<IScopeManifestProvider, ScopeManifestProvider>();
            serviceProvider.AddTransient<ILocDetailsFilterFactory, LocDetailsFilterFactory>();
            serviceProvider.AddTransient<IFileLocDetailsProvider, FileLocDetailsProvider>();
            serviceProvider.AddTransient<IIssueService, IssueService>();
            serviceProvider.AddTransient<IIssueFilterFactory, IssueFilterFactory>();
            serviceProvider.AddTransient<IEvaluationService, EvaluationService>();
            serviceProvider.AddTransient<IDatabaseService, DatabaseService>();
            serviceProvider.AddTransient<IIssueProvider, IssueProvider>();
            
            serviceProvider.AddTransient<IExtractMetadataCommand, ExtractMetadataCommand>();
            serviceProvider.AddTransient<IEvaluateCommand, EvaluateCommand>();
            serviceProvider.AddTransient<IImportFileDetailsCommand, ImportFileDetailsCommand>();
            serviceProvider.AddTransient<IImportIssuesCommand, ImportIssuesCommand>();
            serviceProvider.AddTransient<ICreateNewFilterCommand, CreateNewFilterCommand>();
            serviceProvider.AddTransient<ICreateNewManifestCommand, CreateNewManifestCommand>();

            return serviceProvider.BuildServiceProvider();
        }
    }
}
