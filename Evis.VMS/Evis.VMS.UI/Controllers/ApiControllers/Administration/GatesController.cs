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
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        IQueryable<GatesVM> lstgateVM;
        [Route("~/Api/Gates/SaveGate")]
        [HttpPost]
        public ReturnResult SaveGate([FromBody]  GateMaster gateMaster)
        {
            bool success = false;
            string message = "";
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            if (gateMaster.Id == 0)
            {
                var data = _genericService.GateMaster.GetAll().Where(x => x.GateNumber.Trim() == gateMaster.GateNumber.Trim() && x.BuildingId == gateMaster.BuildingId && x.IsActive == true).ToList();
                if (data.Count() == 0)
                {
                    gateMaster.IsActive = true;
                    gateMaster.CreatedBy = currentUserId;
                    gateMaster.CreatedOn = DateTime.UtcNow;
                    gateMaster.UpdatedBy = currentUserId;
                    gateMaster.UpdatedOn = DateTime.UtcNow;
                    gateMaster.GateNumber = gateMaster.GateNumber.Trim();
                    _genericService.GateMaster.Insert(gateMaster);
                    message = "Gate saved successfully!!";
                    success = true;
                }
                else
                {
                    return new ReturnResult { Message = "UnSuccess", Success = false };
                }
            }
            else
            {
                var existinggate = _genericService.GateMaster.GetById(gateMaster.Id);
                if (existinggate != null)
                {
                    var data = _genericService.GateMaster.GetAll().Where(x => x.Id != gateMaster.Id && x.GateNumber.Trim() == gateMaster.GateNumber.Trim() && x.BuildingId == gateMaster.BuildingId && x.IsActive == true).ToList();
                    if (data.Count() == 0)
                    {
                        existinggate.BuildingId = gateMaster.BuildingId;
                        existinggate.GateNumber = gateMaster.GateNumber.Trim();
                        existinggate.UpdatedBy = currentUserId;
                        existinggate.UpdatedOn = DateTime.UtcNow;
                        existinggate.CreatedOn = existinggate.CreatedOn;
                        gateMaster.IsActive = true;
                        _genericService.GateMaster.Update(existinggate);
                        message = "Gate update successfully!!";
                        success = true;
                    }
                    else
                    {
                        return new ReturnResult { Message = "UnSuccess", Success = false };
                    }
                };
            }
            _genericService.Commit();
            return new ReturnResult { Message = message, Success = true };
        }
        [Route("~/Api/Gates/GetAllGate")]
        [HttpPost]
        public async Task<string> GetAllGate(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();

            if (user == null)
            {
                lstgateVM = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true)
                   .Select(x => new GatesVM
                   {
                       Id = x.Id,
                       BuildingId = x.BuildingId,
                       GateNumber = x.GateNumber,
                       BuildingName = x.BuildingMaster.BuildingName,
                       State = x.BuildingMaster.StateMaster.LookUpValue,
                       Country = x.BuildingMaster.CountryMaster.LookUpValue,
                       City = x.BuildingMaster.CityMaster.LookUpValue,
                       OtherCity = x.BuildingMaster.OtherCity
                   });
            }
            else
            {
                int orgId = user.Organization.Id;
                if (orgId != null)
                {

                    lstgateVM = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true && x.BuildingMaster.OrganizationId == orgId)
                                  .Select(x => new GatesVM
                                  {
                                      Id = x.Id,
                                      BuildingId = x.BuildingId,
                                      GateNumber = x.GateNumber,
                                      BuildingName = x.BuildingMaster.BuildingName,
                                      State = x.BuildingMaster.StateMaster.LookUpValue,
                                      Country = x.BuildingMaster.CountryMaster.LookUpValue,
                                      City = x.BuildingMaster.CityMaster.LookUpValue,
                                      OtherCity = x.BuildingMaster.OtherCity
                                  });

                }
            }
            if (lstgateVM.Count() > 0)
           {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    lstgateVM = lstgateVM.Where(item =>
                        item.GateNumber.ToLower().Contains(globalSearch.ToLower()) ||
                         item.BuildingName.ToLower().Contains(globalSearch.ToLower()) ||
                         item.Country.ToLower().Contains(globalSearch.ToLower()) ||
                         item.State.ToLower().Contains(globalSearch.ToLower()) || 
                         item.City.ToLower().Contains(globalSearch.ToLower())||
                         item.OtherCity.ToLower().Contains(globalSearch.ToLower()))
                         .AsQueryable();
                }
                if (string.IsNullOrEmpty(sortField))
                {
                    sortField = "GateNumber";
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


                result.ToList().ForEach(item =>
                {
                    if (item.City == "Others")
                    {
                        item.City = item.OtherCity;
                    }
                });

                var jsonData = JsonConvert.SerializeObject(result);
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
                    if (_genericService.ShitfAssignment.SearchFor(x => x.GateId == GateMaster.Id && x.IsActive == true).Any())
                    {
                        return new ReturnResult { Message = "Please first delete all the shift assigment under this gate", Success = false };
                    }
                    GaterDelete.IsActive = false;
                    _genericService.GateMaster.Delete(GaterDelete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Gate deleted successfully!!", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }
        [Route("~/Api/Gates/GetAllBuilding")]
        [HttpGet]
        public async Task<IEnumerable<GeneralDropDownVM>> GetAllBuilding()
        {
            IQueryable<GeneralDropDownVM> result;
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            if (user == null)
            {
                result = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive == true)
                   .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.BuildingName });
            }
            else
            {
                int orgId = user.Organization.Id;
                result = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive == true && x.OrganizationId == orgId)
                      .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.BuildingName });
            }
            return result.OrderByDescending(x => x.Id);
        }

    }
}
