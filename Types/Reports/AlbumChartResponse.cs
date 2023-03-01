using System.Text.Json.Serialization;

namespace WinFM.Types.Reports {

  public class AlbumChartResponse {
    public Weeklyalbumchart weeklyalbumchart { get; set; }
  }

  public class Weeklyalbumchart {
    public Album[] album { get; set; }
    [JsonPropertyName("@attr")]
    public Attr3 attr { get; set; }
  }

  public class Attr3 {
    public string from { get; set; }
    public string user { get; set; }
    public string to { get; set; }
  }

  public class Album {
    public AlbumArtist artist { get; set; }
    public string mbid { get; set; }
    public string url { get; set; }
    public string name { get; set; }
    [JsonPropertyName("@attr")]
    public Attr4 attr { get; set; }
    public string playcount { get; set; }
  }

  public class AlbumArtist {
    public string mbid { get; set; }
    [JsonPropertyName("#text")]
    public string text { get; set; }
  }

  public class Attr4 {
    public string rank { get; set; }
  }

}