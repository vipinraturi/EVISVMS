/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evis.VMS.UI.Controllers
{
    public class ReportController : Controller
    {
        public readonly VisitorDetailsReportHelper _visitorDetailsReportHelper = null;
        public readonly ShiftDetailsReportHelper _ShiftDetailsReportHelper = null;

        public ReportController()
        {
            _visitorDetailsReportHelper = new VisitorDetailsReportHelper();
            _ShiftDetailsReportHelper = new ShiftDetailsReportHelper();
        }

        public ActionResult _VisitorDetailsReport()
        {
            return View();
        }

        public ActionResult _ShiftDetailsReport()
        {
            return View();
        }


        public ActionResult PrintShiftDetailReport(string searchData)
        {
            Reports.DataSet.ShiftDetailReportDataset shiftDetailDataset = new Reports.DataSet.ShiftDetailReportDataset();

            var result = _ShiftDetailsReportHelper.GetShiftDataPrint(searchData);

            foreach (var item in result)
            {
                var TabledataRow = shiftDetailDataset.ShiftDetailDatatable.NewShiftDetailDatatableRow();
                TabledataRow.FullName = item.UserName;
                TabledataRow.BuildingName = item.BuildingName;
                TabledataRow.GateNumber = item.GateName;
                TabledataRow.ShiftName = item.ShiftName;
                TabledataRow.FromDate = item.FromDate.ToString();
                TabledataRow.ToDate = item.ToDate.ToString();

                shiftDetailDataset.ShiftDetailDatatable.AddShiftDetailDatatableRow(TabledataRow);
            }
                var reportData = new ReportDataSource("ShiftDetailReportDataset", shiftDetailDataset.Tables[0]);
                var reportViewer = new ReportViewer { ProcessingMode = ProcessingMode.Local };
                reportViewer.LocalReport.ReportPath = GetReportPath() + "\\ShiftDetailReport.rdlc";
                Warning[] warnings;
                string[] streamIds;
                string mimeType;
                string encoding;
                string extension;

                reportViewer.LocalReport.DataSources.Add(reportData);

                byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
                System.Web.HttpContext.Current.Response.Flush();


            
            return new EmptyResult();
        }

        public ActionResult PrintVisitorsDetailsReport(string search)
        {
            Reports.DataSet.VisitorsDetailsDataSet visitorsDetailsDataSet = new Reports.DataSet.VisitorsDetailsDataSet();

            var result = _visitorDetailsReportHelper.PrintVisitorData(search);

            foreach (var item in result)
            {
                var InvoiceRow = visitorsDetailsDataSet.DTVisitorsDetails.NewDTVisitorsDetailsRow();
                InvoiceRow.VisitorName = item.VisitorName;
                InvoiceRow.BuildingName = item.Building;
                InvoiceRow.GateName = item.Gate;
                InvoiceRow.SecurityPerson = item.Security;
                InvoiceRow.CheckIn =item.CheckIn;
                //InvoiceRow.CheckOut = string.IsNullOrEmpty(item.CheckOut) ? null : Convert.ToDateTime(item.CheckOut);
                InvoiceRow.CheckOut = item.CheckOut;
                InvoiceRow.ContactNumber = item.ContactNumber;
                visitorsDetailsDataSet.DTVisitorsDetails.AddDTVisitorsDetailsRow(InvoiceRow);
            }

            var reportData = new ReportDataSource("VisitorsDetailsDataSet", visitorsDetailsDataSet.Tables[0]);
            var viewer = new ReportViewer { ProcessingMode = ProcessingMode.Local };
            viewer.LocalReport.ReportPath = GetReportPath() + "\\VisitorsDetailsReport.rdlc";

            Warning[] warnings;
            string[] streamIds;
            string mimeType;
            string encoding;
            string extension;

            viewer.LocalReport.DataSources.Add(reportData);

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            #region "Save report in PDF format"

            //SaveReportInPDFFormat(bytes, "E:/Shambhoo/PatientSummaryReport.pdf");

            #endregion

            #region "Show RDLC Report in PDF Format"

            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.BufferOutput = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.BufferOutput = true;

            byte[] fileToReturn = new byte[byte.MaxValue];

            System.Web.HttpContext.Current.Response.BinaryWrite(fileToReturn);

            #endregion

            return new EmptyResult();
        }

        private string GetReportPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Reports\\RDLC";
        }
        //smitha added
        //public ActionResult GenerateRDLCReport(string searchData)
        //{
        //    //string search = "{}";
        //    int pageIndex = 1;
        //    int pageSize = 7;
        //    string sortField = "";
        //    string sortOrder = "ASC";
        //    int totalCount = 0;
        //    var visitorDetailsList = _visitorDetailsReportHelper.GetVisitorData(searchData, pageIndex, pageSize, sortField, sortOrder, out totalCount);

        //    var visitorDetailDataSet = new Reports.DataSet.VisitorsDetailsDataSet();
        //    foreach (var visitorDetails in visitorDetailsList)
        //    {
        //        var visitorDetailDataRow = visitorDetailDataSet.DTVisitorsDetails.NewDTVisitorsDetailsRow();

        //        visitorDetailDataRow.VisitorName = visitorDetails.VisitorName;
        //        visitorDetailDataRow.BuildingName = visitorDetails.Building;
        //        visitorDetailDataRow.GateName = visitorDetails.Gate;
        //        visitorDetailDataRow.SecurityPerson = visitorDetails.Security;
        //        visitorDetailDataRow.CheckIn = visitorDetails.CheckIn;
        //        visitorDetailDataRow.CheckOut = visitorDetails.CheckOut;
        //        visitorDetailDataRow.ContactNumber = visitorDetails.ContactNumber;

        //        visitorDetailDataSet.DTVisitorsDetails.AddDTVisitorsDetailsRow(visitorDetailDataRow);
        //    }

        //    var visitorDetailReportData = new ReportDataSource("VisitorsDetailsDataSet", visitorDetailDataSet.Tables[0]);
        //    var viewer = new ReportViewer { ProcessingMode = ProcessingMode.Local };
        //    viewer.LocalReport.ReportPath = "Reports/RDLC/VisitorsDetailsReport.rdlc";
        //    viewer.LocalReport.DataSources.Add(visitorDetailReportData);

        //    byte[] bytes = null;
        //    Warning[] warnings;
        //    string[] streamIds;
        //    string mimeType;
        //    string encoding;
        //    string extension;
        //    bytes = viewer.LocalReport.Render("pdf", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

        //    System.Web.HttpContext.Current.Response.Buffer = true;
        //    System.Web.HttpContext.Current.Response.Clear();
        //    System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
        //    System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
        //    System.Web.HttpContext.Current.Response.Flush();


        //    return new EmptyResult();
        //}

        public ActionResult PrintVisitorsDetailsReportExcel(string search)
        {
            Reports.DataSet.VisitorsDetailsDataSet visitorsDetailsDataSet = new Reports.DataSet.VisitorsDetailsDataSet();

            var result = _visitorDetailsReportHelper.PrintVisitorData(search);

            foreach (var item in result)
            {
                var InvoiceRow = visitorsDetailsDataSet.DTVisitorsDetails.NewDTVisitorsDetailsRow();
                InvoiceRow.VisitorName = item.VisitorName;
                InvoiceRow.BuildingName = item.Building;
                InvoiceRow.GateName = item.Gate;
                InvoiceRow.SecurityPerson = item.Security;
                InvoiceRow.CheckIn = item.CheckIn;
                //InvoiceRow.CheckOut = string.IsNullOrEmpty(item.CheckOut) ? null : Convert.ToDateTime(item.CheckOut);
                InvoiceRow.CheckOut = item.CheckOut;
                InvoiceRow.ContactNumber = item.ContactNumber;
                visitorsDetailsDataSet.DTVisitorsDetails.AddDTVisitorsDetailsRow(InvoiceRow);
            }
            var reportData = new ReportDataSource("VisitorsDetailsDataSet", visitorsDetailsDataSet.Tables[0]);
            var viewer = new ReportViewer { ProcessingMode = ProcessingMode.Local };
            viewer.LocalReport.ReportPath = GetReportPath() + "\\VisitorsDetailsReport.rdlc";
            Warning[] warnings;
            string[] streams;
            string MIMETYPE = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            viewer.LocalReport.DataSources.Add(reportData);
            byte[] bytes = viewer.LocalReport.Render("Excel", null, out MIMETYPE, out encoding, out extension, out streams, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = MIMETYPE;
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
            return new EmptyResult();
        }

    }
}