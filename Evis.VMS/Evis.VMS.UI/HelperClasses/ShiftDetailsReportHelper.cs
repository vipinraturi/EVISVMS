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

        public IList<ShiftReportGridVM> GetShiftData(string search, int pageIndex, int pageSize, string sortField, string sortOrder, int? orgId, out int totalCount)
        {
            IQueryable<ShiftReportGridVM> shiftQueryable = null;

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "FromDate";
                sortOrder = "ASC";
            }
            var searchDetails = JsonConvert.DeserializeObject<SearchShiftReport>(search);
            shiftQueryable = GetShiftDetails(orgId,searchDetails);

            var paginationRequest = new PaginationRequest
            {
                PageIndex = (pageIndex - 1),
                PageSize = pageSize,
                SearchText = search,
                Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
            };

            var result =
            GenericSorterPager.GetSortedPagedList<ShiftReportGridVM>(shiftQueryable, paginationRequest, out totalCount);

            result.ToList().ForEach(item =>
            {
               // item.strFromDate = item.FromDate.ToString("dd/MM/yyyy");
                //item.strToDate = item.ToDate.ToString("dd/MM/yyyy");
                item.ShiftName = item.ShiftName + " (" + item.Fromtime.ToString("hh:mm tt") + " - " + item.Totime.ToString("hh:mm tt") + ")";

            });
            return result;

        }

        

        private IQueryable<ShiftReportGridVM> GetShiftDetails(int? orgId, SearchShiftReport searchDetails)
        {
            var query = (from _shiftDetails in _genericService.ShiftDetails.GetAll()
                         join _shiftMaster in _genericService.ShitfMaster.GetAll() on _shiftDetails.ShiftID equals _shiftMaster.Id
                         join _gateMaster in _genericService.GateMaster.GetAll() on _shiftDetails.GateID equals _gateMaster.Id
                         join _buildingMaster in _genericService.BuildingMaster.GetAll() on _gateMaster.BuildingId equals _buildingMaster.Id
                         where (
                       ((searchDetails.BuildingID == 0) || (searchDetails.BuildingID == _shiftDetails.Gates.BuildingId)) &&
                             ((searchDetails.SecurityId == null) || (searchDetails.SecurityId == _shiftDetails.SecurityID)) &&
                            ((searchDetails.GateId == 0) || (searchDetails.GateId == _shiftDetails.GateID)) &&
                             ((searchDetails.ShiftID == 0) || (searchDetails.ShiftID == _shiftDetails.ShiftID))&&
                             ((searchDetails.FromDate == null) || (searchDetails.FromDate <= _shiftDetails.ShiftDate)) &&
                             ((searchDetails.ToDate == null) || (searchDetails.ToDate >= _shiftDetails.ShiftDate))
                             )
                                           

                         group new
                         {
                            
                             _shiftDetails.ShiftDate

                         }
                          by new
                          {
                              _shiftMaster.ShitfName,
                              _gateMaster.GateNumber,
                              _buildingMaster.BuildingName,
                              _shiftDetails.ApplicationUser.Id,
                              _shiftDetails.ApplicationUser.UserName,
                              _shiftMaster.FromTime,
                              _shiftMaster.ToTime,
                             // _shiftDetails.ShiftDate,
                              _shiftDetails.Gates.BuildingMaster.Organization.CompanyName

                          }
                             into g
                             select new 
                             {
                                 g.Key.ShitfName,
                                 g.Key.GateNumber,
                                 g.Key.BuildingName,
                                 g.Key.UserName,
                                 g.Key.FromTime,
                                 g.Key.ToTime,
                                 g.Key.CompanyName,
                                // g.Key.ShiftDate,


                                 ShiftDates = g.Select(st => st.ShiftDate).Distinct()
                             }).ToList().Select(l =>
                                        new ShiftReportGridVM
                                        {
                                          
                                           SecurityName=l.UserName,
                                           BuildingName = l.BuildingName,
                                           Gate = l.GateNumber,
                                           Fromtime=l.FromTime,
                                           Totime=l.ToTime,
                                           CompanyName=l.CompanyName,
                                           ShiftName=l.ShitfName,
                                          ShiftDates =string.Join(", ", l.ShiftDates.ToList())
                                        }
                                   ).AsQueryable();

            return query;


        }
        public IList<ShiftReportGridVM> GetShiftDataPrint(string search, int? orgId)
        {
            IQueryable<ShiftReportGridVM> shiftQueryable = null;
            var searchDetails = JsonConvert.DeserializeObject<SearchShiftReport>(search);
            shiftQueryable = GetShiftDetails(orgId, searchDetails);
            return shiftQueryable.ToList();
        }
    }
}