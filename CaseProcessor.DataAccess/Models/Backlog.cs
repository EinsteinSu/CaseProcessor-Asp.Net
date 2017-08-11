using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CaseProcessor.DataAccess.Models
{
    public class Backlog
    {
        [Key, ForeignKey("Case")]
        public int CaseId { get; set; }

        [MaxLength(20)]
        public string BacklogNumber { get; set; }

        [MaxLength(100)]
        public string Versions { get; set; }

        [MaxLength(100)]
        public string ETA { get; set; }

        [JsonIgnore]
        public virtual Case Case { get; set; }
    }
}