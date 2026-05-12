namespace DotNet46ApiExample.Models
{
    public class OrderResponse
    {
        public int DatabaseId { get; set; }
        public string KafkaTopic { get; set; }
        public string ExternalApiStatus { get; set; }
        public string MessageId { get; set; }
    }
}
