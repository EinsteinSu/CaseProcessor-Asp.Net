using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CaseProcessor.DataAccess.Models
{
    public class Closed
    {
        [Key, ForeignKey("Case")]
        public int CaseId { get; set; }

        public DateTime? CloseTime { get; set; }

        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string RootCause { get; set; }

        [JsonIgnore]
        public virtual Case Case { get; set; }
    }
}