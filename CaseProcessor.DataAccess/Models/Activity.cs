using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CaseProcessor.DataAccess.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [ForeignKey("Case")]
        public int CaseId { get; set; }

        [JsonIgnore]
        public virtual Case Case { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime Time { get; set; }
    }
}