using System.Text.Json.Serialization;

namespace WinFM.Types {

  public class UserDataResponse {
    public User user { get; set; }
  }

  public class User {
    public string name { get; set; }
    public string age { get; set; }
    public string subscriber { get; set; }
    public string realname { get; set; }
    public string bootstrap { get; set; }
    public string playcount { get; set; }
    public string artist_count { get; set; }
    public string playlists { get; set; }
    public string track_count { get; set; }
    public string album_count { get; set; }
    public Image[] image { get; set; }
    public Registered registered { get; set; }
    public string country { get; set; }
    public string gender { get; set; }
    public string url { get; set; }
    public string type { get; set; }
  }

  public class Registered {
    public string unixtime { get; set; }
    [JsonPropertyName("#text")]
    public int text { get; set; }
  }

}