using RabbitMQ.Client;

namespace JF.RabbitMQ
{
	/// <summary>
	/// 
	/// </summary>
	public class Context
	{
		#region Constantes
		#endregion

		#region Variables privées
		#endregion

		#region Propriétés publiques
		/// <summary>
		/// La connexion
		/// </summary>
		public IConnection Connection { get; set; }
		/// <summary>
		/// Le channel
		/// </summary>
		public IModel Channel { get; set; }
		#endregion

		#region Constructeurs
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cnx"></param>
		/// <param name="channel"></param>
		public Context(IConnection cnx, IModel channel)
		{
			Init(cnx, channel);
		}
		#endregion

		#region Méthodes privées
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cnx"></param>
		/// <param name="channel"></param>
		private void Init(IConnection cnx, IModel channel)
		{
			Connection = cnx;
			Channel = channel;
		}
		#endregion

		#region Méthodes publiques
		#endregion

		#region Méthodes statiques
		#endregion

		#region Méthodes surclassées
		#endregion
	}
}
