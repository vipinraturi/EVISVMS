﻿/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace Evis.VMS.UI.Controllers
{
    public partial class ReportController 
    {
        public ActionResult _ShiftDetailsReport()
        {
            return View();
        }

        public async Task<ActionResult> ShiftReportExcelDownload(string searchData)
        {
            Reports.DataSet.ShiftDetailReportDataSet shiftDetailDataset = new Reports.DataSet.ShiftDetailReportDataSet();
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == System.Web.HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            int? orgId = 0;
            if(user!=null)
            {
                orgId = user.OrganizationId;
            }
            var result = _ShiftDetailsReportHelper.GetShiftDataPrint(searchData,orgId);

            foreach (var item in result)
            {
                var TabledataRow = shiftDetailDataset.ShiftDetailDatatable.NewShiftDetailDatatableRow();
                TabledataRow.FullName = item.SecurityName;
                TabledataRow.BuildingName = item.BuildingName;
                TabledataRow.GateNumber = item.Gate;
                TabledataRow.ShiftName = item.ShiftName + " (" + item.Fromtime.ToString("hh:mm tt") + " - " + item.Totime.ToString("hh:mm tt") + ")";
                TabledataRow.FromDate = item.ShiftDates.ToString(); 
                //TabledataRow.ToDate = item.ToDate.ToString();
                TabledataRow.CompanyName = item.CompanyName;
                shiftDetailDataset.ShiftDetailDatatable.AddShiftDetailDatatableRow(TabledataRow);
            }
            var reportData = new ReportDataSource("ShiftDetailReportDataset", shiftDetailDataset.Tables[0]);
            var reportViewer = new ReportViewer { ProcessingMode = ProcessingMode.Local };
            reportViewer.LocalReport.ReportPath = GetReportPath() + "\\ShiftDetailReport.rdlc";
            Warning[] warnings;
            string[] streams;
            string MIMETYPE = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            reportViewer.LocalReport.DataSources.Add(reportData);
            byte[] bytes = reportViewer.LocalReport.Render("Excel", null, out MIMETYPE, out encoding, out extension, out streams, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = MIMETYPE;
            Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
            return new EmptyResult();
        }

        public async Task<ActionResult> PrintShiftDetailReport(string searchData)
        {
            Reports.DataSet.ShiftDetailReportDataSet shiftDetailDataset = new Reports.DataSet.ShiftDetailReportDataSet();
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == System.Web.HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            int? orgId = 0;
            if (user != null)
            {
                orgId = user.OrganizationId;
            }
            var result = _ShiftDetailsReportHelper.GetShiftDataPrint(searchData, orgId);

            foreach (var item in result)
            {
                var TabledataRow = shiftDetailDataset.ShiftDetailDatatable.NewShiftDetailDatatableRow();
                TabledataRow.FullName = item.SecurityName;
                TabledataRow.BuildingName = item.BuildingName;
                TabledataRow.GateNumber = item.Gate;
                TabledataRow.ShiftName = item.ShiftName + " (" + item.Fromtime.ToString("hh:mm tt") + " - " + item.Totime.ToString("hh:mm tt") + ")";
                TabledataRow.FromDate = item.ShiftDates.ToString();
                //TabledataRow.ToDate = item.ToDate.ToString("dd/MM/yyyy");
                TabledataRow.CompanyName = item.CompanyName;

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

    }
}