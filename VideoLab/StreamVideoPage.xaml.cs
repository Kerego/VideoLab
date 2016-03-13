using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Streaming.Adaptive;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VideoLab
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StreamVideoPage : Page
	{
		DispatcherTimer _timer;
		public StreamVideoPage()
		{
			this.InitializeComponent();
			Loaded += PageLoaded;
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			AdaptiveMediaSourceCreationResult result = null;
			try
			{
				result = await AdaptiveMediaSource.CreateFromUriAsync(new Uri(UrlBox.Text));
				VideoElement.SetMediaStreamSource(result.MediaSource);
			}
			catch
			{
				await Message();
				return;
			}
		}

		async Task Message()
		{
			var message = new MessageDialog("Could not found the video");
			await message.ShowAsync();
		}

		private void PageLoaded(object sender, RoutedEventArgs e)
		{
			VideoElement.MediaOpened += (s, args) =>
			{
				Progress.Maximum = VideoElement.NaturalDuration.TimeSpan.TotalSeconds;
				DurationLabel.Text = VideoElement.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
			};

			Progress.Minimum = 0;
			Progress.IsThumbToolTipEnabled = false;
			_timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.2) };
			_timer.Tick += UpdateVideoPosition;
			_timer.Start();

			SystemNavigationManager.GetForCurrentView().BackRequested += NavigateBackRequested;
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
				Frame.CanGoBack ?
				AppViewBackButtonVisibility.Visible :
				AppViewBackButtonVisibility.Collapsed;
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			VideoElement.Stop();
			_timer.Stop();
			_timer.Tick -= UpdateVideoPosition;
			SystemNavigationManager.GetForCurrentView().BackRequested -= NavigateBackRequested;
		}

		private void NavigateBackRequested(object sender, BackRequestedEventArgs e)
		{
			if (Frame.CanGoBack)
				Frame.GoBack();
		}

		private void UpdateVideoPosition(object sender, object e)
		{
			Progress.Value = VideoElement.Position.TotalSeconds;
			CurrentProgressPositionLabel.Text = VideoElement.Position.ToString(@"hh\:mm\:ss");
		}

		private void ControlElementClicked(object sender, RoutedEventArgs e)
		{
			FrameworkElement senderElement = sender as FrameworkElement;
			Flyout flyout = FlyoutBase.GetAttachedFlyout(senderElement) as Flyout;
			flyout.Placement = FlyoutPlacementMode.Top;
			flyout.ShowAt(senderElement);
		}
		private void PlaybackSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{
			if(VideoElement != null)
				VideoElement.PlaybackRate = (float)e.NewValue;
		}

		private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{
			if (VideoElement != null)
				VideoElement.Volume = (float)e.NewValue;
		}
		private void Progress_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
		{
			if (Math.Abs(VideoElement.Position.TotalSeconds - e.NewValue) < 0.5)
				return;
			if (VideoElement != null)
				VideoElement.Position = TimeSpan.FromSeconds(e.NewValue);
		}
		private void PlayStopButtonClick(object sender, RoutedEventArgs e)
		{
			if (PlayButton.IsChecked.Value)
			{
				VideoElement.Play();
				PlayButton.Content = "\ue103";
			}
			else
			{
				VideoElement.Pause();
				PlayButton.Content = "\ue102";
			}
		}

		private void VideoElementDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
		{
			if (ApplicationView.GetForCurrentView().IsFullScreenMode)
				ApplicationView.GetForCurrentView().ExitFullScreenMode();
			else
				ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
			CommandPanel.Visibility = (CommandPanel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
		}

		private void VideoElementTapped(object sender, TappedRoutedEventArgs e)
		{
			CommandPanel.Visibility = (CommandPanel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
		}

	}
}
