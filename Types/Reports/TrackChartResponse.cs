using System.Text.Json.Serialization;

namespace WinFM.Types.Reports {

  public class TrackChartResponse {
    public Weeklytrackchart weeklytrackchart { get; set; }
  }

  public class Weeklytrackchart {
    public Track[] track { get; set; }
    [JsonPropertyName("@attr")]
    public Attr5 attr { get; set; }
  }

  public class Attr5 {
    public string from { get; set; }
    public string user { get; set; }
    public string to { get; set; }
  }

  public class Track {
    public AlbumArtist artist { get; set; }
    public Image[] image { get; set; }
    public string mbid { get; set; }
    public string url { get; set; }
    public string name { get; set; }
    [JsonPropertyName("@attr")]
    public Attr6 attr { get; set; }
    public string playcount { get; set; }
  }

  public class Attr6 {
    public string rank { get; set; }
  }

  public class Image {
    public string size { get; set; }
    [JsonPropertyName("#text")]
    public string text { get; set; }
  }

}