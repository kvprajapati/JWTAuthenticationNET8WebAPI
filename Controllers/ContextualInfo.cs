namespace JJwtAuthenticationWebAPI.Controllers
{
    public class ContextualInfo
    {
        public DateTime Timestamp { get; set; }
        public string SendingAppName { get; set; }
        public string UserId { get; set; }
        public Guid TrackingId { get; set; }
    }
}