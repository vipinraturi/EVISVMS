using Evis.VMS.Business;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Evis.VMS.Utilities;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class DashboardController : BaseApiController
    {

        GenericService _genericService = new GenericService();
        private readonly UserService _userService = new UserService();
        //
        // GET: /Dashboard/


        [Route("~/Api/DashBoard/LoadGraph")]
        [HttpGet]
        public async Task<List<Tuple<int, string,DateTime>>> GetGraphData()//, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {

            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = await _userService.GetAsync(x => x.Id == currentUserId);
            var myData = from visitorDetails in _genericService.VisitDetails.GetAll()
                         where visitorDetails.GateMaster.BuildingMaster.OrganizationId == (currentUser.OrganizationId==null ? 1:currentUser.OrganizationId)
                         group visitorDetails by EntityFunctions.TruncateTime(visitorDetails.CheckIn) into g
                         
                         orderby g.Key
                         select new { CreateTime = g.Key, Count = g.Count() };

            //var ShitfMaster = _genericService.VisitDetails.GetAll().GroupBy(i => i.CheckIn.ToString("yyyyMMdd"))
            //             .Select(i => new
            //             {
            //                 Date = DateTime.ParseExact(i.Key, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
            //                 Count = i.Count()
            //             });

            //GraphDataList obj = new GraphDataList();
            //// string q;
            //List<GraphVM> onjvm = new List<GraphVM>();

            //string[] formats = { "dd/MM/yyyy" };
            ////onjvm.Add(new GraphVM() { Date = "12/07", Count = 10 });
            ////DateTime.Parse("13/07/2016",new CultureInfo("en-US", true))
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("13/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 14 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("14/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 11 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("15/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 18 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("16/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 16 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("17/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 12 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("18/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 19 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("19/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 13 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("20/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 11 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("21/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 14 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("22/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 11 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("23/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 18 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("24/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 16 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("25/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 12 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("26/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 19 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("27/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 13 });
            //onjvm.Add(new GraphVM() { Date = DateTime.ParseExact("28/07/2016", formats, new CultureInfo("en-US"), DateTimeStyles.None), Count = 11 });
            ////obj.GraphList = onjvm;
            //  return onjvm;
            List<Tuple<int, string,DateTime>> mydict = new List<Tuple<int, string,DateTime>>();

            // add an item

            //q = "[";
            var currentDate = DateTime.Now;
            var lastDate = DateTime.Now.AddDays(-15);
            List<string> dates = new List<string>();
            

            
           
            foreach (var item in myData)
            {
                if (item.CreateTime >= lastDate && item.CreateTime < currentDate)
                {
                    mydict.Add(new Tuple<int, string,DateTime>(item.Count, (Convert.ToDateTime(item.CreateTime)).ToShortDateString(),Convert.ToDateTime(item.CreateTime)));
                }
                // mydict.Add(item.Count, item.Date);
                // q=q+"["+item.Date+","+item.Count+"],";
            }

            foreach (DateTime day in EachDay(lastDate, currentDate))
            {
                if(!mydict.Any(s=>s.Item2==day.Date.ToShortDateString()))
                {
                    mydict.Add(new Tuple<int, string,DateTime>(0, (Convert.ToDateTime(day.Date)).ToShortDateString(),day));
                }
            }
            return mydict.OrderByDescending(x => x.Item3).ToList();
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }


        [Route("~/Api/DashBoard/DisplayAllShift")]
        [HttpPost]
        public string DisplayAllShift(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
           // .Where(t => t.ExpirationDate == null || (t.ExpirationDate != null && DbFunctions.TruncateTime(t.ExpirationDate.Value) > DbFunctions.TruncateTime(DateTime.Now)))
            //.Where(x => x.IsActive == true && x.FromDate <= DateTime.Now && x.ToDate >= DateTime.Now)
            var a =_genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true && ((EntityFunctions.TruncateTime(x.FromDate) >= EntityFunctions.TruncateTime(DateTime.Now)
                || EntityFunctions.TruncateTime(x.ToDate) <= EntityFunctions.TruncateTime(DateTime.Now))) && EntityFunctions.TruncateTime(x.ToDate) <= EntityFunctions.TruncateTime(DateTime.Now )).AsQueryable();

            var ShiftDisplay = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true && ((EntityFunctions.TruncateTime(x.FromDate) >= EntityFunctions.TruncateTime(DateTime.Now)
                || EntityFunctions.TruncateTime(x.ToDate) <= EntityFunctions.TruncateTime(DateTime.Now))) && EntityFunctions.TruncateTime(x.ToDate) <= EntityFunctions.TruncateTime(DateTime.Now ))
                .Select(x => new ShiftAssignmentVM
                {
                    UserId = x.UserId,
                    UserName = x.ApplicationUser.FullName,
                    BuildingId = x.BuildingId,
                    BuildingName = x.Gates.BuildingMaster.BuildingName,
                    GateId = x.GateId,
                    GateName = x.Gates.GateNumber,
                    ShitfId = x.ShitfId,
                    ShiftName = x.Shitfs.ShitfName


                }).ToList();
            var searchDetails = JsonConvert.DeserializeObject<SearchShiftReport>(globalSearch);
           
            //ShiftDisplay = ShiftDisplay.Where(
            // x => (searchDetails == null ||
            //      ((string.IsNullOrEmpty(searchDetails.SecurityId) || x.UserId == searchDetails.SecurityId) &&
            //      (searchDetails.GateId == 0 || x.GateId == searchDetails.GateId) &&
            //      (searchDetails.BuildingID == 0 || x.BuildingId == searchDetails.BuildingID) &&
            //      (searchDetails.ShiftID == 0 || x.ShitfId == searchDetails.ShiftID) &&
            //      (string.IsNullOrEmpty(searchDetails.FromDate) || x.FromDate.ToString().Contains(searchDetails.FromDate.ToString())) &&
            //      (string.IsNullOrEmpty(searchDetails.ToDate) || x.ToDate.ToString().Contains(searchDetails.ToDate.ToString()))))).ToList();
            var paginationRequest = new PaginationRequest
            {
                PageIndex = (pageIndex - 1),
                PageSize = pageSize,
                SearchText = globalSearch,
                Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
            };
            int totalCount = 0;
            IList<ShiftAssignmentVM> result =
            GenericSorterPager.GetSortedPagedList<ShiftAssignmentVM>(ShiftDisplay.AsQueryable(), paginationRequest, out totalCount);

            var jsonData = JsonConvert.SerializeObject(ShiftDisplay.OrderBy(x => x.UserName));
            return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            //return Shift;

        }

    }
}