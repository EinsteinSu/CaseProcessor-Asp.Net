using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CaseProcessor.Business;
using CaseProcessor.DataAccess.Models;
using Newtonsoft.Json;

namespace CaseProcessor.Website.Commons
{
    public static class UIHelper
    {
        public static List<SelectListItem> SelectDevelopers()
        {
            DeveloperMgr developerMgr = new DeveloperMgr();
            var list = new List<SelectListItem>();
            foreach (var developer in developerMgr.GetDevelopers())
            {
                list.Add(new SelectListItem { Text = developer.Name, Value = developer.DeveloperId.ToString() });
            }
            return list;
        }

        public static string ConvertHtmlForActivity(IList<Activity> activities)
        {
            if (activities != null)
            {
                StringBuilder sb = new StringBuilder();
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

        public static string ConvertLevelClass(int level)
        {
            switch (level)
            {
                case 1:
                    return "btn-danger";
                case 2:
                    return "btn-warning";
                case 3:
                    return "btn-info";
                default:
                    return "btn-success";
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

        public static string ConvertStatusClass(CaseStatus status)
        {
            switch (status)
            {
                case CaseStatus.FromSupport:
                case CaseStatus.WaitingForClose:
                    return "btn-danger";
                case CaseStatus.New:
                    return "btn-warning";
                default:
                    return "btn-success";
            }
        }

        public static string ConvertInternalStatusClass(InternalStatus s)
        {
            switch (s)
            {
                case InternalStatus.Done:
                case InternalStatus.Waiting:
                    return "btn-success";
                case InternalStatus.Debugging:
                case InternalStatus.Investigating:
                case InternalStatus.Reproducing:
                    return "btn-info";
                case InternalStatus.ToDo:
                    return "btn-danger";
                default:
                    return "btn-warning";
            }
        }

        public static MvcHtmlString GetJson(this HtmlHelper htmlHelper, object vm)
        {
            string s = JsonConvert.SerializeObject(vm);
            return new MvcHtmlString(s);
        }

        // <summary>
        /// Used to determine the direction of the sort identifier used when filtering lists
        /// 
        /// <param name="htmlHelper"></param>
        /// <param name="sortOrder">the current sort order being used on the page</param>
        /// <param name="field">the field that we are attaching this sort identifier to</param>
        /// <returns>MvcHtmlString used to indicate the sort order of the field</returns>
        public static IHtmlString SortIdentifier(this HtmlHelper htmlHelper, string sortOrder, string field)
        {
            if (string.IsNullOrEmpty(sortOrder) || (sortOrder.Trim() != field && sortOrder.Replace("_desc", "").Trim() != field)) return null;

            string glyph = "glyphicon glyphicon-chevron-up";
            if (sortOrder.ToLower().Contains("desc"))
            {
                glyph = "glyphicon glyphicon-chevron-down";
            }

            var span = new TagBuilder("span");
            span.Attributes["class"] = glyph;

            return MvcHtmlString.Create(span.ToString());
        }

        /// <summary>
        /// Converts a NameValueCollection into a RouteValueDictionary containing all of the elements in the collection, and optionally appends
        /// {newKey: newValue} if they are not null
        /// </summary>
        /// <param name="collection">NameValue collection to convert into a RouteValueDictionary</param>
        /// <param name="newKey">the name of a key to add to the RouteValueDictionary</param>
        /// <param name="newValue">the value associated with newKey to add to the RouteValueDictionary</param>
        /// <returns>A RouteValueDictionary containing all of the keys in collection, as well as {newKey: newValue} if they are not null</returns>
        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection, string newKey, string newValue)
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
