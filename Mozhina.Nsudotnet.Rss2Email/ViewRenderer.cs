using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Mozhina.Nsudotnet.Rss2Email.Mocks;

namespace Mozhina.Nsudotnet.Rss2Email
{
    public class ViewRenderer
    {
        private readonly HttpContextBase _contextMock;

        private readonly ControllerBase _controllerMock;

        private readonly RouteData _routeDataMock;

        private readonly ControllerContext _controllerContextMock;

        public ViewRenderer()
        {
             _routeDataMock = new RouteData();
            _controllerMock = new ControllerMock();
            _contextMock = new HttpContextMock();
            _controllerContextMock = new ControllerContext(_contextMock, _routeDataMock, _controllerMock);
        }

        public string RenderView<TModel>(TModel model, string templateName)
        {
            string templatePath = string.Format(Constants.TemplatesPath, templateName);
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);

            var dict = new ViewDataDictionary(model);
            var viewMock = new RazorView(_controllerContextMock, templatePath, Constants.LayoutPath, true, new string[] { "cshtml" });
            var viewContext = new ViewContext(_controllerContextMock, viewMock, dict, new TempDataDictionary(), tw);

/*
            Type masterType = Type.GetType(string.Format(Constants.TemplatesTypeTemplate, templateName));
            if (masterType == null)
            {
                return "";
            }

            WebViewPage master = (WebViewPage)Activator.CreateInstance(masterType);
            master.VirtualPath = string.Format(Constants.TemplatesPath, "_Layout");
            master.ViewData = dict;
            master.Context = _contextMock;
            master.ViewContext = viewContext;
            master.InitHelpers();
            master.VirtualPathFactory = new VirtualPathFactoryMock();
*/

            Type pageType = Assembly.GetExecutingAssembly().GetType(string.Format(Constants.TemplatesTypeTemplate, templateName));
            if (pageType == null)
            {
                return "";
            }

            WebViewPage vp = (WebViewPage)Activator.CreateInstance(pageType);

            //vp.Layout = Constants.LayoutPath;
            vp.VirtualPath = templatePath;
            vp.ViewContext = viewContext;
            vp.ViewData = viewContext.ViewData;
            vp.Context = _contextMock;
            vp.InitHelpers();
            vp.VirtualPathFactory = new VirtualPathFactoryMock();

            WebPageContext ctx = new WebPageContext(_contextMock, vp, model);
            vp.ExecutePageHierarchy(ctx, tw);

            return sb.ToString();
        }
    }

    public static class Constants
    {
        public const string TemplatesTypeTemplate = "ASP._Views_Mail_{0}_cshtml";
        public const string TemplatesPath = "~/Views/Mail/{0}.cshtml";
        public const string LayoutPath = "~/Views/Mail/_Layout.cshtml";
    }
}