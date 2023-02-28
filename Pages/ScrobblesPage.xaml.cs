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
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinFM.Types;

namespace WinFM.Pages {

  public sealed partial class ScrobblesPage : Page {

    // data that can be used for rendering the recent scrobbles
    RecentScrobblesResponse recentScrobbles;

    public ScrobblesPage() {
      this.InitializeComponent();

      // load the user's recent scrobbles from the lastfm api
      LoadRecentScrobbles();

    }

    // get the user's recent scrobbles from the last fm api
    async void LoadRecentScrobbles() {
      HttpClient client = new HttpClient();

      client.DefaultRequestHeaders.Add("User-Agent", "WinFM");

      string responseJson = await client.GetStringAsync("http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=jamesbmadden&format=json&api_key=" + Api.Key);

      recentScrobbles = JsonSerializer.Deserialize<RecentScrobblesResponse>(responseJson);

      // now response is a proper c# object containing all the data from the API!
      // set the list view to read from the recent scrobbles
      RecentScrobblesList.ItemsSource = recentScrobbles.recenttracks.track;

    }
  }
}
