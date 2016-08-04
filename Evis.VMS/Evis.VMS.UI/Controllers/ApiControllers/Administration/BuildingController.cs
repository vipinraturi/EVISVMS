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

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        [Route("~/Api/Administration/SaveBuilding")]
        [HttpPost]
        public ReturnResult SaveBuilding([FromBody] BuildingMaster buildingMaster)
        {
            string currentUserId = HttpContext.Current.User.Identity.GetUserId();

            if (buildingMaster.Id == 0)
            {
                var data = _genericService.BuildingMaster.GetAll().Where(x => x.BuildingName == buildingMaster.BuildingName.Trim() && x.OrganizationId == buildingMaster.OrganizationId).ToList();
                if (data.Count() == 0)
                {
                    buildingMaster.IsActive = true;
                    buildingMaster.CreatedBy = currentUserId;
                    buildingMaster.CreatedOn = DateTime.UtcNow;
                    buildingMaster.UpdatedBy = currentUserId;
                    buildingMaster.UpdatedOn = DateTime.UtcNow;
                    _genericService.BuildingMaster.Insert(buildingMaster);
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
                    existingOrg.Address = buildingMaster.Address;
                    existingOrg.BuildingName = buildingMaster.BuildingName;
                    existingOrg.CityId = buildingMaster.CityId;
                    existingOrg.ZipCode = buildingMaster.ZipCode;
                    existingOrg.OrganizationId = existingOrg.OrganizationId;
                    buildingMaster.IsActive = true;
                    existingOrg.EmailId = buildingMaster.EmailId;
                    existingOrg.ContactNumber = buildingMaster.ContactNumber;
                    existingOrg.FaxNumber = buildingMaster.FaxNumber;
                    existingOrg.WebSite = buildingMaster.WebSite;
                    existingOrg.UpdatedBy = currentUserId;
                    existingOrg.UpdatedOn = DateTime.UtcNow;
                    _genericService.BuildingMaster.Update(existingOrg);
                };
            }
            _genericService.Commit();
            return new ReturnResult { Message = "Success", Success = true };
        }
        [Route("~/Api/Administration/GetBuildingData")]
        [HttpPost]
        public string GetBuildingData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var lstBuildingVM = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive == true)
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
                    EmailId=x.EmailId,
                    ContactNumber=x.ContactNumber,
                    FaxNumber=x.FaxNumber,
                    WebSite=x.WebSite

                });
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

                var jsonData = JsonConvert.SerializeObject(result.OrderByDescending(x=>x.Id));
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
                    if (_genericService.GateMaster.SearchFor(x => x.BuildingId == BuildingMaster.Id && x.IsActive==true).Any())
                    {
                        return new ReturnResult { Message = "Please first delete all the gates under this building", Success = false };
                    }
                    BuildingMasterDelete.IsActive = false;
                    _genericService.BuildingMaster.Update(BuildingMasterDelete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Success", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }

        [Route("~/Api/Administration/GetAllStateOrCity")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllStateOrCity(int Id)
        {
            var result = _genericService.LookUpValues.GetAll().Where(x => x.ParentId == Id && x.IsActive == true && x.LookUpType.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.LookUpValue });
            return result;
        }

    }
}
