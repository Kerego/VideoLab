using System;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VideoLab
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CameraVideoPage : Page
	{
		MediaCapture _capture;
		public CameraVideoPage()
		{
			this.InitializeComponent();
			Loaded += PageLoaded;
		}

		private async void PageLoaded(object sender, RoutedEventArgs e)
		{
			_capture = new MediaCapture();
			await _capture.InitializeAsync();
			PreviewControl.Source = _capture;
			await _capture.StartPreviewAsync();
			SystemNavigationManager.GetForCurrentView().BackRequested += NavigateBackRequested;
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
				Frame.CanGoBack ?
				AppViewBackButtonVisibility.Visible :
				AppViewBackButtonVisibility.Collapsed;
		}

		protected override async void OnNavigatedFrom(NavigationEventArgs e)
		{
			if(CaptureButton.IsChecked.Value)
			{
				await _capture.StopPreviewAsync();
				await _capture.StopRecordAsync();
			}
			else
				await _capture.StopPreviewAsync();
			_capture.Dispose();
			SystemNavigationManager.GetForCurrentView().BackRequested -= NavigateBackRequested;
		}

		private void NavigateBackRequested(object sender, BackRequestedEventArgs e)
		{
			if (Frame.CanGoBack)
				Frame.GoBack();
		}

		private async void CaptureClick(object sender, RoutedEventArgs e)
		{
			if (CaptureButton.IsChecked.Value)
			{
				var file = await KnownFolders.CameraRoll.CreateFileAsync("Capture.mp4", CreationCollisionOption.GenerateUniqueName);
				await _capture.StartRecordToStorageFileAsync(MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto), file);
			}
			else
			{
				await _capture.StopRecordAsync();
			}

		}
	}
}
