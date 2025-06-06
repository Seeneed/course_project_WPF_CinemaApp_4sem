using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;

namespace CinemaMOON.Views
{
	public partial class AboutUsPage : Page
	{
		public AboutUsPage()
		{
			InitializeComponent();
			this.Loaded += AboutUsPage_Loaded;
		}

		private async void AboutUsPage_Loaded(object sender, RoutedEventArgs e)
		{
			await InitializeWebViewAsync();
		}

		private async Task InitializeWebViewAsync()
		{
			try
			{
				await webView.EnsureCoreWebView2Async(null);

				if (webView.CoreWebView2 == null)
				{
					ShowMapError(
						GetStringResource("AboutUsPage_ErrorWebViewInitFail") ?? "Failed to initialize map component (CoreWebView2). Please ensure the WebView2 Runtime is installed.",
						MessageBoxImage.Warning);
					return;
				}

				string mapEmbedUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2351.3194896984946!2d27.551179877332498!3d53.890525133866866!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x46dbcfdc1184543b%3A0x5b0e0e6c4cf33dad!2sGalileo%20Mall!5e0!3m2!1snl!2snl!4v1744542689928!5m2!1snl!2snl";
				string htmlTitle = GetStringResource("AboutUsPage_HtmlMapTitle") ?? "Map";
				string iframeFallback = GetStringResource("AboutUsPage_HtmlIframeFallback") ?? "Your browser does not support iframes.";

				string htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>{htmlTitle}</title>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        html, body {{ margin: 0; padding: 0; height: 100%; overflow: hidden; }}
                        iframe {{ display: block; width: 100%; height: 100%; border: none; }}
                    </style>
                </head>
                <body>
                    <iframe src='{mapEmbedUrl}' allowfullscreen='' loading='lazy' referrerpolicy='no-referrer-when-downgrade'>
                        {iframeFallback}
                    </iframe>
                </body>
                </html>";

				webView.CoreWebView2.NavigateToString(htmlContent);

			}
			catch (Exception ex)
			{
				string prefix = GetStringResource("AboutUsPage_ErrorMapLoadFailPrefix") ?? "Error loading map:";
				string suffix = GetStringResource("AboutUsPage_ErrorMapLoadFailSuffix") ?? "Please ensure the WebView2 Runtime is installed and you have an internet connection.";
				ShowMapError($"{prefix} {ex.Message}\n\n{suffix}", MessageBoxImage.Error);
			}
		}

		private void ShowMapError(string message, MessageBoxImage icon)
		{
			string title = GetStringResource("AboutUsPage_ErrorMapTitle") ?? "Map Error";
			MessageBox.Show(message, title, MessageBoxButton.OK, icon);
		}

		private string GetStringResource(string key)
		{
			object resource = Application.Current.TryFindResource(key);
			return resource as string;
		}
	}
}