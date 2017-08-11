using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseProcessor.DataAccess.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        public string Value { get; set; }

        public virtual Case Case { get; set; }

        [ForeignKey("Case")]
        public int CaseId { get; set; }
    }
}
