using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Interfaces;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Services;
using SalaryCalculatorApp.Strategies;
using System;

namespace SalaryCalculatorApp.Utils
{
    /// <summary>
    /// Provides extension methods for configuring and building the host.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Creates and configures an <see cref="IHostBuilder"/> for the application.
        /// </summary>
        /// <param name="args">Command-line arguments for configuring the host.</param>
        /// <returns>An <see cref="IHostBuilder"/> instance configured for the application.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Load configuration from a JSON file.
                    config.AddJsonFile("config.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Configure services and settings for dependency injection.
                    ConfigureServices(context.Configuration, services);
                });

        /// <summary>
        /// Configures services and settings for dependency injection.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="services">The service collection for dependency injection.</param>
        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            // Register IConfiguration for dependency injection.
            services.AddSingleton<IConfiguration>(configuration);

            // Configure and register settings classes for dependency injection.
            services.Configure<IncomeTaxConfig>(configuration.GetSection("IncomeTax"));
            services.Configure<MedicareLevyConfig>(configuration.GetSection("MedicareLevy"));
            services.Configure<BudgetRepairLevyConfig>(configuration.GetSection("BudgetRepairLevy"));
            services.Configure<SalarySettingsConfig>(configuration.GetSection("SalarySettings"));


            // Register strategy classes for dependency injection.
            services.AddSingleton<IncomeTaxStrategy>();
            services.AddSingleton<MedicareLevyStrategy>();
            services.AddSingleton<BudgetRepairLevyStrategy>();

            // Register the salary calculator service with its dependencies.
            services.AddSingleton<ISalaryCalculator>(sp => new SalaryCalculator(
                sp.GetRequiredService<IncomeTaxStrategy>(),
                sp.GetRequiredService<MedicareLevyStrategy>(),
                sp.GetRequiredService<BudgetRepairLevyStrategy>(),
                sp.GetRequiredService<IOptions<SalarySettingsConfig>>() 
            ));
        }
    }
}
