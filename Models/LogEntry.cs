using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthenticationWebAPI.Models
{


    [Table("LogEntries")]
    public class LogEntry
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required DateTime Logged { get; set; }
        public required string Application { get; set; }
        public required string Level { get; set; }
        public required string Message { get; set; }
        public required string Logger { get; set; }
        public required string Callsite { get; set; }
        public required string Exception { get; set; }
    }
}
