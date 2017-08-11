using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CaseProcessor.DataAccess.Models
{
    public class Environment
    {
        [Key]
        public int EnvironmentId { get; set; }

        [ForeignKey("Case")]
        public int CaseId { get; set; }

        [JsonIgnore]
        public virtual Case Case { get; set; }

        public EnvironmentType Type { get; set; }

        public string Value { get; set; }
    }

    public enum EnvironmentType
    {
        OS,
        Exchange,
        MapiCdo,
        Sql,
        Another
    }
}