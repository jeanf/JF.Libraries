using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml;

namespace JF.WPF.Controls
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class XmlBrowser : UserControl
	{
		#region Variables privées
		/// <summary>
		/// 
		/// </summary>
		private XslCompiledTransform m_oXsl;
		#endregion

		#region Constructeurs
		/// <summary>
		/// 
		/// </summary>
		public XmlBrowser()
		{
			InitializeComponent();
			Assembly assem = Assembly.GetExecutingAssembly();
			using (Stream st = assem.GetManifestResourceStream("JF.WPF.Controls.xml-pretty-print.xslt"))
			{
				this.m_oXsl = new XslCompiledTransform();
				using (XmlReader xr = XmlReader.Create(st))
				{
					this.m_oXsl.Load(xr);
				}
			}
		}
		#endregion

		#region Méthodes privées
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		/// <param name="oXsl"></param>
		/// <returns></returns>
		private Stream TransformXsl(XElement el, XslCompiledTransform oXsl)
		{
			MemoryStream st = new MemoryStream();
			oXsl.Transform(el.CreateReader(), null, st);
			st.Position = 0;
			return st;
		}
		#endregion

		#region Méthodes publiques
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		public void ShowXml(XElement el)
		{
			this.wb.NavigateToStream(TransformXsl(el, this.m_oXsl));
		}
		#endregion
	}
}