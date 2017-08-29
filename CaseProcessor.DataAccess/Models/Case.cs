using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using Supeng.Common.Datetimes;

namespace CaseProcessor.DataAccess.Models
{
    public class Case
    {
        public Case()
        {
            Status = CaseStatus.New;
            Level = 3;
        }

        [Key]
        public int CaseId { get; set; }

        [MaxLength(7)]
        [Display(Name = "CR Number")]
        public string SrNumber { get; set; }

        public int Level { get; set; }

        [MaxLength(10)]
        public string Version { get; set; }

        [MaxLength(100)]
        public string Customer { get; set; }

        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Subject { get; set; }

        [Display(Name = "Open Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? OpenDate { get; set; }

        public int Duration
        {
            get
            {
                if (OpenDate.HasValue)
                {
                    return (int)OpenDate.Value.DateDiff(DateTime.Now);
                }
                return 999;
            }
        }


        [MaxLength(50)]
        public string Component { get; set; }

        public Location Location { get; set; }

        public InternalStatus InternalStatus { get; set; }

        public CaseStatus Status { get; set; }

        [MaxLength(50)]
        public string Owner { get; set; }

        [ForeignKey("Developer")]
        [JsonIgnore]
        public int? DeveloperId { get; set; }

        public bool CRTracking { get; set; }

        public virtual Developer Developer { get; set; }
        public virtual IList<Environment> Environments { get; set; }
        public virtual IList<ToDo> ToDoList { get; set; }

        public string CurrentToDo
        {
            get
            {
                if (ToDoList != null && ToDoList.Any(w => !w.Done))
                {
                    return ToDoList.Where(w => !w.Done).OrderByDescending(o => o.Time).First().Content;
                }
                return string.Empty;
            }
        }

        public string TFSNumber
        {
            get
            {
                if (!string.IsNullOrEmpty(Backlog?.BacklogNumber))
                    return Backlog.BacklogNumber;
                return string.Empty;
            }
        }

        public string CurrentActivity
        {
            get
            {
                if (Activities != null && Activities.Any())
                {
                    return Activities.OrderByDescending(o => o.Time).First().Content;
                }
                return string.Empty;
            }
        }
        public virtual IList<Activity> Activities { get; set; }
        public virtual IList<Tag> Tags { get; set; }
        public virtual Backlog Backlog { get; set; }
        public virtual Closed Closed { get; set; }
        public virtual KB KB { get; set; }

    }


    public enum Location
    {
        ACPC,
        EMEA,
        AMER
    }

    public enum InternalStatus
    {
        [Display(Name = "To Do")]
        ToDo,
        Waiting,
        Investigating,
        Reproducing,
        Debugging,
        Done
    }

    public enum CaseStatus
    {
        [Display(Name = "From Support")]
        FromSupport,
        [Display(Name = "Waiting for Support Response")]
        WaitForSupport,
        [Display(Name = "Defect Confirmed")]
        DefectConfirmed,
        New,
        Closed,
        [Display(Name = "Enhancement Request Created")]
        EnhancementRequestCreated,
        [Display(Name = "Case waiting for close")]
        WaitingForClose
    }
}