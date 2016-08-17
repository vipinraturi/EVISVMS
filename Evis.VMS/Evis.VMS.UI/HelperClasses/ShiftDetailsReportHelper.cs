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
       public IList<ShiftAssignmentVM> GetShiftData(string search, int pageIndex, int pageSize, string sortField, string sortOrder, out int totalCount)
       {
           var Shift = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true)
             .Select(x => new ShiftAssignmentVM
             {
                 BuildingId = x.BuildingId,
                 BuildingName = x.Gates.BuildingMaster.BuildingName,
                 GateId = x.GateId,
                 GateName = x.Gates.GateNumber,
                 ShitfId = x.ShitfId,
                 ShiftName = x.Shitfs.ShitfName,
                 UserId = x.UserId,
                 UserName = x.ApplicationUser.FullName,
                 FromDate = x.FromDate,
                 ToDate = x.ToDate,
                 strFromDate = x.ToDate.ToString(),
                 strToDate = x.ToDate.ToString(),

             }).ToList();
           if (string.IsNullOrEmpty(sortField))
           {
               sortField = "";
           }


           var searchDetails = JsonConvert.DeserializeObject<SearchShiftReport>(search);
           Shift = Shift.Where(
              x => (searchDetails == null ||
                   ((string.IsNullOrEmpty(searchDetails.SecurityId) || x.UserId == searchDetails.SecurityId) &&
                   (searchDetails.GateId == 0 || x.GateId == searchDetails.GateId) &&
                   (searchDetails.BuildingID == 0 || x.BuildingId == searchDetails.BuildingID) &&
                   (searchDetails.ShiftID==0 || x.ShitfId==searchDetails.ShiftID) &&
                   (string.IsNullOrEmpty(searchDetails.FromDate) || x.FromDate.ToString().Contains(searchDetails.FromDate.ToString())) &&
                   (string.IsNullOrEmpty(searchDetails.ToDate) || x.ToDate.ToString().Contains(searchDetails.ToDate.ToString()))))).ToList();
           var paginationRequest = new PaginationRequest
           {
               PageIndex = (pageIndex - 1),
               PageSize = pageSize,
               SearchText = search,
               Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
           };

           IList<ShiftAssignmentVM> result =
           GenericSorterPager.GetSortedPagedList<ShiftAssignmentVM>(Shift.AsQueryable(), paginationRequest, out totalCount);

           return result;

       }
        //*************************************
       public IList<ShiftAssignmentVM> GetShiftDataPrint(string search)
       {
           var Shift = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true)
             .Select(x => new ShiftAssignmentVM
             {
                 BuildingId = x.BuildingId,
                 BuildingName = x.Gates.BuildingMaster.BuildingName,
                 GateId = x.GateId,
                 GateName = x.Gates.GateNumber,
                 ShitfId = x.ShitfId,
                 ShiftName = x.Shitfs.ShitfName,
                 UserId = x.UserId,
                 UserName = x.ApplicationUser.FullName,
                 FromDate = x.FromDate,
                 ToDate = x.ToDate,
                 strFromDate = x.ToDate.ToString(),
                 strToDate = x.ToDate.ToString(),
                 CompanyName = x.BuildingMaster.Organization.CompanyName
             }).ToList();
          


           var searchDetails = JsonConvert.DeserializeObject<SearchShiftReport>(search);
           Shift = Shift.Where(
              x => (searchDetails == null ||
                   ((string.IsNullOrEmpty(searchDetails.SecurityId) || x.UserId == searchDetails.SecurityId) &&
                   (searchDetails.GateId == 0 || x.GateId == searchDetails.GateId) &&
                   (searchDetails.BuildingID == 0 || x.BuildingId == searchDetails.BuildingID) &&
                   (searchDetails.ShiftID == 0 || x.ShitfId == searchDetails.ShiftID) &&
                   (string.IsNullOrEmpty(searchDetails.FromDate) || x.FromDate.ToString().Contains(searchDetails.FromDate.ToString())) &&
                   (string.IsNullOrEmpty(searchDetails.ToDate) || x.ToDate.ToString().Contains(searchDetails.ToDate.ToString()))))).ToList();




           return Shift;

       }
    }
}