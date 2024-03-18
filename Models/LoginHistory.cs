using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthenticationWebAPI.Models
{
    public class LoginHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public required string UserName { get; set; }
        public required DateTime Timestamp { get; set; }
        public required string IpAddress { get; set; }
        public required Guid TrackingID { get; set; }
    }
}
