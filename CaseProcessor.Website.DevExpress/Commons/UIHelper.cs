using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CaseProcessor.Business;
using CaseProcessor.DataAccess.Models;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Newtonsoft.Json;

namespace CaseProcessor.Website.DevExpress.Commons
{
    public static class UIHelper
    {
        public static List<Developer> SelectDevelopers()
        {
            var developerMgr = new DeveloperMgr();
            return developerMgr.GetDevelopers().ToList();
        }

        public static string GetDisplayNameFromAttribute<T>(this T source)
        {
            var fi = source.GetType().GetField(source.ToString());

            var attributes = (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Name;
            return source.ToString();
        }

        public static string GetBacklogHtml(object caseId)
        {
            int id;
            int.TryParse(caseId.ToString(), out id);
            var backlog = new CasesMgr().GetCaseBacklog(id);
            if (backlog != null)
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(
                    "<h3>Backlog Number: <a target='_blank' href='http://tfsreports.prod.quest.corp:8080/Windows%20Management/ArchiveManager/_workitems#_a=edit&id={0}'>{0}</a> </h3>",
                    backlog.BacklogNumber));
                sb.AppendLine(string.Format("<h3>Versions: {0} </h3>", backlog.Versions));
                sb.AppendLine(string.Format("<h3>ETA: {0} </h3>", backlog.ETA));
                return sb.ToString();
            }
            return string.Empty;
        }

        public static void SetCaseGridViewColors(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            var gridView = sender as MVCxGridView;
            if (gridView != null)
            {
                if (e.DataColumn.FieldName == "InternalStatus")
                {
                    var status = (InternalStatus)gridView.GetRowValues(e.VisibleIndex, "InternalStatus");
                    e.Cell.BackColor = ConvertInternalStatusColor(status);
                }
                if (e.DataColumn.FieldName == "Status")
                {
                    var status = (CaseStatus)gridView.GetRowValues(e.VisibleIndex, "Status");
                    e.Cell.BackColor = ConvertStatusColor(status);
                }
                if (e.DataColumn.FieldName == "Level")
                {
                    var level = (int)gridView.GetRowValues(e.VisibleIndex, "Level");
                    e.Cell.BackColor = ConvertLevelColor(level);
                }
            }
        }

        public static string GetClosedHtml(object caseId)
        {
            int id;
            int.TryParse(caseId.ToString(), out id);
            var mgr = new CasesMgr();
            var closed = mgr.GetClosed(id);
            var c = mgr.GetCaseById(id);
            if (closed != null)
            {
                var sb = new StringBuilder();
                //var isClosed = closed.CloseTime != null;
                //var color = isClosed ? "green" : "red";
                //sb.AppendLine(string.Format("<h3 style='color:{1}'>Closed: {0}</h3>", isClosed, color));
                sb.AppendLine("<h3>Root Cause:</h3>");
                sb.AppendLine(string.Format("<h3 style='color:red'> {0} </h3>", closed.RootCause));
                return sb.ToString();
            }
            return string.Empty;
        }

        public static string ConvertHtmlForActivity(IList<Activity> activities)
        {
            if (activities != null)
            {
                var sb = new StringBuilder();
                foreach (var item in activities)
                {
                    //sb.AppendLine("<p>");
                    sb.AppendLine(item.Content);
                    sb.AppendLine(item.Time.ToShortDateString());
                    //sb.AppendLine("</p>");
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        public static Color ConvertLevelColor(int level)
        {
            switch (level)
            {
                case 1:
                    return Color.Red;
                case 2:
                    return Color.DarkOrange;
                case 3:
                    return Color.SteelBlue;
                default:
                    return Color.MediumSeaGreen;
            }
        }

        public static string ConvertStatusToString(CaseStatus status)
        {
            switch (status)
            {
                case CaseStatus.Closed:
                    return "Closed";
                case CaseStatus.DefectConfirmed:
                    return "Defect Confirmed";
                case CaseStatus.EnhancementRequestCreated:
                    return "Enhancement Created";
                case CaseStatus.FromSupport:
                    return "From Support";
                case CaseStatus.New:
                    return "New";
                case CaseStatus.WaitForSupport:
                    return "Wait Support";
                case CaseStatus.WaitingForClose:
                    return "Waiting for Close";
                default:
                    return status.ToString();
            }
        }

        public static Color ConvertStatusColor(CaseStatus status)
        {
            switch (status)
            {
                case CaseStatus.FromSupport:
                case CaseStatus.WaitingForClose:
                    return Color.Red;
                case CaseStatus.New:
                    return Color.DarkOrange;
                default:
                    return Color.MediumSeaGreen;
            }
        }

        public static Color ConvertInternalStatusColor(InternalStatus s)
        {
            switch (s)
            {
                case InternalStatus.Done:
                case InternalStatus.Waiting:
                    return Color.MediumSeaGreen;
                case InternalStatus.Debugging:
                case InternalStatus.Investigating:
                case InternalStatus.Reproducing:
                    return Color.SteelBlue;
                case InternalStatus.ToDo:
                    return Color.Red;
                default:
                    return Color.DarkOrange;
            }
        }

        public static MvcHtmlString GetJson(this HtmlHelper htmlHelper, object vm)
        {
            var s = JsonConvert.SerializeObject(vm);
            return new MvcHtmlString(s);
        }

        // <summary>
        /// Used to determine the direction of the sort identifier used when filtering lists
        /// <param name="htmlHelper"></param>
        /// <param name="sortOrder">the current sort order being used on the page</param>
        /// <param name="field">the field that we are attaching this sort identifier to</param>
        /// <returns>MvcHtmlString used to indicate the sort order of the field</returns>
        public static IHtmlString SortIdentifier(this HtmlHelper htmlHelper, string sortOrder, string field)
        {
            if (string.IsNullOrEmpty(sortOrder) || sortOrder.Trim() != field &&
                sortOrder.Replace("_desc", "").Trim() != field) return null;

            var glyph = "glyphicon glyphicon-chevron-up";
            if (sortOrder.ToLower().Contains("desc"))
                glyph = "glyphicon glyphicon-chevron-down";

            var span = new TagBuilder("span");
            span.Attributes["class"] = glyph;

            return MvcHtmlString.Create(span.ToString());
        }

        /// <summary>
        ///     Converts a NameValueCollection into a RouteValueDictionary containing all of the elements in the collection, and
        ///     optionally appends
        ///     {newKey: newValue} if they are not null
        /// </summary>
        /// <param name="collection">NameValue collection to convert into a RouteValueDictionary</param>
        /// <param name="newKey">the name of a key to add to the RouteValueDictionary</param>
        /// <param name="newValue">the value associated with newKey to add to the RouteValueDictionary</param>
        /// <returns>
        ///     A RouteValueDictionary containing all of the keys in collection, as well as {newKey: newValue} if they are not
        ///     null
        /// </returns>
        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection, string newKey,
            string newValue)
        {
            var routeValueDictionary = new RouteValueDictionary();
            foreach (var key in collection.AllKeys)
            {
                if (key == null) continue;
                if (routeValueDictionary.ContainsKey(key))
                    routeValueDictionary.Remove(key);

                routeValueDictionary.Add(key, collection[key]);
            }
            if (string.IsNullOrEmpty(newValue))
            {
                routeValueDictionary.Remove(newKey);
            }
            else
            {
                if (routeValueDictionary.ContainsKey(newKey))
                    routeValueDictionary.Remove(newKey);

                routeValueDictionary.Add(newKey, newValue);
            }
            return routeValueDictionary;
        }

        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection)
        {
            return ToRouteValueDictionary(collection, "", "");
        }
    }
}