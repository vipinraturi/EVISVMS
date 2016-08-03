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
using System.Web.Script.Serialization;

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
        public string GetVisitorData(string search, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC", string globalSearch = "")
        {
            var visitorsDetails = (from vm in _genericService.VisitorMaster.GetAll()
                                   //.Where(x => searchDetails != null && (searchDetails.SecurityId == "" || x.CreatedBy == searchDetails.SecurityId))
                                   join vd in _genericService.VisitDetails.GetAll()
                                       //.Where(x => searchDetails != null &&
                                       //      (searchDetails.GateId == 0 || x.CheckInGate == searchDetails.GateId || x.CheckOutGate == searchDetails.GateId) &&
                                       //      (searchDetails.BuildingId == 0 || x.GateMaster.BuildingId == searchDetails.BuildingId))
                                   on vm.Id equals vd.VisitorId
                                   select new VisitorsDetailsVM
                                   {
                                       VisitorId = vm.Id,
                                       VisitorName = vm.VisitorName,
                                       VisitDate = vm.CreatedDate,
                                       CheckIn = vd.CheckIn,
                                       CheckOut = vd.CheckOut,
                                       ContactNumber = vm.ContactNo,
                                       VisitDetails = vd.PurposeOfVisit,
                                       BuildingId = vd.GateMaster.BuildingId,
                                       GateId = vd.CheckInGate,
                                       SecurityId = vm.CreatedBy
                                       //FromDate  = vm.end
                                   }).ToList();

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
                var searchDetails = JsonConvert.DeserializeObject<SearchVisitorVM>(search);

                //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                //SearchVisitorVM searchDetails = (SearchVisitorVM)json_serializer.DeserializeObject(search);

                visitorsDetails = visitorsDetails.Where(
                   x => (searchDetails == null ||
                        ((searchDetails.SecurityId == "" || x.SecurityId == searchDetails.SecurityId) &&
                        (searchDetails.GateId == 0 || x.GateId == searchDetails.GateId || x.GateId == searchDetails.GateId) &&
                        (searchDetails.BuildingId == 0 || x.BuildingId == searchDetails.BuildingId) &&
                        (searchDetails.VisitorName == "" || x.VisitorName.Contains(searchDetails.VisitorName)) &&
                        (searchDetails.FromDate == "" || x.FromDate.Contains(searchDetails.FromDate)) &&
                        (searchDetails.ToDate == "" || x.ToDate.Contains(searchDetails.ToDate)))
                        )).ToList();
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
                    GenericSorterPager.GetSortedPagedList<VisitorsDetailsVM>(visitorsDetails.AsQueryable(), paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result.OrderBy(x => x.VisitorName));
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }

            return null;


        }
    }
}
