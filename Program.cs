using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;

namespace AargonTools
{
    public class Program
    {
        //for reading the appsettings.json strings 
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();


        public static void Main(string[] args)
        {
            //this connection string is used for log
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //for adding a additional column userName
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn("UserName",SqlDbType.VarChar)
                }
            };


            //this is the log controller
            //Log.Logger = new LoggerConfiguration()
            //    .Enrich.FromLogContext()
            //    .WriteTo.MSSqlServer(connectionString,
            //        sinkOptions: new SinkOptions { TableName = "WebApiLogs" }
            //        , null, null, LogEventLevel.Information, null, columnOptions: columnOptions, null, null)
            //    .WriteTo.Seq("http://localhost:5341") // Add this line to configure Seq
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            //    .CreateLogger();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // Set global minimum log level
                .Enrich.FromLogContext()
                .WriteTo.MSSqlServer(connectionString,
                    sinkOptions: new SinkOptions { TableName = "WebApiLogs" },
                    restrictedToMinimumLevel: LogEventLevel.Information, // Minimum level for SQL Server sink
                    columnOptions: columnOptions)
                .WriteTo.Seq("http://localhost:5341", restrictedToMinimumLevel: LogEventLevel.Debug) // Minimum level for Seq
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error) // Override for Microsoft logs
                .CreateLogger();



            //build the api after everything


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:8090");
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
        //using the Serilog
    }
}
