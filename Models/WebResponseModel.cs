public class WebResponseModel
{
    public int statusCode { get; set; }
    public string traceId { get; set; }
    public DateTime dateTime { get; set; }
    public string message { get; set; }
    public Dictionary<string, object> Args { get; set; }
}
