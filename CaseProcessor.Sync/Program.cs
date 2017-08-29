using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaseProcessor.DataAccess;
using CaseProcessor.DataAccess.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CaseProcessor.Sync
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = GetSyncData(@"C:\Users\esu\Downloads\output.csv");
            using (CaseProcessorDataContext context = new CaseProcessorDataContext())
            {
                foreach (var item in list)
                {
                    var c = context.Cases.FirstOrDefault(f => f.SrNumber.Equals(item.SRNumber.Replace("-1", "").Replace("-2", "")));
                    if (c != null)
                    {

                        context.Entry(c).State = EntityState.Modified;

                    }
                    else
                    {
                        c = new Case();
                        c.SrNumber = item.SRNumber;
                        c.DeveloperId = 1;
                        item.ProcessCase(c);
                        c.Backlog = new Backlog();
                        c.Backlog.ETA = string.Empty;
                        context.Cases.Add(c);
                        Console.WriteLine("New case {0} will be inserted", c.SrNumber);
                    }
                    item.ProcessCase(c);
                }
                var listSr = list.Select(s => s.SRNumber);
                var closes = from s in context.Cases.Where(w => w.Status != CaseStatus.Closed)
                             where listSr.All(a => a != s.SrNumber)
                             select s;
                Console.WriteLine("{0} case(s) closed", closes.Count());
                foreach (var c in closes)
                {
                    c.Status = CaseStatus.WaitingForClose;
                    context.Entry(c).State = EntityState.Modified;
                    Console.WriteLine("Case {0} is waiting for close.", c.SrNumber);
                }
                context.SaveChanges();
            }
            Console.Read();
        }

        static List<SyncData> GetSyncData(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<SyncDataMap>();
                csv.Configuration.Delimiter = "\t";
                var records = csv.GetRecords<SyncData>().ToList();
                return records;
            }
        }
    }
}

public class SyncDataMap : CsvClassMap<SyncData>
{
    public SyncDataMap()
    {
        Map(m => m.SRNumber).Index(18);
        Map(m => m.Subject).Index(4);
        Map(m => m.Status).Index(5);
        Map(m => m.Level).Index(6);
        Map(m => m.Owner).Index(7);
        Map(m => m.Version).Index(9);
        Map(m => m.LastUpdated).Index(13);
        Map(m => m.Customer).Index(15);
        Map(m => m.Description).Index(19);
        Map(m => m.Created).Index(20);
        Map(m => m.PushToDev).Index(22);
    }
}

public class SyncData
{
    public string SRNumber { get; set; }

    public string Subject { get; set; }

    public string Status { get; set; }

    public string Level { get; set; }

    public string Owner { get; set; }

    public string Version { get; set; }

    public string LastUpdated { get; set; }

    public string Customer { get; set; }

    public string Description { get; set; }

    public string Created { get; set; }

    public string PushToDev { get; set; }

    public override string ToString()
    {
        return string.Format("{0}-{1} {2}", SRNumber, Subject, Owner);
    }

    public void ProcessCase(Case c)
    {
        //c.SrNumber = this.SRNumber.Replace("-1", "");
        c.Subject = this.Subject;
        c.Status = ConvertStatus(this.Status);
        c.Level = int.Parse(this.Level.Replace("Level", "").Trim());
        c.Owner = this.Owner;
        c.Version = this.Version;
        c.Customer = this.Customer;
        c.OpenDate = DateTime.Parse(string.IsNullOrEmpty(this.PushToDev) ? this.Created : this.PushToDev);
        if (c.Status == CaseStatus.WaitForSupport && c.InternalStatus == InternalStatus.ToDo)
        {
            c.InternalStatus = InternalStatus.Waiting;
        }
        c.Location = ConvertLocation(c.Owner);
    }

    public CaseStatus ConvertStatus(string status)
    {
        switch (status)
        {
            case "Waiting Support Response":
                return CaseStatus.WaitForSupport;
            case "Update from Support":
                return CaseStatus.FromSupport;
            case "New":
                return CaseStatus.New;
            case "Defect Confirmed":
                return CaseStatus.DefectConfirmed;
            case "Enhancement Request Created":
                return CaseStatus.EnhancementRequestCreated;
            default:
                return CaseStatus.FromSupport;
        }
    }

    public Location ConvertLocation(string ownerName)
    {
        switch (ownerName)
        {
            case "MHATCH":
                return Location.EMEA;
            case "MKUNTING":
            case "SSOMASUN":
            case "ECHOO":
                return Location.ACPC;
            default:
                return Location.AMER;
        }
    }
}

