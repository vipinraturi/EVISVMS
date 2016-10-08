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
    public class ShiftDetailsReportHelper
    {
        GenericService _genericService = null;
        public ShiftDetailsReportHelper()
        {
            _genericService = new GenericService();
        }

        public IList<ShiftAssignmentVM> GetShiftData(string search, int pageIndex, int pageSize, string sortField, string sortOrder,int? orgId, out int totalCount)
        {
            IQueryable<ShiftAssignmentVM> shiftQueryable = null;

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "FromDate";
                sortOrder = "ASC";
            }
            var searchDetails = JsonConvert.DeserializeObject<SearchShiftReport>(search);
            shiftQueryable = GetShiftQueryable(orgId, searchDetails);

            var paginationRequest = new PaginationRequest
            {
                PageIndex = (pageIndex - 1),
                PageSize = pageSize,
                SearchText = search,
                Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
            };

            var result =
            GenericSorterPager.GetSortedPagedList<ShiftAssignmentVM>(shiftQueryable, paginationRequest, out totalCount);

            result.ToList().ForEach(item =>
            {
                item.strFromDate = item.FromDate.ToString("dd/MM/yyyy");
                item.strToDate = item.ToDate.ToString("dd/MM/yyyy");
                item.ShiftName = item.ShiftName + " (" + item.FromDate.ToString("hh:mm tt") + " - " + item.ToDate.ToString("hh:mm tt") + ")";

            });
            return result;

        }

        private IQueryable<ShiftAssignmentVM> GetShiftQueryable(int? orgId, SearchShiftReport searchDetails)
        {

            if ((string.IsNullOrEmpty(searchDetails.FromDate)))
            {
                searchDetails.FromDate = null;

            }
            if ((string.IsNullOrEmpty(searchDetails.ToDate)))
            {
                searchDetails.ToDate = null;
            }
            DateTime fromdat = Convert.ToDateTime(searchDetails.FromDate);
            DateTime Todat = Convert.ToDateTime(searchDetails.ToDate);
            if (Todat.Date == DateTime.MinValue || Todat == null)
            {
                Todat = DateTime.MaxValue;
            }


            IQueryable<ShiftAssignmentVM> shiftQueryable = _genericService.ShiftDetails.GetAll()
                .Where(x => x.IsActive
                    && orgId != 0 ? x.ApplicationUser.OrganizationId == orgId : true
                    && (searchDetails.BuildingID == 0 || searchDetails.BuildingID == x.Gates.BuildingId)
                    && (searchDetails.GateId == 0 || searchDetails.GateId == x.GateID)
                    && (string.IsNullOrEmpty(searchDetails.SecurityId) || x.ApplicationUser.Id == searchDetails.SecurityId)
                    && (searchDetails.ShiftID == 0 || x.ShiftID == searchDetails.ShiftID)
                   && ((fromdat == null) || fromdat <= x.ShiftDate)
                  && ((Todat == null) || Todat >= x.ShiftDate)
                    )
                //.GroupBy(x => x.ShiftID)

                 .Select(x => new ShiftAssignmentVM
                 {
                     BuildingId = x.Gates.BuildingId,
                     BuildingName = x.Gates.BuildingMaster.BuildingName,
                     GateId = x.GateID,
                     GateName = x.Gates.GateNumber,
                     ShitfId = x.ShiftID,
                     ShiftName = x.Shitfs.ShitfName,
                     UserId = x.ApplicationUser.Id,
                     UserName = x.ApplicationUser.FullName,
                     FromDate = x.ShiftDate,
                     ToDate = x.ShiftDate,
                     OrganizationId = x.ApplicationUser.OrganizationId,
                     CompanyName = x.Gates.BuildingMaster.Organization.CompanyName

                 }).AsQueryable();

            return shiftQueryable;
        }
       

        public IList<ShiftAssignmentVM> GetShiftDataPrint(string search, int? orgId)
        {
            IQueryable<ShiftAssignmentVM> shiftQueryable = null;
            var searchDetails = JsonConvert.DeserializeObject<SearchShiftReport>(search);
            shiftQueryable = GetShiftQueryable(orgId, searchDetails);
            return shiftQueryable.ToList();
        }
    }
}