using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CaseProcessor.DataAccess.Models
{
    public class KB
    {
        [Key, ForeignKey("Case")]
        public int CaseId { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        [JsonIgnore]
        public virtual Case Case { get; set; }
    }
}