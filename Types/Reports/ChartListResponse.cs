using System.Text.Json.Serialization;

namespace WinFM.Types.Reports {

  public class ChartListResponse {
    public Weeklychartlist weeklychartlist { get; set; }
  }

  public class Weeklychartlist {
    public Chart[] chart { get; set; }
    [JsonPropertyName("@attr")]
    public Attr attr { get; set; }
  }

  public class Attr {
    public string user { get; set; }
  }

  public class Chart {
    [JsonPropertyName("#text")]
    public string text { get; set; }
    public string from { get; set; }
    public string to { get; set; }
  }

}