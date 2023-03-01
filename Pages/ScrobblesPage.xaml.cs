using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

  // class for incrementally loading recent scrobbles from the LastFM API
  public class RecentScrobbleLoader : ObservableCollection<Track>, ISupportIncrementalLoading {

    uint currentPage = 1;
    uint scrobblesPerPage = 50;
    uint totalPages = 1;
    HttpClient client = new HttpClient();

    public RecentScrobbleLoader() {
      client.DefaultRequestHeaders.Add("User-Agent", "WinFM");
    }

    // check whether more can be loaded
    public bool HasMoreItems { get { return currentPage <= totalPages; } }

    // load more items
    public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) {

      return AsyncInfo.Run(async cancelToken => {

        // fetch the data from the server!
        string responseJson = await client.GetStringAsync("https://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=jamesbmadden&page=" + currentPage + "&format=json&api_key=" + Api.Key);

        RecentScrobblesResponse recentScrobbles = JsonSerializer.Deserialize<RecentScrobblesResponse>(responseJson);

        // add each track to the list
        foreach (Track track in recentScrobbles.recenttracks.track) {
          Add(track);
        }

        // if this is the first page, keep track of how many total pages there are
        if (currentPage == 1) {
          totalPages = uint.Parse(recentScrobbles.recenttracks.attr.totalPages);
        }

        // make sure the next page is loaded on the next request
        currentPage++;

        return new LoadMoreItemsResult { Count = (uint) recentScrobbles.recenttracks.track.Length };
      });

    }

  }

  public sealed partial class ScrobblesPage : Page {

    public ScrobblesPage() {
      this.InitializeComponent();

      // establish the source of scrobbles as the RecentScrobbleLoader class
      RecentScrobblesList.ItemsSource = new RecentScrobbleLoader();

    }
  }
}
