using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Evis.VMS.UI.Controllers;

namespace Evis.VMS.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext context)
            {
                //object sender = (object)context;
                ErrorLog(context.Exception, "", context.Request.RequestUri.PathAndQuery.ToString());

                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Index");
                routeData.Values.Add("Summary", "");
                routeData.Values.Add("Description", "");

                // ErrorHandling(sender, routeData);
            }
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*BarCodeHandler}", new { BarCodeHandler = @"(.*/)?Barcode.ashx(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            #region Controller and Action Info
            var httpContext = ((MvcApplication)sender).Context;
            var _controllerName = " ";
            var _actionName = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    _controllerName = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    _actionName = currentRouteData.Values["action"].ToString();
                }
            }

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Error");
            routeData.Values.Add("Summary", exception.InnerException);
            routeData.Values.Add("Description", exception.Message);





            #endregion

            #region Call Error Logging

            ErrorLog(exception, _controllerName, _actionName);

            #endregion

            #region Call Error Handling

            Response.TrySkipIisCustomErrors = true;

            if (exception.GetType() == typeof(HttpException))
            {
                Response.StatusCode = 404;
                var httpException = (HttpException)exception;
                var code = httpException.GetHttpCode();
                routeData.Values.Add("Status", code);
            }

            else
            {
                Response.StatusCode = 500;
                routeData.Values.Add("Status", 500);
            }

            routeData.Values.Add("error", exception);

            IController errorcontroller = new ErrorController();

            errorcontroller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));


            //IController controller = new SGHSolution.Web.Controllers.ErrorController();
            ////controller.Execute(new RequestContext());
            //controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            Response.End();

            #endregion

        }
        #region ErrorLogging
        private static void ErrorLog(Exception _exception, string _ctrlName, string _actionName)
        {
            #region Private Member Declaration

            FileStream fileStream = null;
            StreamWriter streamWriter = null;

            string logFilePath = "~/SGHErrorLog/";
            string FolderName = DateTime.Now.ToString("MMM") + "-" + DateTime.Today.Year.ToString() + "/";
            string fileName = DateTime.Now.Date.ToString("dd-MMM-yyyy") + "." + "txt";
            logFilePath += FolderName + fileName;
            #endregion

            #region Create the Log file directory if it does not exists

            // if (logFilePath.Equals("")) return;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(logFilePath));
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();

            #endregion Create the Log file directory if it does not exists

            #region Error Title

            string _errorTitle = Environment.NewLine + "*********************************************************************" + Environment.NewLine +
                            "Error Date - " + DateTime.Today.Date.ToString("yyyy-MM-dd") + Environment.NewLine +
                            "Error Time - " + DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine +
                            "*********************************************************************" + Environment.NewLine + Environment.NewLine;

            #endregion

            #region Write Error

            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(System.Web.HttpContext.Current.Server.MapPath(logFilePath), FileMode.Append);
            }
            streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(_errorTitle + "Controller Name :- " + _ctrlName + Environment.NewLine +
               "Action Name :- " + _actionName + Environment.NewLine + Environment.NewLine
                + "Error Message :- " + _exception);

            #endregion

            #region Close Connection

            streamWriter.Dispose();
            fileStream.Dispose();


            #endregion

        }
        #endregion
    }
}
