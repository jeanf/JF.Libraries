using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using JF.RabbitMQ;
using JF.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

var config = new ConfigurationBuilder()
	   .SetBasePath(Directory.GetCurrentDirectory())
	   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	   .Build();

var logger = new LoggerConfiguration()
						.ReadFrom.Configuration(config)
						.CreateLogger();

Log.Logger = logger;

//var serviceCollection = new ServiceCollection()
//				.AddLogging(builder => builder.AddSerilog(
//					new LoggerConfiguration()
//						.ReadFrom.Configuration(config)
//						.CreateLogger()))
//				.BuildServiceProvider();

//var logger = serviceCollection.GetService<ILogger<Program>>();

logger.Error("Error       : Hello depuis, Serilog!");
logger.Warning("Warning     : Hello depuis, Serilog!");
logger.Information("Information : Hello depuis, Serilog!");
logger.Debug("Debug       : Hello depuis, Serilog!");


var cnxstr = config["ConnectionString:ProgTv"];
var strcnx = config.GetSection("ConnectionString").GetSection("ProgTv").Value;

logger.Information($"cnxstr = {cnxstr}");
logger.Information($"strcnx = {strcnx}");

var t = new Test();

var cl = new ClientServer("TstInput", "40.69.35.140", "odf", "Palm5ouV");
//var cl = new ClientServer("TstInput", "localhost", "odf", "Palm5ouV");
var el = XElement.Load(@"filezilla.xml");
//cl.SendXmlDir(el);
//logger.Information("Fichier envoyé");
//cl.SendXmlDir(el);
//logger.Information("Fichier envoyé");
//cl.SendXmlDir(el);
//logger.Information("Fichier envoyé");

var dir = @"c:\odf\input";
if (!Directory.Exists(dir))
{
	Directory.CreateDirectory(dir);
	logger.Information($"le répertoire [{dir}] est créé");
}
cl.Open();
while ((el = cl.ReadXmlDir()) != null)
{
	var nom = $@"{dir}\{DateTime.Now.ToString("yyyyMMddHHmmssfffff")}.xml";
	el.Save(nom);
	logger.Information($"fichier [{nom}] reçu");
}
cl.Close();

logger.Information("Fin");
Console.ReadLine();
