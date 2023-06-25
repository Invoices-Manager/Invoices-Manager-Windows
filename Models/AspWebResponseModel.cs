
public class AspWebResponseModel
{
    public string type { get; set; }
    public string title { get; set; }
    public int status { get; set; }
    public string traceId { get; set; }
    public Dictionary<string, object> errors { get; set; }
}