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
using Microsoft.AspNet.Identity;
using System.Web;
using System.Threading.Tasks;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        IQueryable<BuildingVM> lstBuildingVM;
        LookUpValues city;



        [Route("~/Api/Administration/SaveBuilding")]
        [HttpPost]
        public ReturnResult SaveBuilding([FromBody] BuildingVM buildingMaster)
        {
            bool success = false;
            string message = "";
            string currentUserId = HttpContext.Current.User.Identity.GetUserId();
            if (buildingMaster.Id == 0)
            {
                var data = _genericService.BuildingMaster.GetAll().Where(x => x.BuildingName == buildingMaster.BuildingName.Trim() && x.OrganizationId == buildingMaster.OrganizationId).ToList();
                if (data.Count() == 0)
                {
                    //if (buildingMaster.CityId == null)
                    //{
                    //    var country = _genericService.LookUpValues.Insert(new LookUpValues { LookUpTypeId = 2, ParentId = null, LookUpValue = buildingMaster.txtcountry, Description = buildingMaster.txtcountry, IsActive = true });
                    //    _genericService.Commit();
                    //    var state = _genericService.LookUpValues.Insert(new LookUpValues { LookUpTypeId = 3, ParentId = country.Id, LookUpValue = buildingMaster.txtstate, Description = buildingMaster.txtstate, IsActive = true });
                    //    _genericService.Commit();
                    //    city = _genericService.LookUpValues.Insert(new LookUpValues { LookUpTypeId = 4, ParentId = state.Id, LookUpValue = buildingMaster.txtcity, Description = buildingMaster.txtcity, IsActive = true });
                    //    _genericService.Commit();
                    //}
                    //else
                    //{
                    BuildingMaster obj = new BuildingMaster();
                    obj.Address = buildingMaster.Address.Trim();
                    obj.BuildingName = buildingMaster.BuildingName.Trim();
                    obj.CityId = buildingMaster.CityId;
                    obj.ZipCode = buildingMaster.ZipCode.Trim();
                    obj.OrganizationId = buildingMaster.OrganizationId;
                    obj.IsActive = true;
                    obj.EmailId = buildingMaster.EmailId.Trim();
                    obj.ContactNumber = buildingMaster.ContactNumber.Trim();
                    obj.FaxNumber = buildingMaster.FaxNumber.Trim();
                    obj.WebSite = buildingMaster.WebSite.Trim();
                    obj.UpdatedBy = currentUserId;
                    obj.UpdatedOn = DateTime.UtcNow;
                    obj.CreatedOn = DateTime.UtcNow;
                    obj.CreatedBy = currentUserId;
                    obj.OtherCountry = (buildingMaster.CityId == null) ? buildingMaster.txtcountry.Trim() : null;
                    obj.OtherState = (buildingMaster.CityId == null) ? buildingMaster.txtstate.Trim() : null;
                    obj.OtherCity = (buildingMaster.CityId == null) ? buildingMaster.txtcity.Trim() : null;
                    _genericService.BuildingMaster.Insert(obj);
                    message = "Building saved successfully!!";
                    success = true;
                }
                else
                {
                    return new ReturnResult { Message = "UnSuccess", Success = false };
                }
            }
            else
            {
                var existingOrg = _genericService.BuildingMaster.GetById(buildingMaster.Id);
                if (existingOrg != null)
                {
                    var data = _genericService.BuildingMaster.GetAll().Where(x => x.BuildingName == buildingMaster.BuildingName.Trim() && x.OrganizationId == buildingMaster.OrganizationId).ToList();
                    if (data.Count() == 0)
                    {
                        existingOrg.Address = buildingMaster.Address.Trim();
                        existingOrg.BuildingName = buildingMaster.BuildingName.Trim();
                        existingOrg.CityId = buildingMaster.CityId;
                        existingOrg.ZipCode = buildingMaster.ZipCode.Trim();
                        existingOrg.OrganizationId = existingOrg.OrganizationId;
                        buildingMaster.IsActive = true;
                        existingOrg.EmailId = buildingMaster.EmailId.Trim();
                        existingOrg.ContactNumber = buildingMaster.ContactNumber.Trim();
                        existingOrg.FaxNumber = buildingMaster.FaxNumber.Trim();
                        existingOrg.WebSite = buildingMaster.WebSite.Trim();
                        existingOrg.UpdatedBy = currentUserId;
                        existingOrg.UpdatedOn = DateTime.UtcNow;
                        existingOrg.OtherCountry = (buildingMaster.CityId == null) ? buildingMaster.txtcountry.Trim() : null;
                        existingOrg.OtherState = (buildingMaster.CityId == null) ? buildingMaster.txtstate.Trim() : null;
                        existingOrg.OtherCity = (buildingMaster.CityId == null) ? buildingMaster.txtcity.Trim() : null;
                        _genericService.BuildingMaster.Update(existingOrg);
                        message = "Building update successfully!!";
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

        [Route("~/Api/Administration/GetBuildingData")]
        [HttpPost]
        public async Task<string> GetBuildingData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            if (user == null)
            {
                lstBuildingVM = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive == true)
             .Select(x => new BuildingVM
             {
                 Id = x.Id,
                 BuildingName = x.BuildingName,
                 OrganizationId = x.OrganizationId,
                 CityId = x.CityId,
                 Address = x.Address,
                 ZipCode = x.ZipCode,
                 NationalityId = x.CityMaster.ParentValues.ParentId,
                 StateId = x.CityMaster.ParentId,
                 OrganizationName = x.Organization.CompanyName,
                 EmailId = x.EmailId,
                 ContactNumber = x.ContactNumber,
                 FaxNumber = x.FaxNumber,
                 WebSite = x.WebSite,
                 txtcountry = x.OtherCountry,
                 txtstate = x.OtherState,
                 txtcity = x.OtherCity,
                 CreatedOn = x.CreatedOn.ToString()
             });
            }
            else
            {
                int orgId = user.Organization.Id;
                if (orgId != 0)
                {
                    lstBuildingVM = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive == true && x.OrganizationId == orgId)
                 .Select(x => new BuildingVM
                 {
                     Id = x.Id,
                     BuildingName = x.BuildingName,
                     OrganizationId = x.OrganizationId,
                     CityId = x.CityId,
                     Address = x.Address,
                     ZipCode = x.ZipCode,
                     NationalityId = x.CityMaster.ParentValues.ParentId,
                     StateId = x.CityMaster.ParentId,
                     OrganizationName = x.Organization.CompanyName,
                     EmailId = x.EmailId,
                     ContactNumber = x.ContactNumber,
                     FaxNumber = x.FaxNumber,
                     WebSite = x.WebSite,
                     txtcountry = x.OtherCountry,
                     txtstate = x.OtherState,
                     txtcity = x.OtherCity,
                     CreatedOn = x.CreatedOn.ToString()

                 });
                }
                //int orgId = user.Organization.Id;
                //organizations = _genericService.Organization.GetAll()
                //                    .Where(x => x.IsActive == true && x.Id == orgId)
                //                    .Select(x => new GeneralDropDownVM { Id = x.Id, Name = x.CompanyName });
            }

            if (lstBuildingVM.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    lstBuildingVM = lstBuildingVM.Where(item =>
                        item.Address.ToLower().Contains(globalSearch.ToLower()) ||
                        item.BuildingName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.ZipCode.ToLower().Contains(globalSearch.ToLower())
                        ).AsQueryable();
                }


                if (string.IsNullOrEmpty(sortField))
                {
                    sortField = "CreatedOn";

                }

                var paginationRequest = new PaginationRequest
                {
                    PageIndex = (pageIndex - 1),
                    PageSize = pageSize,
                    SearchText = globalSearch,
                    Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
                };

                int totalCount = 0;
                IList<BuildingVM> result =
                   GenericSorterPager.GetSortedPagedList<BuildingVM>(lstBuildingVM, paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result.OrderByDescending(x => x.Id));
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }
            return null;
        }
        [Route("~/Api/Administration/DeleteBuilding")]
        [HttpPost]
        public ReturnResult DeleteBuilding([FromBody] BuildingMaster BuildingMaster)
        {
            if (BuildingMaster != null)
            {
                var BuildingMasterDelete = _genericService.BuildingMaster.GetAll().Where(x => x.Id == BuildingMaster.Id).FirstOrDefault();
                if (BuildingMasterDelete != null)
                {
                    if (_genericService.GateMaster.SearchFor(x => x.BuildingId == BuildingMaster.Id && x.IsActive == true).Any())
                    {
                        return new ReturnResult { Message = "Please first delete all the gates under this building", Success = false };
                    }
                    BuildingMasterDelete.IsActive = false;
                    _genericService.BuildingMaster.Delete(BuildingMasterDelete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Building deleted successfully!!", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }

        [Route("~/Api/Administration/GetAllStateOrCity")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllStateOrCity(int Id)
        {

            IEnumerable<GeneralDropDownVM> result;
            result = _genericService.LookUpValues.GetAll().Where(x => x.ParentId == Id && x.IsActive == true && x.LookUpType.IsActive == true)
                             .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.LookUpValue });
            return result.OrderByDescending(x => x.Id);
        }


    }
}
