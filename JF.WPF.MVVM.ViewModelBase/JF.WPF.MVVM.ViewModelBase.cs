using System.Reflection;

namespace JF.WPF.MVVM
{
	public class ViewModelBase : CommandSink
	{
		private string _titreapp;

		public string TitreApp
		{
			get
			{
				return _titreapp;
			}
			set
			{
				if (_titreapp != value)
				{
					_titreapp = value;
					RaisePropertyChanged("TitreApp");
				}
			}
		}

		protected string GenerateAppTitle(string title)
		{
			Version version = Assembly.GetCallingAssembly().GetName().Version;
			return $"{title} v{version.Major}.{version.Minor}.{version.Build}";
		}
	}
}
