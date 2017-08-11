using System;
using System.Collections.Generic;
using System.Linq;
using CaseProcessor.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = CaseProcessor.DataAccess.Models.Environment;

namespace CaseProcessor.Business.Tests
{
    [TestClass]
    public class CaseMgrTest
    {
        private CasesMgr mgr = new CasesMgr();
        [TestMethod]
        public void Add()
        {
            Case c = new Case();
            c.SrNumber = "1234567";
            c.Environments = new List<Environment>();
            c.Environments.Add(new Environment { Type = EnvironmentType.Sql, Value = "SQL 2014" });
            var c1 = mgr.Add(c);
            Assert.IsNotNull(c1);
            Assert.IsTrue(mgr.Delete(c1.CaseId));
        }

        [TestMethod]
        public void Update()
        {
            Case c = new Case();
            c.SrNumber = "1234567";
            c.Environments = new List<Environment>();
            c.Environments.Add(new Environment { Type = EnvironmentType.Sql, Value = "SQL 2014" });
            mgr.Add(c);
            c.SrNumber = "456789";
            Assert.IsNotNull(mgr.Update(c));
            var c1 = mgr.GetCaseById(c.CaseId);
            Assert.AreEqual(c1.SrNumber, "456789");
            Assert.IsTrue(mgr.Delete(c.CaseId));
        }
    }
}
