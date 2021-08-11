using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace JF
{
	/// <summary>
	/// Classe de méthodes statiques utiles autour du XML
	/// </summary>
	public class XmlTools
	{
		#region Variables statiques privées
		#endregion

		#region Méthodes statiques publiques
		#region Formatage du XML
		/// <summary>
		/// Formate dans une chaîne un DOM XML
		/// </summary>
		/// <param name="o">DOM XML</param>
		/// <returns>Chaîne contenant le texte représentant le DOM XML formatté</returns>
		public static string Format(XmlDocument o)
		{
			return XmlTools.Format(o, ' ', 4);
		}
		/// <summary>
		/// Formate dans une chaîne un DOM XML
		/// </summary>
		/// <param name="o">DOM XML</param>
		/// <param name="cIdentChar">Caractère à utiliser pour l'indentation</param>
		/// <param name="nIdentation">Nombre de caractères à insérer pour chaque indentation</param>
		/// <returns>Chaîne contenant le texte représentant le DOM XML formatté</returns>
		public static string Format(XmlDocument o, char cIdentChar, int nIdentation)
		{
			// On le met en forme pour l'affichage
			StringWriter sw = new StringWriter();
			XmlTextWriter w = new XmlTextWriter(sw);
			w.Indentation = nIdentation;
			w.IndentChar = cIdentChar;
			w.Formatting = Formatting.Indented;
			o.Save(w);
			return sw.ToString();
		}
		#endregion
		#region Transformation XSLT
		/// <summary>
		/// Transforme un document XML avec une feuille de style XSLT
		/// </summary>
		/// <param name="oXml">
		/// Document XML, l'interface IXPathNavigable est implémentée par les classes 
		/// XmlNode et XPathDocument
		/// </param>
		/// <param name="oXsl">Feuille de style XSLT</param>
		/// <returns>Un stream contenant le résultat de la transformation</returns>
		public static Stream TransformXsl(IXPathNavigable oXml, XslCompiledTransform oXsl)
		{
			MemoryStream st = new MemoryStream();
			oXsl.Transform(oXml, null, st);
			st.Position = 0;
			return st;
		}
		/// <summary>
		/// Transforme un document XML avec une feuille de style XSLT
		/// </summary>
		/// <param name="oXml">
		/// Document XML, l'interface IXPathNavigable est implémentée par les classes 
		/// XmlNode et XPathDocument
		/// </param>
		/// <param name="sXsl">Chemin de la feuille de style XSLT à utiliser</param>
		/// <returns>Un stream contenant le résultat de la transformation</returns>
		public static Stream TransformXsl(IXPathNavigable oXml, string sXsl)
		{
			XsltSettings oSettings = new XsltSettings(true, false);
			XslCompiledTransform oXsl = new XslCompiledTransform();
			oXsl.Load(sXsl, oSettings, new XmlUrlResolver());
			MemoryStream st = new MemoryStream();
			oXsl.Transform(oXml, null, st);
			st.Position = 0;
			return st;
		}
		/// <summary>
		/// Transforme un document XML avec une feuille de style XSLT et des arguments
		/// </summary>
		/// <param name="oXml">
		/// Document XML, l'interface IXPathNavigable est implémentée par les classes 
		/// XmlNode et XPathDocument
		/// </param>
		/// <param name="oXsl">Feuille de style XSLT</param>
		/// <param name="oArgs">Liste des arguments à passer</param>
		/// <returns>Un stream contenant le résultat de la transformation</returns>
		public static Stream TransformXsl(IXPathNavigable oXml, XslCompiledTransform oXsl, XsltArgumentList oArgs)
		{
			MemoryStream st = new MemoryStream();
			oXsl.Transform(oXml, oArgs, st);
			st.Position = 0;
			return st;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xr"></param>
		/// <param name="oXsl"></param>
		/// <returns></returns>
		public static Stream TransformXsl(XmlReader xr, XslCompiledTransform oXsl)
		{
			MemoryStream st = new MemoryStream();
			oXsl.Transform(xr, null, st);
			st.Position = 0;
			return st;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xr"></param>
		/// <param name="sXsl"></param>
		/// <returns></returns>
		public static Stream TransformXsl(XmlReader xr, string sXsl)
		{
			XsltSettings oSettings = new XsltSettings(true, false);
			XslCompiledTransform oXsl = new XslCompiledTransform();
			oXsl.Load(sXsl, oSettings, new XmlUrlResolver());
			MemoryStream st = new MemoryStream();
			oXsl.Transform(xr, null, st);
			st.Position = 0;
			return st;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xr"></param>
		/// <param name="oXsl"></param>
		/// <param name="oArgs"></param>
		/// <returns></returns>
		public static Stream TransformXsl(XmlReader xr, XslCompiledTransform oXsl, XsltArgumentList oArgs)
		{
			MemoryStream st = new MemoryStream();
			oXsl.Transform(xr, oArgs, st);
			st.Position = 0;
			return st;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xr"></param>
		/// <param name="oXsl"></param>
		/// <param name="st"></param>
		public static void TransformXsl(XmlReader xr, XslCompiledTransform oXsl, Stream st)
		{
			oXsl.Transform(xr, null, st);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xr"></param>
		/// <param name="sXsl"></param>
		/// <param name="st"></param>
		public static void TransformXsl(XmlReader xr, string sXsl, Stream st)
		{
			XsltSettings oSettings = new XsltSettings(true, false);
			XslCompiledTransform oXsl = new XslCompiledTransform();
			oXsl.Load(sXsl, oSettings, new XmlUrlResolver());
			oXsl.Transform(xr, null, st);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xr"></param>
		/// <param name="oXsl"></param>
		/// <param name="oArgs"></param>
		/// <param name="st"></param>
		public static void TransformXsl(XmlReader xr, XslCompiledTransform oXsl, XsltArgumentList oArgs, Stream st)
		{
			oXsl.Transform(xr, oArgs, st);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xr"></param>
		/// <param name="sXsl"></param>
		/// <param name="oArgs"></param>
		/// <param name="st"></param>
		public static void TransformXsl(XmlReader xr, string sXsl, XsltArgumentList oArgs, Stream st)
		{
			XsltSettings oSettings = new XsltSettings(true, false);
			XslCompiledTransform oXsl = new XslCompiledTransform();
			oXsl.Load(sXsl, oSettings, new XmlUrlResolver());
			oXsl.Transform(xr, oArgs, st);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		/// <param name="oXsl"></param>
		/// <returns></returns>
		public static Stream TransformXsl(XElement el, XslCompiledTransform oXsl)
		{
			MemoryStream st = new MemoryStream();
			oXsl.Transform(el.CreateReader(), null, st);
			st.Position = 0;
			return st;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		/// <param name="oXsl"></param>
		/// <param name="oArgs"></param>
		/// <returns></returns>
		public static Stream TransformXsl(XElement el, XslCompiledTransform oXsl, XsltArgumentList oArgs)
		{
			MemoryStream st = new MemoryStream();
			oXsl.Transform(el.CreateReader(), oArgs, st);
			st.Position = 0;
			return st;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		/// <param name="oXsl"></param>
		/// <param name="st"></param>
		public static void TransformXsl(XElement el, XslCompiledTransform oXsl, Stream st)
		{
			oXsl.Transform(el.CreateReader(), null, st);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		/// <param name="sXsl"></param>
		/// <param name="st"></param>
		public static void TransformXsl(XElement el, string sXsl, Stream st)
		{
			XsltSettings oSettings = new XsltSettings(true, false);
			XslCompiledTransform oXsl = new XslCompiledTransform();
			oXsl.Load(sXsl, oSettings, new XmlUrlResolver());
			oXsl.Transform(el.CreateReader(), null, st);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		/// <param name="oXsl"></param>
		/// <param name="oArgs"></param>
		/// <param name="st"></param>
		public static void TransformXsl(XElement el, XslCompiledTransform oXsl, XsltArgumentList oArgs, Stream st)
		{
			oXsl.Transform(el.CreateReader(), oArgs, st);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="el"></param>
		/// <param name="sXsl"></param>
		/// <param name="oArgs"></param>
		/// <param name="st"></param>
		public static void TransformXsl(XElement el, string sXsl, XsltArgumentList oArgs, Stream st)
		{
			XsltSettings oSettings = new XsltSettings(true, false);
			XslCompiledTransform oXsl = new XslCompiledTransform();
			oXsl.Load(sXsl, oSettings, new XmlUrlResolver());
			oXsl.Transform(el.CreateReader(), oArgs, st);
		}
		#endregion
		#region Validation XML avec un schéma XSD
		/// <summary>
		/// Valide un document XML avec un schéma XSD
		/// </summary>
		/// <param name="oSrc">DOM XML à valider</param>
		/// <param name="oXss">Schéma à utiliser</param>
		/// <returns>true si le fichier est valide et false dans le cas contraire</returns>
		public static bool ValidateXml(XmlDocument oSrc, XmlSchemaSet oXss)
		{
			oSrc.Schemas = oXss;
			bool bValid = true;
			ValidationEventHandler validator = delegate (object sender, ValidationEventArgs e)
			{
				bValid = false;
			};
			oSrc.Validate(validator);
			return bValid;
		}
		/// <summary>
		/// Valide un document XML avec un schéma XSD et retourne la description des erreurs
		/// </summary>
		/// <param name="oSrc">DOM XML à valider</param>
		/// <param name="oXss">Schéma à utiliser</param>
		/// <param name="aErr">Tableau de classe XmlValidationError</param>
		/// <returns>true si le fichier est valide et false dans le cas contraire</returns>
		public static bool ValidateXml(XmlDocument oSrc, XmlSchemaSet oXss, out List<XmlValidationError> aErr)
		{
			// Liste des erreurs à retourner, la liste est vide s'il n'y a pas d'erreur
			List<XmlValidationError> aRet = new List<XmlValidationError>();
			// Au depart on considère le document valide
			bool bValid = true;
			// Delegate inline appelé pour chaque erreur
			ValidationEventHandler validator = delegate (object sender, ValidationEventArgs e)
			{
				// On récupère la ligne et la colonne de l'erreur
				IXmlLineInfo li = (IXmlLineInfo)sender;
				// Le document n'est pas valide
				bValid = false;
				// On ajoute l'erreur à la liste
				aRet.Add(new XmlValidationError(e, li.LineNumber, li.LinePosition));
			};
			// On crée un environement pour un XmlReader
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.Schemas = oXss;
			settings.ValidationType = ValidationType.Schema;
			settings.ValidationEventHandler += validator;
			// Stream en mémoire
			Stream st = new MemoryStream();
			// On sauve le document dans le stream
			oSrc.Save(st);
			st.Position = 0;
			// On crée un XmlReader
			XmlReader reader = XmlReader.Create(st, settings);
			// On valide le document
			while (reader.Read()) ;
			// On retourne le résultat
			aErr = aRet;
			return bValid;
		}
		/// <summary>
		/// Valide un document XML avec un schéma XSD
		/// </summary>
		/// <param name="sSrc">Chemin du fichier XML à valider</param>
		/// <param name="oXss">Schéma à utiliser</param>
		/// <returns>true si le fichier est valide et false dans le cas contraire</returns>
		public static bool ValidateXml(string sSrc, XmlSchemaSet oXss)
		{
			bool bValid = true;
			ValidationEventHandler validator = delegate (object sender, ValidationEventArgs e)
			{
				bValid = false;
			};
			XmlReaderSettings xrs = new XmlReaderSettings();
			xrs.ValidationType = ValidationType.Schema;
			xrs.Schemas = oXss;
			xrs.ValidationEventHandler += validator;
			XmlReader reader = XmlReader.Create(sSrc, xrs);
			while (reader.Read()) ;
			return bValid;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="elXml"></param>
		/// <param name="oXss"></param>
		/// <returns></returns>
		public static bool ValidateXml(XElement elXml, XmlSchemaSet oXss)
		{
			//XmlDocument x = new XmlDocument();
			//x.Load(elXml.CreateReader());
			//return ValidateXml(x, oXss);
			bool bValid = true;
			ValidationEventHandler validator = delegate (object sender, ValidationEventArgs e)
			{
				bValid = false;
			};
			XmlReaderSettings xrs = new XmlReaderSettings();
			xrs.ValidationType = ValidationType.Schema;
			xrs.Schemas = oXss;
			//xrs.ValidationFlags |= XmlSchemaValidationFlags.AllowXmlAttributes;
			xrs.ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints;
			xrs.ValidationEventHandler += validator;
			try
			{
				//elXml.Document.Validate(oXss, validator);
				using (XmlReader reader = XmlReader.Create(elXml.CreateReader(), xrs))
				{
					while (reader.Read()) ;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return bValid;
		}
		/// <summary>
		/// Valide un document XML avec un schéma XSD et retourne la description des erreurs
		/// </summary>
		/// <param name="oSrc">DOM XML à valider</param>
		/// <param name="oXss">Schéma à utiliser</param>
		/// <param name="aErr">Tableau de classe XmlValidationError</param>
		/// <returns>true si le fichier est valide et false dans le cas contraire</returns>
		public static bool ValidateXml(XElement elDoc, XmlSchemaSet oXss, out List<XmlValidationError> aErr)
		{
			// Liste des erreurs à retourner, la liste est vide s'il n'y a pas d'erreur
			List<XmlValidationError> aRet = new List<XmlValidationError>();
			// Au depart on considère le document valide
			bool bValid = true;
			// Delegate inline appelé pour chaque erreur
			ValidationEventHandler validator = delegate (object sender, ValidationEventArgs e)
			{
				// On récupère la ligne et la colonne de l'erreur
				IXmlLineInfo li = (IXmlLineInfo)sender;
				// Le document n'est pas valide
				bValid = false;
				// On ajoute l'erreur à la liste
				aRet.Add(new XmlValidationError(e, li.LineNumber, li.LinePosition));
			};
			// On crée un environement pour un XmlWriter
			XmlWriterSettings xws = new XmlWriterSettings();
			xws.Indent = true;  // Si on ne met pas cette option les numéros de lines sont toujours à 1 ????
			xws.NewLineChars = "\n";
			xws.NewLineHandling = NewLineHandling.Replace;
			// Stream en mémoire
			Stream st = new MemoryStream();
			// On sauve le document dans le stream
			using (XmlWriter xw = XmlWriter.Create(st, xws))
			{
				elDoc.Save(xw);
			}
			st.Position = 0;
			// On crée un environement pour un XmlReader
			XmlReaderSettings xrs = new XmlReaderSettings();
			xrs.Schemas = oXss;
			xrs.ValidationType = ValidationType.Schema;
			xrs.ValidationEventHandler += validator;
			// On crée un XmlReader
			XmlReader reader = XmlReader.Create(st, xrs);
			// On valide le document
			while (reader.Read()) ;
			// On retourne le résultat
			aErr = aRet;
			return bValid;
		}
		#endregion
		#endregion
	}
	/// <summary>
	/// Classe qui contient la description d'une erreur de validation
	/// </summary>
	public class XmlValidationError
	{
		#region Variables privées
		private ValidationEventArgs _va;
		#endregion

		#region Propriétés publiques
		/// <summary>
		/// 
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Severity { get; set; }
		/// <summary>
		/// Ligne de l'erreur
		/// </summary>
		public int Line { get; set; }
		/// <summary>
		/// Colonne de l'erreur
		/// </summary>
		public int Column { get; set; }
		#endregion

		#region Constructeurs
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="severity"></param>
		public XmlValidationError(string message, string severity)
		{
			_va = null;
			Message = message;
			Severity = severity;
			Line = 0;
			Column = 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="va"></param>
		public XmlValidationError(ValidationEventArgs va)
		{
			_va = va;
			Message = va.Message;
			Severity = va.Severity.ToString();
			Line = 0;
			Column = 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="va"></param>
		/// <param name="line"></param>
		/// <param name="col"></param>
		public XmlValidationError(ValidationEventArgs va, int line, int col)
		{
			_va = va;
			Message = va.Message;
			Severity = va.Severity.ToString();
			Line = line;
			Column = col;
		}
		#endregion

		#region Méthode surclassée
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (_va == null)
				return $"[{Severity}] {Message}";
			else
				return $"[{Line},{Column}][{_va.Severity}] {_va.Message}";
		}
		#endregion
	}
}
