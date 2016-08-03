/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Evis.VMS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class ReportController
    {
        public readonly GenericService _genericService = null;

        public ReportController()
        {
            _genericService = new GenericService();
        }

        [Route("~/Api/VisitorsDetails/GetVisitorsDetails")]
        [HttpPost]
        public string GetVisitorData(VisitorsDetailsVM searchDetails, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC", string globalSearch = "")
        {
            var visitorsDetails = (from vm in _genericService.VisitorMaster.GetAll()
                                       .Where(x => searchDetails != null && (searchDetails.SecurityId == "" || x.CreatedBy == searchDetails.SecurityId))
                                   join vd in _genericService.VisitDetails.GetAll()
                                       .Where(x => searchDetails != null &&
                                             (searchDetails.GateId == 0 || x.CheckInGate == searchDetails.GateId || x.CheckOutGate == searchDetails.GateId) &&
                                             (searchDetails.BuildingId == 0 || x.GateMaster.BuildingId == searchDetails.BuildingId))
                                   on vm.Id equals vd.VisitorId
                                   select new VisitorsDetailsVM
                                   {
                                       VisitorId = vm.Id,
                                       VisitorName = vm.VisitorName,
                                       VisitDate = vm.CreatedDate,
                                       CheckIn = vd.CheckIn,
                                       CheckOut = vd.CheckOut,
                                       ContactNumber = vm.ContactNo,
                                       VisitDetails = vd.PurposeOfVisit
                                   }).AsQueryable();

            //var user = (await .GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "VisitorName";
            }

            if (string.IsNullOrEmpty(globalSearch))
            {
                globalSearch = "";
            }

            if (visitorsDetails.Count() > 0)
            {
                //if (!string.IsNullOrEmpty(globalSearch))
                //{
                //    visitorsDetails = visitorsDetails.Where(item =>
                //        item.CompanyName.ToLower().Contains(globalSearch.ToLower()) ||
                //        item.Country.ToLower().Contains(globalSearch.ToLower()) ||
                //        item.WebSite.ToLower().Contains(globalSearch.ToLower())
                //        ).AsQueryable();
                //}

                //creating pager object to send for filtering and sorting
                var paginationRequest = new PaginationRequest
                {
                    PageIndex = (pageIndex - 1),
                    PageSize = pageSize,
                    SearchText = globalSearch,
                    Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
                };

                int totalCount = 0;
                IList<VisitorsDetailsVM> result =
                    GenericSorterPager.GetSortedPagedList<VisitorsDetailsVM>(visitorsDetails, paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result.OrderBy(x => x.VisitorName));
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }

            return null;


        }
    }
}
