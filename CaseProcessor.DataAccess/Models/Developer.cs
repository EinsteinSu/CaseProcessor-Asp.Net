using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseProcessor.DataAccess.Models
{
    public class Developer
    {
        [Key]
        public int DeveloperId { get; set; }

        public string Name { get; set; }

    }
}
