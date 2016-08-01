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
        [Route("~/Api/Administration/SaveBuilding")]
        [HttpPost]
        public ReturnResult SaveBuilding([FromBody] BuildingMaster BuildingMaster)
        {
            if (BuildingMaster.Id == 0)
            {
                var data = _genericService.BuildingMaster.GetAll().Where(x => x.BuildingName == BuildingMaster.BuildingName.Trim() && x.OrganizationId == BuildingMaster.OrganizationId).ToList();
                if (data.Count() == 0)
                {
                    BuildingMaster.IsActive = true;
                    _genericService.BuildingMaster.Insert(BuildingMaster);
                }
                else
                {
                    return new ReturnResult { Message = "UnSuccess", Success = false };

                }
            }
            else
            {
                var existingOrg = _genericService.BuildingMaster.GetById(BuildingMaster.Id);
                if (existingOrg != null)
                {
                    existingOrg.Address = BuildingMaster.Address;
                    existingOrg.BuildingName = BuildingMaster.BuildingName;
                    existingOrg.CityId = BuildingMaster.CityId;
                    existingOrg.ZipCode = BuildingMaster.ZipCode;
                    existingOrg.OrganizationId = existingOrg.OrganizationId;
                    BuildingMaster.IsActive = true;
                    existingOrg.EmailId = BuildingMaster.EmailId;
                    existingOrg.ContactNumber = BuildingMaster.ContactNumber;
                    existingOrg.FaxNumber = BuildingMaster.FaxNumber;
                    existingOrg.WebSite = BuildingMaster.WebSite;
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
