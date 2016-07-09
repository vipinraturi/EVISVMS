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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        [Route("~/Api/swift/SaveShift")]
        [HttpPost]
        public ReturnResult Saveswift([FromBody]  ShitfMaster ShitfMaster)
        {
            if (ShitfMaster.Id == 0)
            {
                ShitfMaster.IsActive = true;
                _genericService.ShitfMaster.Insert(ShitfMaster);
            }

            else
            {
                var Shift = _genericService.ShitfMaster.GetById(ShitfMaster.Id);
                if (Shift != null)
                {
                    Shift.ShitfName = ShitfMaster.ShitfName;
                    Shift.FromTime = ShitfMaster.FromTime;
                    Shift.ToTime = ShitfMaster.ToTime;
                    ShitfMaster.IsActive = true;
                    _genericService.ShitfMaster.Update(ShitfMaster);
                };
            }
            _genericService.Commit();
            return new ReturnResult { Message = "Success", Success = true };
        }
        [Route("~/Api/Shift/GetAllShift")]
        [HttpPost]
        public string GetAllshift(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var ShitfMaster = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true).ToList()
                            .Select(x => new ShitfMaster
                            {
                                Id = x.Id,
                                ShitfName = x.ShitfName,
                                ToTime = x.ToTime,
                                FromTime = x.FromTime
                            })
                            .ToList();
            if (ShitfMaster.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    ShitfMaster = ShitfMaster.Where(item =>
                        item.ShitfName.ToLower().Contains(globalSearch.ToLower())
                        ).ToList();
                }


                bool sortAscending = (sortOrder == "ASC" ? true : false);
                if (!string.IsNullOrEmpty(sortField))
                {
                    if (!sortAscending)
                    {
                        ShitfMaster = ShitfMaster
                               .OrderBy(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                               .ToList();
                    }
                    else
                    {
                        ShitfMaster = ShitfMaster
                               .OrderByDescending(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                               .ToList();
                    }
                }
            }

            var data = ShitfMaster.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var jsonData = JsonConvert.SerializeObject(data);
            var total = ShitfMaster.Count();
            return JsonConvert.SerializeObject(new { totalRows = total, result = jsonData });
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
