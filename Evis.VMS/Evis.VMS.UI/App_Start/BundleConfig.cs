using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using Evis.VMS.UI.App_Start;

namespace Evis.VMS.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/Styles/bootstrap.min.css",
                 "~/Content/Styles/font-awesome.min.css",
                 "~/Content/Styles/bootstrap-progressbar-3.3.4.min.css",
                 "~/Content/Styles/jquery-jvectormap-2.0.3.css",
                 "~/Content/Styles/green.css",
                 "~/Content/Styles/custom.min.css",
                 "~/Content/Styles/dropzone.min.css",
                 "~/Content/Styles/pager.css",
                 "~/Content/Styles/toastr.min.css",
                 "~/Content/Styles/toastr-custom.css"
                 ));

            var bundle = new ScriptBundle("~/bundles/js")
             
                .Include("~/Content/Scripts/plugins/jquery.min.js")
                .Include("~/Content/Scripts/plugins/bootstrap.min.js")
                .Include("~/Content/Scripts/plugins/bootstrap-progressbar.min.js")
                //.Include("~/Content/Scripts/plugins/jquery.flot.pie.js")
                //.Include("~/Content/Scripts/plugins/jquery.flot.time.js")
                //.Include("~/Content/Scripts/plugins/jquery.flot.stack.js")
                //.Include("~/Content/Scripts/plugins/jquery.flot.resize.js")
                //.Include("~/Content/Scripts/plugins/jquery.flot.orderBars.js")
                .Include("~/Content/Scripts/plugins/date.js")
                //.Include("~/Content/Scripts/plugins/jquery.flot.spline.js")
                //.Include("~/Content/Scripts/plugins/jquery-jvectormap-2.0.3.min.js")
                .Include("~/Content/Scripts/plugins/moment.min.js")
                .Include("~/Content/Scripts/plugins/daterangepicker.js")
               .Include("~/Content/Scripts/plugins/validator.min.js")
                .Include("~/Content/Scripts/plugins/icheck.min.js")
                 .Include("~/Content/Scripts/plugins/fastclick.js")
                 .Include("~/Content/Scripts/plugins/dropzone.min.js")
                 .Include("~/Content/Scripts/plugins/Chart.bundle.js")
                 .Include("~/Content/Scripts/plugins/googleapi.js")
                .Include("~/Content/Scripts/plugins/custom.min.js");

                //.Include("~/Content/Scripts/plugins/jquery-jvectormap-world-mill-en.js")
                //.Include("~/Content/Scripts/plugins/jquery-jvectormap-us-aea-en.js")
                //.Include("~/Content/Scripts/plugins/gdp-data.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);


            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Content/Scripts/plugins/knockout-{version}.js",
                "~/Content/Scripts/plugins/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
              
               "~/Content/Scripts/application/pager.js",
               "~/Content/Scripts/application/toastr.min.js",
                "~/Content/Scripts/application/common.js",
              "~/Content/Scripts/viewModels/BuildingViewModel.js",
              "~/Content/Scripts/viewModels/DashboardViewModel.js",
              "~/Content/Scripts/viewModels/GatesViewModel.js",
              "~/Content/Scripts/viewModels/MyProfileViewModel.js",
              "~/Content/Scripts/viewModels/OrganizationViewModel.js",
              "~/Content/Scripts/viewModels/ScanVisitorViewModel.js",
              "~/Content/Scripts/viewModels/ShiftAssignmentViewModel.js",
              "~/Content/Scripts/viewModels/ShiftDetailsViewModel.js",
              "~/Content/Scripts/viewModels/ShiftViewModel.js",
              "~/Content/Scripts/viewModels/UsersViewModel.js",
              "~/Content/Scripts/viewModels/VisitorCheckInCheckOutViewModel.js",
              "~/Content/Scripts/viewModels/VisitorDetailsViewModel.js",
              "~/Content/Scripts/viewModels/VisitorViewModel.js",
              "~/Content/Scripts/viewModels/ThemeSelectionViewModel.js",
              "~/Content/Scripts/application/config.js",
               "~/Content/Scripts/application/app.js"
              ));

            //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //        "~/Content/Scripts/plugins/jquery-{version}.js"));

            //    bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
            //        "~/Content/Scripts/plugins/knockout-{version}.js",
            //        "~/Content/Scripts/plugins/knockout.validation.js"));

            //    //bundles.Add(new ScriptBundle("~/bundles/app").Include(
            //    //    "~/Scripts/app/ajaxPrefilters.js",
            //    //    "~/Scripts/app/app.bindings.js",
            //    //    "~/Scripts/app/app.datamodel.js",
            //    //    "~/Scripts/app/app.viewmodel.js",
            //    //    "~/Scripts/app/home.viewmodel.js",
            //    //    "~/Scripts/app/login.viewmodel.js",
            //    //    "~/Scripts/app/register.viewmodel.js",
            //    //    "~/Scripts/app/registerExternal.viewmodel.js",
            //    //    "~/Scripts/app/manage.viewmodel.js",
            //    //    "~/Scripts/app/userInfo.viewmodel.js",
            //    //    "~/Scripts/app/_run.js"));

            //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
            //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //        "~/Content/Scripts/plugins/modernizr-*"));

            //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //        "~/Content/Scripts/plugins/bootstrap.js",
            //        "~/Content/Scripts/plugins/respond.js"));

            //    bundles.Add(new StyleBundle("~/Content/css").Include(
            //         "~/Content/Styles/bootstrap.css",
            //         "~/Content/Styles/Site.css"));
            //}
        }
    }
}
