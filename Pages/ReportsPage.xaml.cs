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
using WinFM.Types.Reports;

namespace WinFM.Pages {

  // keep track of the loaded reports
  // in the future, they can be saved to a local file since
  // they will never change.
  public class ReportsData {

    // the reports already loaded
    public Dictionary<int, ArtistChartResponse> artistCharts = new Dictionary<int, ArtistChartResponse>();
    public Dictionary<int, AlbumChartResponse> albumCharts = new Dictionary<int, AlbumChartResponse>();
    public Dictionary<int, TrackChartResponse> trackCharts = new Dictionary<int, TrackChartResponse>();

    public ChartListResponse chartLists;

    HttpClient client = new HttpClient();

    // setup the headers
    public ReportsData() {
      client.DefaultRequestHeaders.Add("User-Agent", "WinFM");
    }

    // grab the list of charts available
    public async Task GetCharts() {

      // grab the list of charts
      string responseJson = await client.GetStringAsync("https://ws.audioscrobbler.com/2.0/?method=user.getweeklychartlist&user=jamesbmadden&format=json&api_key=" + Api.Key);
      chartLists = JsonSerializer.Deserialize<ChartListResponse>(responseJson);

    }

    // load a week 
    public async Task<(ArtistChartResponse, AlbumChartResponse, TrackChartResponse)> GetWeek(int weekStart) {

      // if the week is already loaded, return it!
      if (artistCharts.ContainsKey(weekStart)) {
        return (
          artistCharts.GetValueOrDefault(weekStart), 
          albumCharts.GetValueOrDefault(weekStart), 
          trackCharts.GetValueOrDefault(weekStart)
        );
      }

      // otherwise, it'll have to be loaded from the API!
      return await LoadWeekData(weekStart);

    }

    async Task<(ArtistChartResponse, AlbumChartResponse, TrackChartResponse)> LoadWeekData(int weekStart) {

      // grab the artist chart
      string artistResponseJson = await client.GetStringAsync("https://ws.audioscrobbler.com/2.0/?method=user.getweeklyartistchart&user=jamesbmadden&from=" + weekStart + "&format=json&api_key=" + Api.Key);
      ArtistChartResponse artistChart = JsonSerializer.Deserialize<ArtistChartResponse>(artistResponseJson);
      artistCharts.Add(weekStart, artistChart);

      // and the album
      string albumResponseJson = await client.GetStringAsync("https://ws.audioscrobbler.com/2.0/?method=user.getweeklyalbumchart&user=jamesbmadden&from=" + weekStart + "&format=json&api_key=" + Api.Key);
      AlbumChartResponse albumChart = JsonSerializer.Deserialize<AlbumChartResponse>(albumResponseJson);
      albumCharts.Add(weekStart, albumChart);

      // and the tracks
      string trackResponseJson = await client.GetStringAsync("https://ws.audioscrobbler.com/2.0/?method=user.getweeklytrackchart&user=jamesbmadden&from=" + weekStart + "&format=json&api_key=" + Api.Key);
      TrackChartResponse trackChart = JsonSerializer.Deserialize<TrackChartResponse>(trackResponseJson);
      trackCharts.Add(weekStart, trackChart);

      // return the data!
      return (artistChart, albumChart, trackChart);

    }

  }
  
  public sealed partial class ReportsPage : Page {

    ReportsData data = new ReportsData();
    int currentWeek = 0;
    int currentWeekIndex = 0;

    public ReportsPage() {

      this.InitializeComponent();

      loadData();
      
    }

    public async void loadData() {

      // establish the list of charts
      await data.GetCharts();

      currentWeek = int.Parse(data.chartLists.weeklychartlist.chart[currentWeekIndex].from);

      // now load the current week!
      await data.GetWeek(currentWeek);
      // then set the fields
      setDataFields();

    }

    // set the data
    public void setDataFields() {

      // set the top artists
      TopArtist0.Text = data.artistCharts.GetValueOrDefault(currentWeek).weeklyartistchart.artist[0].name;
      TopArtist1.Text = data.artistCharts.GetValueOrDefault(currentWeek).weeklyartistchart.artist[1].name;
      TopArtist2.Text = data.artistCharts.GetValueOrDefault(currentWeek).weeklyartistchart.artist[2].name;
      TopArtist3.Text = data.artistCharts.GetValueOrDefault(currentWeek).weeklyartistchart.artist[3].name;
      TopArtist4.Text = data.artistCharts.GetValueOrDefault(currentWeek).weeklyartistchart.artist[4].name;

      // set the top albums
      TopAlbum0.Text = data.albumCharts.GetValueOrDefault(currentWeek).weeklyalbumchart.album[0].name;
      TopAlbum1.Text = data.albumCharts.GetValueOrDefault(currentWeek).weeklyalbumchart.album[1].name;
      TopAlbum2.Text = data.albumCharts.GetValueOrDefault(currentWeek).weeklyalbumchart.album[2].name;
      TopAlbum3.Text = data.albumCharts.GetValueOrDefault(currentWeek).weeklyalbumchart.album[3].name;
      TopAlbum4.Text = data.albumCharts.GetValueOrDefault(currentWeek).weeklyalbumchart.album[4].name;

      // set the top tracks
      TopTrack0.Text = data.trackCharts.GetValueOrDefault(currentWeek).weeklytrackchart.track[0].name;
      TopTrack1.Text = data.trackCharts.GetValueOrDefault(currentWeek).weeklytrackchart.track[1].name;
      TopTrack2.Text = data.trackCharts.GetValueOrDefault(currentWeek).weeklytrackchart.track[2].name;
      TopTrack3.Text = data.trackCharts.GetValueOrDefault(currentWeek).weeklytrackchart.track[3].name;
      TopTrack4.Text = data.trackCharts.GetValueOrDefault(currentWeek).weeklytrackchart.track[4].name;

    }

  }
}
