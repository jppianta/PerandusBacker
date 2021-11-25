using System.Collections.Generic;

namespace PerandusBacker.Json
{
  internal interface PostData
  {
    IEnumerable<KeyValuePair<string, string>> ToArray();
  }

  internal class ForumInfo : PostData
  {
    public string Content { get; set; }

    public string Title { get; set; }

    public string Hash { get; set; }

    public string Submit { get; set; }

    public IEnumerable<KeyValuePair<string, string>> ToArray()
    {
      return new[] {
        new KeyValuePair<string, string>("content", Content),
        new KeyValuePair<string, string>("title", Title),
        new KeyValuePair<string, string>("hash", Hash),
        new KeyValuePair<string, string>("submit", Submit),
      };
    }
  }
}
