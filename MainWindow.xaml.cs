// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;
using System.Text.Json;
using WinFM.Types;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinFM {
  /// <summary>
  /// An empty window that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainWindow : Window {

    // enables mica background
    IntPtr dispatchQueue = IntPtr.Zero;
    MicaController backdropController;
    SystemBackdropConfiguration micaConfig;

    UserDataResponse userData;

    public MainWindow() {
      this.InitializeComponent();

      // make sure the first tab (scrobbles) is selected
      mainNavigation.SelectedItem = mainNavigation.MenuItems.OfType<NavigationViewItem>().First();

      // activate the mica backdrop
      setMica();
      // and customize the title bar IF SUPPORTED
      IntPtr hWnd = WindowNative.GetWindowHandle(this);
      WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
      AppWindow appWindow = AppWindow.GetFromWindowId(wndId);

      if (AppWindowTitleBar.IsCustomizationSupported()) {
        var titleBar = appWindow.TitleBar;
        titleBar.ExtendsContentIntoTitleBar = true;
        titleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        // set button colours so they are transparent too
        titleBar.ButtonBackgroundColor = Colors.Transparent;
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
      }

      // load the user data 
      LoadUserData();

    }

    async void LoadUserData() {
      HttpClient client = new HttpClient();

      client.DefaultRequestHeaders.Add("User-Agent", "WinFM");

      string responseJson = await client.GetStringAsync("https://ws.audioscrobbler.com/2.0/?method=user.getinfo&user=jamesbmadden&format=json&api_key=" + Api.Key);

      userData = JsonSerializer.Deserialize<UserDataResponse>(responseJson);

      // set the profile picture and name to the loaded values
      UserName.Text = userData.user.realname;
      BitmapImage pfpImage = new BitmapImage();
      pfpImage.UriSource = new Uri(userData.user.image[1].text);
      ProfilePicture.ProfilePicture = pfpImage;

    }

    // on navigation event from the NavView
    public void Navigated (NavigationView navView, NavigationViewSelectionChangedEventArgs args) {

      var selectedComponent = (NavigationViewItem) args.SelectedItem;
      var selectedTag = (string) selectedComponent.Tag;
      
      switch (selectedTag) {
        case "ScrobblesPage":
          pageFrame.Navigate(typeof(Pages.ScrobblesPage)); break;
        case "ReportsPage":
          pageFrame.Navigate(typeof(Pages.ReportsPage)); break;
        case "ChartsPage":
          pageFrame.Navigate(typeof(Pages.ChartsPage)); break;
      }

    }

    // attempts to set the mica backdrop
    bool setMica() {

      // make sure there is a dispatch queue setup
      createDispatchQueue();

      if (MicaController.IsSupported()) {

        micaConfig = new SystemBackdropConfiguration();
        micaConfig.IsInputActive = true;

        backdropController = new MicaController();

        backdropController.AddSystemBackdropTarget(this.As<ICompositionSupportsSystemBackdrop>());
        backdropController.SetSystemBackdropConfiguration(micaConfig);
        
        return true;
      }
      else {
        // no mica support, return false :(
        return false;
      }

    }

    // struct for the options for dispatch queue
    [StructLayout(LayoutKind.Sequential)]
    struct DispatchQueueOptions {
      internal int dwSize;
      internal int threadType;
      internal int apartmentType;
    }

    [DllImport("CoreMessaging.dll")]
    private static unsafe extern int CreateDispatcherQueueController(DispatchQueueOptions options, IntPtr* instance);

    // creates a dispatch queue, a necessary feature for mica support
    void createDispatchQueue() {
      if (Windows.System.DispatcherQueue.GetForCurrentThread() != null) {
        // one is already set, so no need for a new one
        return;
      }

      if (dispatchQueue == IntPtr.Zero) {
        DispatchQueueOptions options;
        options.dwSize = Marshal.SizeOf(typeof(DispatchQueueOptions));
        options.threadType = 2;
        options.apartmentType = 2;

        unsafe {
          IntPtr _dispatchQueue;

          CreateDispatcherQueueController(options, &_dispatchQueue);

          dispatchQueue = _dispatchQueue;
        };
        
      }
    }

  }
}
