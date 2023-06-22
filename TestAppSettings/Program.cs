using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

var config = new ConfigurationBuilder()
	   .SetBasePath(Directory.GetCurrentDirectory())
	   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	   .Build();

var serviceCollection = new ServiceCollection()
				.AddLogging(builder => builder.AddSerilog(
					new LoggerConfiguration()
						.ReadFrom.Configuration(config)
						.CreateLogger()))
				.BuildServiceProvider();

var logger = serviceCollection.GetService<ILogger<Program>>();

logger.LogError("Error       : Hello depuis, Serilog!");
logger.LogWarning("Warning     : Hello depuis, Serilog!");
logger.LogInformation("Information : Hello depuis, Serilog!");
logger.LogDebug("Debug       : Hello depuis, Serilog!");
logger.LogCritical("Critical    : Hello depuis, Serilog!");
logger.LogTrace("Trace       : Hello depuis, Serilog!");


var cnxstr = config["ConnectionString:ProgTv"];
var strcnx = config.GetSection("ConnectionString").GetSection("ProgTv").Value;

logger.LogInformation($"cnxstr = {cnxstr}");
logger.LogInformation($"strcnx = {strcnx}");
Console.ReadLine();
