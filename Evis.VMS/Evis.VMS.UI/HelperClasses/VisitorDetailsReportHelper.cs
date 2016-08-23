/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.ViewModel;
using Evis.VMS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.HelperClasses
{
    public class VisitorDetailsReportHelper
    {
        GenericService _genericService = null;
        public VisitorDetailsReportHelper()
        {
            _genericService = new GenericService();
        }

        public IList<VisitorsDetailsVM> VisitorData(string search, int pageIndex, int pageSize, string sortField, string sortOrder, out int totalCount)
        {
            var visitorsDetails = (from vm in _genericService.VisitorMaster.GetAll()
                                   join vd in _genericService.VisitDetails.GetAll()
                                   on vm.Id equals vd.VisitorId
                                   select new VisitorsDetailsVM
                                   {
                                       //VisitorId = vm.Id,
                                       VisitorName = vm.VisitorName,
                                       CheckIn = vd.CheckIn.ToString(),
                                       CheckOut = vd.CheckOut.ToString(),
                                       ContactNumber = vm.ContactNo,
                                       VisitDetails = vd.PurposeOfVisit,
                                       BuildingId = vd.GateMaster.BuildingId,
                                       GateId = vd.CheckInGate,
                                       SecurityId = vd.CreatedBy,
                                       Building = vd.GateMaster.BuildingMaster.BuildingName,
                                       Gate = vd.GateMaster.GateNumber,
                                       Security = vd.CreatedUser.FullName,
                                       CompanyName = vd.GateMaster.BuildingMaster.Organization.CompanyName
                                   }).ToList();


            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "CheckIn";
                sortOrder = "DES";
            }

            var searchDetails = JsonConvert.DeserializeObject<SearchVisitorVM>(search);
            string targetDate = "";
            if (!string.IsNullOrEmpty(searchDetails.CheckOut))
            {
                DateTime toDateFormat = Convert.ToDateTime(searchDetails.CheckOut);
                targetDate = toDateFormat.AddDays(1).ToString();
            }
            visitorsDetails = visitorsDetails.Where(
               x => (searchDetails == null ||
                    ((string.IsNullOrEmpty(searchDetails.SecurityId) || x.SecurityId == searchDetails.SecurityId) &&
                    (searchDetails.GateId == 0 || x.GateId == searchDetails.GateId) &&
                    (searchDetails.BuildingId == 0 || x.BuildingId == searchDetails.BuildingId) &&
                    (string.IsNullOrEmpty(searchDetails.VisitorName) || x.VisitorName.ToLower().Contains(searchDetails.VisitorName.ToLower())) &&
                    (string.IsNullOrEmpty(searchDetails.CheckIn) || Convert.ToDateTime(x.CheckIn) >= Convert.ToDateTime(searchDetails.CheckIn))
                    && (string.IsNullOrEmpty(targetDate) || Convert.ToDateTime(x.CheckIn) <= Convert.ToDateTime(targetDate))
                    )
                    )).ToList();
            //   visitorsDetails = visitorsDetails.OrderBy(x => x.CheckIn).ToList();
            //}

            //creating pager object to send for filtering and sorting
            var paginationRequest = new PaginationRequest
            {
                PageIndex = (pageIndex - 1),
                PageSize = pageSize,
                SearchText = search,
                Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
            };

            IList<VisitorsDetailsVM> result =
            GenericSorterPager.GetSortedPagedList<VisitorsDetailsVM>(visitorsDetails.AsQueryable(), paginationRequest, out totalCount);

            return result;

        }

        public IList<VisitorsDetailsVM> PrintVisitorData(string searchDetailss)
        {
            var visitorsDetails = (from vm in _genericService.VisitorMaster.GetAll()
                                   join vd in _genericService.VisitDetails.GetAll()
                                   on vm.Id equals vd.VisitorId
                                   select new VisitorsDetailsVM
                                   {
                                       VisitorId = vm.Id,
                                       VisitorName = vm.VisitorName,
                                       CheckIn = vd.CheckIn.ToString(),
                                       CheckOut = vd.CheckOut.ToString(),
                                       ContactNumber = vm.ContactNo,
                                       VisitDetails = vd.PurposeOfVisit,
                                       BuildingId = vd.GateMaster.BuildingId,
                                       GateId = vd.CheckInGate,
                                       SecurityId = vd.CreatedBy,
                                       Building = vd.GateMaster.BuildingMaster.BuildingName,
                                       Gate = vd.GateMaster.GateNumber,
                                       Security = vd.CreatedUser.FullName,
                                       CompanyName = vd.GateMaster.BuildingMaster.Organization.CompanyName
                                   }).ToList();
            var searchDetails = JsonConvert.DeserializeObject<SearchVisitorVM>(searchDetailss);
            string targetDate = "";
            if (!string.IsNullOrEmpty(searchDetails.CheckOut))
            {
                DateTime toDateFormat = Convert.ToDateTime(searchDetails.CheckOut);
                targetDate = toDateFormat.AddDays(1).ToString();
            }
            visitorsDetails = visitorsDetails.Where(
               x => (searchDetails == null ||
                    ((string.IsNullOrEmpty(searchDetails.SecurityId) || x.SecurityId == searchDetails.SecurityId) &&
                    (searchDetails.GateId == 0 || x.GateId == searchDetails.GateId) &&
                    (searchDetails.BuildingId == 0 || x.BuildingId == searchDetails.BuildingId) &&
                    (string.IsNullOrEmpty(searchDetails.VisitorName) || x.VisitorName.Contains(searchDetails.VisitorName)) &&
                    (string.IsNullOrEmpty(searchDetails.CheckIn) || Convert.ToDateTime(x.CheckIn) >= Convert.ToDateTime(searchDetails.CheckIn))
                    && (string.IsNullOrEmpty(targetDate) || Convert.ToDateTime(x.CheckIn) <= Convert.ToDateTime(targetDate))
                    )
                    )).ToList();

            return visitorsDetails;
        }
    }

}