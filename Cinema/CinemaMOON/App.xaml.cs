using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using CinemaMOON.Data;
using CinemaMOON.ViewModels;
using CinemaMOON.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaMOON
{
    public partial class App : Application
	{
		private static List<CultureInfo> m_Languages = new List<CultureInfo>();
		public static List<CultureInfo> Languages => m_Languages;
		public static event EventHandler LanguageChanged;
		private static ResourceDictionary _currentLanguageDictionary = null;
		private static ResourceDictionary _currentThemeDictionary = null;
		private static bool _isDarkTheme = true;

		public static IServiceProvider ServiceProvider { get; private set; }

		public App()
		{
			if (m_Languages.Count == 0)
			{
				m_Languages.Add(new CultureInfo("en-US"));
				m_Languages.Add(new CultureInfo("ru-RU"));
			}

			ConfigureServices();
		}

		private void ConfigureServices()
		{
			var services = new ServiceCollection();

			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CinemaMOON;Trusted_Connection=True;TrustServerCertificate=True;"));

			ServiceProvider = services.BuildServiceProvider();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			FindInitialDictionaries();

			string defaultCultureName = "ru-RU";
			if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name != defaultCultureName)
			{
				System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(defaultCultureName);
			}

			try
			{
				var dbContext = ServiceProvider.GetRequiredService<AppDbContext>();

				var mainWindowViewModel = new MainWindowViewModel(dbContext);

				var mainWindow = new MainWindow
				{
					DataContext = mainWindowViewModel
				};
				mainWindow.Show();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Critical error on startup: {ex.Message}\n\n{ex.StackTrace}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private static void FindInitialDictionaries()
		{
			_currentLanguageDictionary = Application.Current.Resources.MergedDictionaries
				.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Dictionary"));

			_currentThemeDictionary = Application.Current.Resources.MergedDictionaries
			   .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));

			_isDarkTheme = _currentThemeDictionary?.Source.OriginalString.Contains("DarkTheme") ?? true;
		}

		public static CultureInfo Language
		{
			get => System.Threading.Thread.CurrentThread.CurrentUICulture;
			set
			{
				if (value == null) throw new ArgumentNullException(nameof(value));
				if (value == System.Threading.Thread.CurrentThread.CurrentUICulture && _currentLanguageDictionary != null) return;

				System.Threading.Thread.CurrentThread.CurrentUICulture = value;

				ResourceDictionary newLangDict = new ResourceDictionary();
				string langPath;

				switch (value.Name)
				{
					case "ru-RU":
						langPath = "Dictionaries/RussianDictionary.xaml";
						break;
					default:
						langPath = "Dictionaries/EnglishDictionary.xaml";
						break;
				}
				newLangDict.Source = new Uri(langPath, UriKind.Relative);

				ResourceDictionary oldLangDictToRemove = _currentLanguageDictionary;

				if (oldLangDictToRemove != null && Application.Current.Resources.MergedDictionaries.Contains(oldLangDictToRemove))
				{
					int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldLangDictToRemove);
					Application.Current.Resources.MergedDictionaries.Remove(oldLangDictToRemove);
					Application.Current.Resources.MergedDictionaries.Insert(ind, newLangDict);
				}
				else
				{
					string oldLangPath = _currentLanguageDictionary?.Source?.OriginalString;
					oldLangDictToRemove = null;
					if (!string.IsNullOrEmpty(oldLangPath))
					{
						oldLangDictToRemove = Application.Current.Resources.MergedDictionaries
												.FirstOrDefault(d => d.Source?.OriginalString.Equals(oldLangPath, StringComparison.OrdinalIgnoreCase) ?? false);
					}

					if (oldLangDictToRemove != null)
					{
						int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldLangDictToRemove);
						Application.Current.Resources.MergedDictionaries.Remove(oldLangDictToRemove);
						Application.Current.Resources.MergedDictionaries.Insert(ind, newLangDict);
					}
					else
					{
						Application.Current.Resources.MergedDictionaries.Add(newLangDict);
					}
				}
				_currentLanguageDictionary = newLangDict;
				LanguageChanged?.Invoke(Application.Current, new EventArgs());
			}
		}

		public static event EventHandler ThemeChanged;

		private const string DarkThemePath = "Resources/Themes/DarkTheme.xaml";
		private const string BlueThemePath = "Resources/Themes/BlueTheme.xaml";

		public static Dictionary<string, Uri> AvailableThemes { get; } = new Dictionary<string, Uri>
		{
			{ "Dark", new Uri(DarkThemePath, UriKind.Relative) },
			{ "Blue", new Uri(BlueThemePath, UriKind.Relative) }
		};

		public static void ToggleTheme()
		{
			Uri nextThemeUri = _isDarkTheme ? AvailableThemes["Blue"] : AvailableThemes["Dark"];
			SwitchThemeDictionary(nextThemeUri);
			_isDarkTheme = !_isDarkTheme;
			ThemeChanged?.Invoke(Application.Current, EventArgs.Empty);
		}

		private static void SwitchThemeDictionary(Uri themeUri)
		{
			if (themeUri == null) throw new ArgumentNullException(nameof(themeUri));
			try
			{
				ResourceDictionary newThemeDict = new ResourceDictionary { Source = themeUri };
				ResourceDictionary oldThemeDictToRemove = _currentThemeDictionary;

				if (oldThemeDictToRemove != null && Application.Current.Resources.MergedDictionaries.Contains(oldThemeDictToRemove))
				{
					int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldThemeDictToRemove);
					Application.Current.Resources.MergedDictionaries.Remove(oldThemeDictToRemove);
					Application.Current.Resources.MergedDictionaries.Insert(ind, newThemeDict);
				}
				else
				{
					string oldThemePath = _currentThemeDictionary?.Source?.OriginalString;
					oldThemeDictToRemove = null;
					if (!string.IsNullOrEmpty(oldThemePath))
					{
						oldThemeDictToRemove = Application.Current.Resources.MergedDictionaries
												  .FirstOrDefault(d => d.Source?.OriginalString.Equals(oldThemePath, StringComparison.OrdinalIgnoreCase) ?? false);
					}

					if (oldThemeDictToRemove != null)
					{
						int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldThemeDictToRemove);
						Application.Current.Resources.MergedDictionaries.Remove(oldThemeDictToRemove);
						Application.Current.Resources.MergedDictionaries.Insert(ind, newThemeDict);
					}
					else
					{
						Application.Current.Resources.MergedDictionaries.Add(newThemeDict);
					}
				}

				_currentThemeDictionary = newThemeDict;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки файла темы: {themeUri.OriginalString}\n{ex.Message}", "Ошибка темы", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}