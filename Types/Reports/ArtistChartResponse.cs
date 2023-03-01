using System.Text.Json.Serialization;

namespace WinFM.Types.Reports {

  public class ArtistChartResponse {
    public Weeklyartistchart weeklyartistchart { get; set; }
  }

  public class Weeklyartistchart {
    public Artist[] artist { get; set; }
    [JsonPropertyName("@attr")]
    public Attr2 attr { get; set; }
  }

  public class Attr2 {
    public string from { get; set; }
    public string user { get; set; }
    public string to { get; set; }
  }

  public class Artist {
    public string mbid { get; set; }
    public string url { get; set; }
    public string name { get; set; }
    [JsonPropertyName("@attr")]
    public Attr1 attr { get; set; }
    public string playcount { get; set; }
  }

  public class Attr1 {
    public string rank { get; set; }
  }

}