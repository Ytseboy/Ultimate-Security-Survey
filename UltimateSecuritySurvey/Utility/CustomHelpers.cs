using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace UltimateSecuritySurvey.Utility
{   
    public static class CustomHelpers
    {
        /* This Ajax helper is needed to create multiline links 
         * or add html elements like <span> inside of a link    */
        public static MvcHtmlString CustomActionLink(this AjaxHelper ajaxHelper, string linkText,
            string actionName, object routeValues, AjaxOptions ajaxOptions)
        {
            var dummyText = Guid.NewGuid().ToString();
            var link = ajaxHelper.ActionLink(dummyText, actionName, routeValues, ajaxOptions);
            return MvcHtmlString.Create(link.ToString().Replace(dummyText, linkText));
        }

        public static MvcHtmlString CustomActionLink(this AjaxHelper ajaxHelper, string linkText,
            string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions)
        {
            var dummyText = Guid.NewGuid().ToString();
            var link = ajaxHelper.ActionLink(dummyText, actionName, controllerName, routeValues, ajaxOptions);
            return MvcHtmlString.Create(link.ToString().Replace(dummyText, linkText));
        }

        public static MvcHtmlString CustomActionLink(this AjaxHelper ajaxHelper, string linkText,
            string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var dummyText = Guid.NewGuid().ToString();
            var link = ajaxHelper.ActionLink(dummyText, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(link.ToString().Replace(dummyText, linkText));
        }
    }
}