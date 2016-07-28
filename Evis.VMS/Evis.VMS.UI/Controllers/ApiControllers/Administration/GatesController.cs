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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        [Route("~/Api/Gates/SaveGate")]
        [HttpPost]
        public ReturnResult SaveGate([FromBody]  GateMaster GateMaster)
        {
            if (GateMaster.Id == 0)
            {
                var data = _genericService.GateMaster.GetAll().Where(x => x.GateNumber == GateMaster.GateNumber.Trim() && x.BuildingId == GateMaster.BuildingId).ToList();
                if (data.Count() == 0)
                {
                    GateMaster.IsActive = true;
                    _genericService.GateMaster.Insert(GateMaster);
                }
                else
                {
                    return new ReturnResult { Message = "UnSuccess", Success = false };
                }
            }
            else
            {
                var existinggate = _genericService.GateMaster.GetById(GateMaster.Id);
                if (existinggate != null)
                {
                    existinggate.BuildingId = GateMaster.BuildingId;
                    existinggate.GateNumber = GateMaster.GateNumber;
                    GateMaster.IsActive = true;
                    _genericService.GateMaster.Update(existinggate);
                };
            }
            _genericService.Commit();
            return new ReturnResult { Message = "Success", Success = true };
        }
        [Route("~/Api/Gates/GetAllGate")]
        [HttpPost]
        public string GetAllGate(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var lstgateVM = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true)
                .Select(x => new GatesVM
                {
                    Id = x.Id,
                    BuildingId = x.Id,
                    GateNumber = x.GateNumber,
                    BuildingName = x.BuildingMaster.BuildingName

                });
            if (lstgateVM.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    lstgateVM = lstgateVM.Where(item =>
                        item.GateNumber.ToLower().Contains(globalSearch.ToLower()) ||
                         item.BuildingName.ToLower().Contains(globalSearch.ToLower())
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
                IList<GatesVM> result =
                   GenericSorterPager.GetSortedPagedList<GatesVM>(lstgateVM, paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result.OrderByDescending(x=>x.Id));
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }
            return null;
        }
        [Route("~/Api/Gates/GetAllBuildingDetails")]
        [HttpGet]
        public IList<BuildingMaster> GetAllBuildingDetails(int Id)
        {
            if (Id != 0)
            {
                var result = _genericService.BuildingMaster.GetAll().Where(x => x.Id == Id && x.Organization.IsActive == true && x.IsActive == true).ToList();
                return result;
            }
            return null;
        }
        [Route("~/Api/Gates/DeleteGate")]
        [HttpPost]
        public ReturnResult DeleteGate([FromBody] GateMaster GateMaster)
        {
            if (GateMaster != null)
            {
                var GaterDelete = _genericService.GateMaster.GetAll().Where(x => x.Id == GateMaster.Id).FirstOrDefault();
                if (GaterDelete != null)
                {
                    GaterDelete.IsActive = false;
                    _genericService.GateMaster.Update(GaterDelete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Success", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }
        [Route("~/Api/Gates/GetAllBuilding")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllBuilding()
        {
            var result = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.BuildingName });
            return result;
        }

    }
}
