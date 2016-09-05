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
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        IQueryable<ShiftDetailsVM> LstShiftDetailsVM;
        [Route("~/Api/Shift/SaveShift")]
        [HttpPost]
        public ReturnResult Saveshift([FromBody]  ShitfMaster ShiftDetail)
        {
            bool success = false;
            string Message = "";
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            if (ShiftDetail.Id == 0)
            {
                var data = _genericService.ShitfMaster.GetAll().Where(x => x.ShitfName == ShiftDetail.ShitfName.Trim()).ToList();
                if (data.Count() == 0)
                {
                    var result = _genericService.ShitfMaster.GetAll().Where(y => DbFunctions.CreateTime(y.ToTime.Hour, y.ToTime.Minute, y.ToTime.Second) == DbFunctions.CreateTime(ShiftDetail.ToTime.Hour, ShiftDetail.ToTime.Minute, ShiftDetail.ToTime.Second)
                        && DbFunctions.CreateTime(y.FromTime.Hour, y.FromTime.Minute, y.FromTime.Second) == DbFunctions.CreateTime(ShiftDetail.FromTime.Hour, ShiftDetail.FromTime.Minute, ShiftDetail.FromTime.Second)).ToList();
                    if (ShiftDetail.FromTime != ShiftDetail.ToTime)
                    {
                        if (result.Count() == 0)
                        {
                            ShiftDetail.IsActive = true;
                            ShiftDetail.CreatedBy = currentUserId;
                            ShiftDetail.CreatedOn = DateTime.UtcNow;
                            ShiftDetail.UpdatedBy = currentUserId;
                            ShiftDetail.UpdatedOn = DateTime.UtcNow;
                            ShiftDetail.FromTime = ShiftDetail.FromTime;
                            ShiftDetail.ToTime = ShiftDetail.ToTime;
                            _genericService.ShitfMaster.Insert(ShiftDetail);
                            Message = "Shift saved successfully!!";
                            success = true;
                        }
                        else
                        {
                            return new ReturnResult { Message = "Shift time already assigned to other shift", Success = false };
                        }
                    }
                    else
                    {
                        return new ReturnResult { Message = "Shift time invalid", Success = false };
                    }
                }
                else
                {
                    return new ReturnResult { Message = "Shift name already assigned to other shift", Success = false };
                }
            }
            else
            {
                var shiftFromDb = _genericService.ShitfMaster.GetById(ShiftDetail.Id);

                if (shiftFromDb != null)
                {
                    var data = _genericService.ShitfMaster.GetAll().Where(x => x.Id != ShiftDetail.Id && x.ShitfName == ShiftDetail.ShitfName.Trim()).ToList();
                    if (data.Count() == 0)
                    {
                        var result = _genericService.ShitfMaster.GetAll().Where(y => y.Id != ShiftDetail.Id && y.ToTime == ShiftDetail.ToTime && y.FromTime == ShiftDetail.FromTime).ToList();
                        if (result.Count() == 0)
                        {
                            if (ShiftDetail.FromTime != ShiftDetail.ToTime)
                            {
                                if (shiftFromDb != null)
                                {

                                    shiftFromDb.ShitfName = ShiftDetail.ShitfName;
                                    shiftFromDb.FromTime = ShiftDetail.FromTime;
                                    shiftFromDb.ToTime = ShiftDetail.ToTime;
                                    shiftFromDb.UpdatedBy = currentUserId;
                                    shiftFromDb.UpdatedOn = DateTime.UtcNow;
                                    ShiftDetail.IsActive = true;
                                    shiftFromDb.CreatedOn = shiftFromDb.CreatedOn;
                                    _genericService.ShitfMaster.Update(shiftFromDb);
                                    Message = "Shift update successfully!!";
                                    success = true;
                                };
                            }
                            else
                            {
                                return new ReturnResult { Message = "Shift time invalid", Success = false };
                            }
                        }
                        else
                        {
                            return new ReturnResult { Message = "Shift time already assigned to other shift", Success = false };
                        }
                    }
                    else
                    {
                        return new ReturnResult { Message = "Shift name already assigned to other shift", Success = false };
                    }
                }



            }
            _genericService.Commit();
            return new ReturnResult { Message = Message, Success = true };
        }
        [Route("~/Api/Shift/GetAllShift")]
        [HttpPost]
        public async Task<string> GetAllshift(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            if (user == null)
            {
                LstShiftDetailsVM = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true).AsEnumerable()
                             .Select(x => new ShiftDetailsVM
                             {
                                 Id = x.Id,
                                 ShitfName = x.ShitfName,
                                 ToTime = (x.ToTime),
                                 FromTime = x.FromTime,
                                 strFromTime = x.FromTime.ToString("hh:mm tt"),
                                 strToTime = x.ToTime.ToString("hh:mm tt"),
                                 CreatedOn = x.CreatedOn.ToString(),
                             }).AsQueryable();
            }
            else
            {
                string userid = user.Id;
                if (userid != null)
                {
                    LstShiftDetailsVM = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true && x.UpdatedBy == userid).AsEnumerable()
                                 .Select(x => new ShiftDetailsVM
                                 {
                                     Id = x.Id,
                                     ShitfName = x.ShitfName,
                                     ToTime = (x.ToTime),
                                     FromTime = x.FromTime,
                                     strFromTime = x.FromTime.ToString("hh:mm tt"),
                                     strToTime = x.ToTime.ToString("hh:mm tt"),
                                     CreatedOn = x.CreatedOn.ToString(),
                                 }).AsQueryable();

                }
            }
            if (LstShiftDetailsVM.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    LstShiftDetailsVM = LstShiftDetailsVM.Where(item =>
                        item.ShitfName.ToLower().Contains(globalSearch.ToLower()) || item.strFromTime.ToLower().Contains(globalSearch.ToLower()) ||
                        item.strToTime.ToLower().Contains(globalSearch.ToLower())).AsQueryable();
                }
                if (string.IsNullOrEmpty(sortField))
                {
                    sortField = "ShitfName";
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
                   GenericSorterPager.GetSortedPagedList<ShiftDetailsVM>(LstShiftDetailsVM, paginationRequest, out totalCount);
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
                    if (_genericService.ShitfAssignment.SearchFor(x => x.ShitfId == ShitfMaster.Id && x.IsActive == true).Any())
                    {
                        return new ReturnResult { Message = "Please first delete all the shifts under this shift assignment", Success = false };
                    }
                    ShitfDelete.IsActive = false;
                    _genericService.ShitfMaster.Delete(ShitfDelete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Shift delete successful!!", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }
    }
}
