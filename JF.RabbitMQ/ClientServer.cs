using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Xml.Linq;
using Serilog;

namespace JF.RabbitMQ
{
	/// <summary>
	/// 
	/// </summary>
	public class ClientServer
	{
		#region Constantes
		#endregion

		#region Variables privées
		protected ILogger logger = Log.Logger;
		#endregion

		#region Propriétés publiques
		/// <summary>
		/// Nom de la queue
		/// </summary>
		public string NomQueue { get; set; }
		/// <summary>
		/// Serveur sur lequel est installé RabbitMQ
		/// </summary>
		public string Host { get; set; }
		/// <summary>
		/// Nom de l'utilisateur à utiliser
		/// </summary>
		public string User { get; set; }
		/// <summary>
		/// Mot de passe associé à l'utilisateur
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// Contexte de la connexion
		/// </summary>
		public Context Contexte { get; set; }
		#endregion

		#region Constructeurs
		/// <summary>
		/// 
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="host"></param>
		/// <param name="user"></param>
		/// <param name="pass"></param>
		public ClientServer(string queue, string host, string user, string pass)
		{
			Init(queue, host, user, pass);
		}
		#endregion

		#region Méthodes privées
		/// <summary>
		/// 
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="host"></param>
		/// <param name="user"></param>
		/// <param name="pass"></param>
		private void Init(string queue, string host, string user, string pass)
		{
			NomQueue = queue;
			Host = host;
			User = user;
			Password = pass;
			Contexte = null;
		}
		#endregion

		#region Méthodes publiques
		/// <summary>
		/// 
		/// </summary>
		public void Open()
		{
			string ret = String.Empty;
			var factory = new ConnectionFactory()
			{
				HostName = Host,
				UserName = User,
				Password = Password
			};
			var connection = factory.CreateConnection();
			var channel = connection.CreateModel();
			channel.QueueDeclare(NomQueue, true, false, false, null);
			channel.BasicQos(0, 1, false);

			Contexte = new Context(connection, channel);
		}
		/// <summary>
		/// 
		/// </summary>
		public void Close()
		{
			if (Contexte != null)
			{
				Contexte.Channel.Dispose();
				Contexte.Connection.Dispose();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public XElement ReadXmlDir()
		{
			XElement el = null;
			BasicGetResult result = Contexte.Channel.BasicGet(NomQueue, false);
			if (result != null)
			{
				// Récupérer un élément dans la queue RabbitMQ 
				byte[] body = result.Body.ToArray();
				ulong tag = result.DeliveryTag;
				if (body != null)
				{
					using (var ms = new MemoryStream(body))
					{
						el = XElement.Load(ms);
					}
				}
				// On retire l'élément de la queue
				Contexte.Channel.BasicAck(tag, false);
			}
			return el;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		public void SendXmlDir(XElement el)
		{
			//Contexte.Channel.QueueDeclare(NomQueue, true, false, false, null);
			//var body = Encoding.UTF8.GetBytes(el.ToString());
			//var properties = Contexte.Channel.CreateBasicProperties();
			//properties.Persistent = true;
			//Contexte.Channel.BasicPublish("", NomQueue, properties, body);
			try
			{
				var factory = new ConnectionFactory()
				{
					HostName = Host,
					UserName = User,
					Password = Password
				};
				using (var connection = factory.CreateConnection())
				{
					using (var channel = connection.CreateModel())
					{
						channel.QueueDeclare(NomQueue, true, false, false, null);
						var body = Encoding.UTF8.GetBytes(el.ToString());
						var properties = channel.CreateBasicProperties();
						properties.Persistent = true;
						channel.BasicPublish("", NomQueue, properties, body);
					}
				}
			}
			catch (Exception ex)
			{
				logger.Error("SendXmlDir : " + ex.Message);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmleater"></param>
		public (Task task, CancellationTokenSource cts) StartReadXml(string rabbitin, Func<XElement, XElement> xmleater)
		{
			logger.Information($"Démarrage du service");
			var count = 0;
			var lastnotified = DateTime.Now;
			var lastreceived = DateTime.Now;
			var cts = new CancellationTokenSource();
			var task = Task.Factory.StartNew(o =>
			{
				var a = rabbitin.Split('|');
				var queue = a[0];
				var host = a[1];
				var user = a[2];
				var pass = a[3];
				var token = (CancellationToken)o;
				ClientServer s = null;
				while (!token.IsCancellationRequested)
				{
					try
					{
						if (s == null)
						{
							s = new ClientServer(queue, host, user, pass);
							s.Open();
						}
						var ttw = 1000;
						var el = s.ReadXmlDir();
						if (el != null)
						{
							var ret = xmleater(el);
							count++;
							lastreceived = DateTime.Now;
							lastnotified = DateTime.Now;
							ttw = 1;
						}
						var x = DateTime.Now.Subtract(lastnotified);
						if (x.TotalMinutes > 1.0)
						{
							if (count == 0)
							{
								logger.Information("Aucun document reçu depuis le lancement du service");
							}
							else
							{
								var pluriel = (count > 1) ? "s" : String.Empty;
								var date = lastreceived.ToString("yyyy-MM-dd");
								var heure = lastreceived.ToString("HH:mm:ss.fff");
								var depuis = DateTime.Now.Subtract(lastreceived);
								logger.Debug($"{count} document{pluriel} reçu{pluriel}. Dernier document reçu le {date} à {heure} ({depuis})");
							}
							lastnotified = DateTime.Now;
						}
						Thread.Sleep(ttw);
					}
					catch (Exception ex)
					{
						s.Close();
						s = null;
						logger.Error($"Exception dans le thread de lecture RabbitMQ : {ex.Message}");
					}
				}
				if (s != null)
				{
					s.Close();
					s = null;
				}
				logger.Information("On quitte le thread consommateur");
			}, cts.Token);
			return (task, cts);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="task"></param>
		/// <param name="cts"></param>
		public void StopReadXml(Task task, CancellationTokenSource cts)
		{
			logger.Information($"Arrêt du service");
			try
			{
				cts.Cancel();
				task.Wait(500);
			}
			catch (Exception ex)
			{
				logger.Error($"Exception lors de l'arrêt du service : {ex.Message}");
			}
			finally
			{
				cts = null;
				task = null;
			}
			logger.Information($"Service arrêté");
		}
		#endregion

		#region Méthodes statiques
		#endregion

		#region Méthodes surclassées
		#endregion
	}
}
