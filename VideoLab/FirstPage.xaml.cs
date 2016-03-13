using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
	public sealed partial class FirstPage : Page
	{
		public FirstPage()
		{
			this.InitializeComponent();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
				Frame.CanGoBack ?
				AppViewBackButtonVisibility.Visible :
				AppViewBackButtonVisibility.Collapsed;
		}


		private void FileButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(FileVideoPage));
		}

		private void StreamButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(StreamVideoPage));
		}

		private void CameraButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(CameraVideoPage));
		}
	}
}
