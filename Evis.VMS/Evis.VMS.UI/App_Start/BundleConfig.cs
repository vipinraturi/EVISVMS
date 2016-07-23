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
            bundles.Add(new StyleBundle("~/Content/style").Include(
                  "~/Content/Styles/jquery-ui.css",
                  "~/Content/Styles/bootstrap.min.css",
               "~/Content/Styles/font-awesome.min.css",
               "~/Content/Styles/bootstrap-progressbar-3.3.4.min.css",
               "~/Content/Styles/jquery-jvectormap-2.0.3.css"
                 ));

            bundles.Add(new StyleBundle("~/Content/theme1").Include(
                  "~/Content/Styles/jquery-ui.css",
               "~/Content/Styles/green.css",
                 "~/Content/Styles/custom.css",
                 "~/Content/Styles/custom_theme1.css",
               "~/Content/Styles/dropzone.min.css",
               "~/Content/Styles/pager.css",
               "~/Content/Styles/toastr.min.css",
               "~/Content/Styles/toastr-custom.css",
               "~/Content/Styles/timepicker.less"
                 ));

            bundles.Add(new StyleBundle("~/Content/theme2").Include(
                 "~/Content/Styles/jquery-ui.css",
               "~/Content/Styles/green.css",
                "~/Content/Styles/custom_theme2.css",
               "~/Content/Styles/dropzone.min.css",
               "~/Content/Styles/pager.css",
               "~/Content/Styles/toastr.min.css",
               "~/Content/Styles/toastr-custom.css"
                ));

            bundles.Add(new StyleBundle("~/Content/theme3").Include(
                 "~/Content/Styles/jquery-ui.css",
               "~/Content/Styles/green.css",
                "~/Content/Styles/custom_theme3.css",
               "~/Content/Styles/dropzone.min.css",
               "~/Content/Styles/pager.css",
               "~/Content/Styles/toastr.min.css",
               "~/Content/Styles/toastr-custom.css"
                ));

            bundles.Add(new StyleBundle("~/Content/theme4").Include(
                 "~/Content/Styles/jquery-ui.css",
               "~/Content/Styles/green.css",
                 "~/Content/Styles/custom_theme4.css",
               "~/Content/Styles/dropzone.min.css",
               "~/Content/Styles/pager.css",
               "~/Content/Styles/toastr.min.css",
               "~/Content/Styles/toastr-custom.css"
                ));
            bundles.Add(new StyleBundle("~/Content/theme5").Include(
               "~/Content/Styles/jquery-ui.css",
               "~/Content/Styles/green.css",
               "~/Content/Styles/custom_theme5.css",
             "~/Content/Styles/dropzone.min.css",
             "~/Content/Styles/pager.css",
             "~/Content/Styles/toastr.min.css",
             "~/Content/Styles/toastr-custom.css"
              ));
            bundles.Add(new StyleBundle("~/Content/theme6").Include(
               "~/Content/Styles/jquery-ui.css",
               "~/Content/Styles/green.css",
               "~/Content/Styles/custom_theme6.css",
             "~/Content/Styles/dropzone.min.css",
             "~/Content/Styles/pager.css",
             "~/Content/Styles/toastr.min.css",
             "~/Content/Styles/toastr-custom.css"
              ));
            bundles.Add(new StyleBundle("~/Content/theme7").Include(
               "~/Content/Styles/jquery-ui.css",
               "~/Content/Styles/green.css",
               "~/Content/Styles/custom_theme7.css",
             "~/Content/Styles/dropzone.min.css",
             "~/Content/Styles/pager.css",
             "~/Content/Styles/toastr.min.css",
             "~/Content/Styles/toastr-custom.css"
              ));
            bundles.Add(new StyleBundle("~/Content/theme8").Include(
               "~/Content/Styles/jquery-ui.css",
               "~/Content/Styles/green.css",
               "~/Content/Styles/custom_theme8.css",
             "~/Content/Styles/dropzone.min.css",
             "~/Content/Styles/pager.css",
             "~/Content/Styles/toastr.min.css",
             "~/Content/Styles/toastr-custom.css"

              ));

            var bundle = new ScriptBundle("~/bundles/js")
            .Include("~/Content/Scripts/plugins/jquery.min.js")
            .Include("~/Content/Scripts/plugins/jquery-ui.js")
            .Include("~/Content/Scripts/plugins/bootstrap.min.js")
            .Include("~/Content/Scripts/plugins/bootstrap-progressbar.min.js")
            .Include("~/Content/Scripts/plugins/date.js")
            .Include("~/Content/Scripts/plugins/moment.min.js")
            .Include("~/Content/Scripts/plugins/daterangepicker.js")
             .Include("~/Content/Scripts/plugins/bootstrap-timepicker.js")
            .Include("~/Content/Scripts/plugins/validator.min.js")
            .Include("~/Content/Scripts/plugins/icheck.min.js")
            .Include("~/Content/Scripts/plugins/fastclick.js")
            .Include("~/Content/Scripts/plugins/dropzone.min.js")
            .Include("~/Content/Scripts/plugins/Chart.bundle.js")
            .Include("~/Content/Scripts/plugins/googleapi.js")
            .Include("~/Content/Scripts/plugins/custom.min.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);


            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Content/Scripts/plugins/knockout-{version}.js",
                "~/Content/Scripts/plugins/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
               "~/Content/Scripts/application/ImageUpload.js",

               "~/Content/Scripts/application/pager.js",
               "~/Content/Scripts/application/toastr.min.js",
                "~/Content/Scripts/application/common.js",
              "~/Content/Scripts/viewModels/BuildingViewModel.js",
              "~/Content/Scripts/viewModels/DashboardViewModel.js",
              "~/Content/Scripts/viewModels/GatesViewModel.js",
              "~/Content/Scripts/viewModels/MyProfileViewModel.js",
              "~/Content/Scripts/viewModels/ChangePasswordViewModel.js",
              "~/Content/Scripts/viewModels/OrganizationViewModel.js",
              "~/Content/Scripts/viewModels/MyOrganizationViewModel.js",
              "~/Content/Scripts/viewModels/ScanVisitorViewModel.js",
              "~/Content/Scripts/viewModels/ShiftAssignmentViewModel.js",
              "~/Content/Scripts/viewModels/ShiftDetailsViewModel.js",
              "~/Content/Scripts/viewModels/ShiftViewModel.js",
              "~/Content/Scripts/viewModels/UsersViewModel.js",
              "~/Content/Scripts/viewModels/VisitorCheckInViewModel.js",
              "~/Content/Scripts/viewModels/VisitorCheckOutViewModel.js",
              "~/Content/Scripts/viewModels/VisitorDetailsViewModel.js",
              "~/Content/Scripts/viewModels/VisitorViewModel.js",
              "~/Content/Scripts/viewModels/ThemeSelectionViewModel.js",
              "~/Content/Scripts/application/config.js",
               "~/Content/Scripts/application/app.js"
              ));

        }
    }
}
