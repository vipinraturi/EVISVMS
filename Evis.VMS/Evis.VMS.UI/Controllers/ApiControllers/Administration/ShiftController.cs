/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Model.Entities;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Evis.VMS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        [Route("~/Api/Shift/SaveShift")]
        [HttpPost]
        public ReturnResult Saveshift([FromBody]  ShitfMaster ShiftDetailsVM)
        {

            if (ShiftDetailsVM.Id == 0)
            {
                ShiftDetailsVM.IsActive = true;
                _genericService.ShitfMaster.Insert(ShiftDetailsVM);
            }

            else
            {
                var Shift = _genericService.ShitfMaster.GetById(ShiftDetailsVM.Id);
                if (Shift != null)
                {
                    Shift.ShitfName = ShiftDetailsVM.ShitfName;
                    Shift.FromTime = ShiftDetailsVM.FromTime;
                    Shift.ToTime = ShiftDetailsVM.ToTime;
                    ShiftDetailsVM.IsActive = true;
                    _genericService.ShitfMaster.Update(ShiftDetailsVM);
                };
            }
            _genericService.Commit();
            return new ReturnResult { Message = "Success", Success = true };
        }
        [Route("~/Api/Shift/GetAllShift")]
        [HttpPost]
        public string GetAllshift(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var ShitfMaster = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true).AsEnumerable()
                            .Select(x => new ShiftDetailsVM
                            {
                                Id = x.Id,
                                ShitfName = x.ShitfName,
                                ToTime = (x.ToTime),
                                FromTime = x.FromTime,
                                strFromTime = x.FromTime.ToString("hh:mm tt"),
                                strToTime = x.ToTime.ToString("hh:mm tt")
                            }).AsQueryable();
            if (ShitfMaster.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    ShitfMaster = ShitfMaster.Where(item =>
                        item.ShitfName.ToLower().Contains(globalSearch.ToLower())
                        ).AsQueryable();
                }
                var paginationRequest = new PaginationRequest
                             {
                                 PageIndex = (pageIndex - 1),
                                 PageSize = pageSize,
                                 SearchText = globalSearch,
                                 Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
                             };
                int totalCount = 0;
                IList<ShiftDetailsVM> result =
                   GenericSorterPager.GetSortedPagedList<ShiftDetailsVM>(ShitfMaster, paginationRequest, out totalCount);
                var jsonData = JsonConvert.SerializeObject(result);
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }
            return null;

        }
        [Route("~/Api/Shift/DeleteShift")]
        [HttpPost]
        public ReturnResult Deleteswift([FromBody] ShitfMaster ShitfMaster)
        {
            if (ShitfMaster != null)
            {
                var ShitfDelete = _genericService.ShitfMaster.GetAll().Where(x => x.Id == ShitfMaster.Id).FirstOrDefault();
                if (ShitfDelete != null)
                {
                    ShitfDelete.IsActive = false;
                    _genericService.ShitfMaster.Update(ShitfDelete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Success", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }
    }
}
